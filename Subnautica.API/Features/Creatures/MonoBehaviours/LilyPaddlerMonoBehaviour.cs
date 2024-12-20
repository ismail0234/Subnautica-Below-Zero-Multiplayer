namespace Subnautica.API.Features.Creatures.MonoBehaviours
{
    using UnityEngine;

    public class LilyPaddlerMonoBehaviour : BaseMultiplayerCreature
    {
        /**
         *
         * Balina sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::LilyPaddlerHypnotize LilyPaddlerHypnotize { get; set; }

        /**
         *
         * Sınıf uyanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Awake()
        {
            this.LilyPaddlerHypnotize = this.GetComponent<global::LilyPaddlerHypnotize>();
        }

        /**
         *
         * Her karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Update()
        {
            if (this.MultiplayerCreature.CreatureItem.IsMine())
            {
                if (this.LilyPaddlerHypnotize.isActive)
                {
                    this.LilyPaddlerHypnotize.currentTarget = ZeroPlayer.CurrentPlayer.PlayerModel;

                    var deltaTime   = Time.deltaTime;
                    var difference  = this.LilyPaddlerHypnotize.currentTarget.transform.position - this.transform.position;
                    var magnitude   = difference.magnitude;
                    difference      = difference.normalized;

                    var quaternion  = Quaternion.LookRotation(-difference);
                    var eulerAngles = quaternion.eulerAngles;

                    var num = Mathf.Clamp01(this.LilyPaddlerHypnotize.distanceHypnFactor.Evaluate(Mathf.InverseLerp(this.LilyPaddlerHypnotize.minPullDistance, this.LilyPaddlerHypnotize.maxDistance, magnitude)) * this.LilyPaddlerHypnotize.lookAngleHypnFactor.Evaluate(Mathf.InverseLerp(1f, 0.65f, Vector3.Dot(-difference, MainCameraControl.main.transform.forward))));

                    this.LilyPaddlerHypnotize.hypnotizedScalar += this.LilyPaddlerHypnotize.hypnotizingSpeed * this.LilyPaddlerHypnotize.hypnotizingSpeedMultiplier.Evaluate(num) * deltaTime;
                    Player.main.mesmerizedSpeedMultiplier = this.LilyPaddlerHypnotize.mesmerizedPlayerSpeedMultiplier.Evaluate(this.LilyPaddlerHypnotize.hypnotizedScalar);
                    
                    var deltaLerp = Mathf.Lerp(this.LilyPaddlerHypnotize.lerpCameraMinSpeed, this.LilyPaddlerHypnotize.lerpCameraMaxSpeed, num) * deltaTime;

                    MainCameraControl.main.rotationX = Mathf.LerpAngle(MainCameraControl.main.rotationX, eulerAngles.y - ZeroPlayer.CurrentPlayer.PlayerModel.transform.localEulerAngles.y, deltaLerp);
                    MainCameraControl.main.rotationY = Mathf.LerpAngle(MainCameraControl.main.rotationY, -eulerAngles.x, deltaLerp);

                    if (Mathf.Abs(magnitude - this.LilyPaddlerHypnotize.minPullDistance) > 0.1f)
                    {
                        this.LilyPaddlerHypnotize.currentTarget.transform.position -= (magnitude > this.LilyPaddlerHypnotize.minPullDistance ? 1f : -1f) * difference * this.LilyPaddlerHypnotize.pullPlayerSpeed * deltaTime;
                    }

                    if (this.LilyPaddlerHypnotize.hypnotizedScalar >= 1.0f)
                    {
                        Player.main.lilyPaddlerHypnosis.Hypnotise();

                        this.LilyPaddlerHypnotize.StopPerform(Time.time);
                    }
                }
            }
        }

        /**
         *
         * Yaratık öldüğünde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnMultiplayerKill()
        {
            if (this.LilyPaddlerHypnotize.isActive)
            {
                this.LilyPaddlerHypnotize.StopPerform(Time.time);
            }
        }

        /**
         *
         * Yaratık öldüğünde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnChangedOwnership()
        {
            if (this.LilyPaddlerHypnotize.isActive)
            {
                this.LilyPaddlerHypnotize.StopPerform(Time.time);
            }
        }

        /**
         *
         * Hipnozu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void StartHypnotize(ZeroPlayer target, float hypnotizeTime)
        {
            target.SetLastHypnotizeTime(hypnotizeTime);

            if (target.IsMine)
            {
                this.LilyPaddlerHypnotize.lastTarget.SetTarget(target.PlayerModel);

                this.LilyPaddlerHypnotize.currentTarget    = this.LilyPaddlerHypnotize.lastTarget.target;
                this.LilyPaddlerHypnotize.isActive         = true;
                this.LilyPaddlerHypnotize.timeNextSwim     = Time.time;
                this.LilyPaddlerHypnotize.hypnotizedScalar = 0.0f;

                this.LilyPaddlerHypnotize.swimBehaviour.Idle();
                this.LilyPaddlerHypnotize.swimBehaviour.LookAt(this.LilyPaddlerHypnotize.currentTarget.transform);
                this.LilyPaddlerHypnotize.animator.SetBool(AnimatorHashID.attack, true);
                this.LilyPaddlerHypnotize.locomotion.freezeHorizontalRotation = false;

                this.LilyPaddlerHypnotize.loopingSound?.Play();
                this.LilyPaddlerHypnotize.screenFX?.StartHypnose();

                this.Update();
            }
        }
    }
}
