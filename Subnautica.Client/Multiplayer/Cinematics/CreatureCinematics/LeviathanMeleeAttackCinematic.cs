namespace Subnautica.Client.Multiplayer.Cinematics.CreatureCinematics
{
    using FMODUnity;

    using Subnautica.API.Features;
    using Subnautica.Client.MonoBehaviours.Player;
    using Subnautica.Network.Structures;

    using UnityEngine;

    public class LeviathanMeleeAttackCinematic : CinematicController
    {
        /**
         *
         * Yatağı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::LeviathanMeleeAttack MeleeAttack { get; set; }

        /**
         *
         * Animasyonu resetler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnResetAnimations(PlayerCinematicQueueItem item)
        {
            this.MeleeAttack = this.Target.GetComponentInChildren<global::LeviathanMeleeAttack>();

            if (this.Target.TryGetComponent<global::AttackLastTarget>(out var attackLastTarget)) 
            {
                attackLastTarget.StopAttackSoundAndAnimation();
            }
        }

        /**
         *
         * Yakın dövüş saldırısını başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void StartMeleeAttack()
        {
            if (this.MeleeAttack)
            {
                this.MeleeAttack.timeLastBite = Time.time;

                this.SetCinematic(this.MeleeAttack.playerAttackCinematicController);
                this.StartCinematicMode();

                if (ZeroVector3.Distance(this.MeleeAttack.transform.position, global::Player.main.transform.position) < 3600f)
                {
                    FakeFMODByBenson.Instance.PlaySound(this.MeleeAttack.playerAttackSfx, this.Player.transform, 60f, this.IsValidSound);
                }
            }
        }

        /**
         *
         * Ses geçerlilik durumunu kontrol eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsValidSound(StudioEventEmitter eventEmitter, Transform attachedTransform)
        {
            return this.IsCinematicModeActive && this.Player && attachedTransform;
        }
    }
}
