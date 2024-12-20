namespace Subnautica.API.Features.Creatures.MonoBehaviours
{
    using Subnautica.Network.Structures;

    using UnityEngine;

    using static Jellyfish;

    public class JellyFishMonobehaviour : BaseMultiplayerCreature
    {
        /**
         *
         * Jellyfish sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::Jellyfish Jellyfish { get; set; }

        /**
         *
         * MaxDistanceToTarget değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float MaxDistanceToTarget { get; set; }

        /**
         *
         * En yakındaki oyuncuyu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GameObject NearestPlayer;

        /**
         *
         * Enson en yakındaki oyuncuyu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GameObject LastNearestPlayer;

        /**
         *
         * Sınıf uyanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Awake()
        {
            this.Jellyfish = this.GetComponent<global::Jellyfish>();
            this.MaxDistanceToTarget = this.Jellyfish.maxDistanceToTarget * this.Jellyfish.maxDistanceToTarget;
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
            this.UpdateEyeState();
            this.UpdateEyeAnimation(Time.deltaTime);
        }

        /**
         *
         * Her sabit karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void FixedUpdate()
        {
            this.NearestPlayer = this.GetNearestPlayer();
        }

        /**
         *
         * Hareket ve göz durumunu ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void UpdateEyeState()
        {   
            var eyeState = this.Jellyfish.eyeState;

            if (this.Jellyfish.eyeState == EyeState.FollowPlayer && !this.NearestPlayer)
            {
                eyeState = EyeState.TransitionToIdle;
            }
            else if (this.Jellyfish.eyeState == EyeState.FollowPlayer && this.LastNearestPlayer != this.NearestPlayer)
            {
                eyeState = EyeState.TransitionToIdle;
            }
            else if (this.Jellyfish.eyeState != EyeState.FollowPlayer && this.NearestPlayer)
            {
                eyeState = EyeState.FollowPlayer;
            }
            else if (this.Jellyfish.eyeState == EyeState.TransitionToIdle && this.Jellyfish.timeEyeStateChanged + 1f < Time.time)
            {
                eyeState = EyeState.Idle;
            }

            if (this.Jellyfish.eyeState != eyeState)
            {
                this.Jellyfish.eyeState = eyeState;
                this.Jellyfish.timeEyeStateChanged = Time.time;
                this.Jellyfish.GetAnimator().SetBool(animEyeFollowPlayer, this.Jellyfish.eyeState != EyeState.Idle);
                
                this.LastNearestPlayer = this.NearestPlayer;

                if (this.MultiplayerCreature.CreatureItem.IsMine())
                {
                    if (this.Jellyfish.eyeState == EyeState.FollowPlayer)
                    {
                        this.Jellyfish.presenceSfx.Play();
                        this.Jellyfish.swimToTarget.SetTarget(this.NearestPlayer);
                        this.Jellyfish.TryStartAction(this.Jellyfish.swimToTarget);
                    }
                    else
                    {
                        this.Jellyfish.presenceSfx.Stop();
                        this.Jellyfish.swimToTarget.SetTarget(null);
                    }
                }
            }
        }

        /**
         *
         * Göz animasyonunu ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void UpdateEyeAnimation(float deltaTime)
        {
            if (this.Jellyfish.eyeState == EyeState.TransitionToIdle)
            {
                this.Jellyfish.GetAnimator().SetFloat(animEyeLookDepth, 0f, 1f, deltaTime);
            }
            else if (this.Jellyfish.eyeState == EyeState.FollowPlayer && this.NearestPlayer)
            {
                var to  = this.Jellyfish.eye.InverseTransformPoint(this.NearestPlayer.transform.position);
                var num = 1f - to.normalized.y;

                this.Jellyfish.GetAnimator().SetFloat(Jellyfish.animEyeLookDepth, num, 1f, deltaTime);

                to.y = 0.0f;

                this.Jellyfish.eyeRotation = Mathf.LerpAngle(this.Jellyfish.eyeRotation, -Mathf.Sign(to.x) * Vector3.Angle(Vector3.forward, to), 2f * deltaTime);

                while (this.Jellyfish.eyeRotation < 0.0f)
                {
                    this.Jellyfish.eyeRotation += 360f;
                }

                while (this.Jellyfish.eyeRotation > 360.0f)
                {
                    this.Jellyfish.eyeRotation -= 360f;
                }

                this.Jellyfish.GetAnimator().SetFloat(Jellyfish.animEyeRotation, this.Jellyfish.eyeRotation);
            }
        }

        /**
         *
         * En yakındaki oyuncuyu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GameObject GetNearestPlayer()
        {
            var currentDistance = 10000f;
            var currentPlayer   = (GameObject) null;

            foreach (var player in ZeroPlayer.GetPlayers())
            {
                var distance = ZeroVector3.Distance(player.Position, this.transform.position);
                if (distance < this.MaxDistanceToTarget && distance < currentDistance)
                {
                    currentDistance = distance;
                    currentPlayer   = player.PlayerModel;
                }
            }

            var _distance = ZeroVector3.Distance(global::Player.main.transform.position, this.transform.position);
            if (_distance < this.MaxDistanceToTarget && _distance < currentDistance)
            {
                return global::Player.main.gameObject;
            }

            return currentPlayer;
        }
    }
}
