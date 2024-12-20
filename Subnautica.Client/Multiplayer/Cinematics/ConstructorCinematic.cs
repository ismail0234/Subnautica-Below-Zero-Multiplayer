namespace Subnautica.Client.Multiplayer.Cinematics
{
    using Subnautica.Client.MonoBehaviours.Player;

    using UnityEngine;

    public class ConstructorCinematic : CinematicController
    {
        /**
         *
         * Yatağı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::ConstructorCinematicController Constructor { get; set; }

        /**
         *
         * Animasyonu resetler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnResetAnimations(PlayerCinematicQueueItem item)
        {
            this.Constructor = this.Target.GetComponentInChildren<global::ConstructorCinematicController>();
            this.Constructor.animator.Play(AnimatorHashID.deployed, 0, 1f);
            this.Constructor.animator.SetBool(AnimatorHashID.deployed, true);
        }

        /**
         *
         * Binme animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void EngageStartCinematic()
        {
            this.Constructor.ResetAnimParams(this.PlayerAnimator);

            this.SetCinematic(this.Constructor.engageCinematicController);
            this.StartCinematicMode();
        }

        /**
         *
         * İnme animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void DisengageStartCinematic()
        {
            this.EngageConditionSimulate();

            this.Constructor.ResetAnimParams(this.PlayerAnimator);

            this.SetCinematic(this.Constructor.disengageCinematicController);
            this.StartCinematicMode();
        }

        /**
         *
         * Engage koşul simülasyonunu çalıştırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void EngageConditionSimulate()
        {
            this.Constructor.animator.Play(this.Constructor.engageCinematicController.animParam, 0, 1f);
            this.Constructor.animator.SetBool(this.Constructor.engageCinematicController.animParam, true);

            this.PlayerAnimator.speed = 99f;
            this.PlayerAnimator.SetTrigger(this.Constructor.engageCinematicController.playerViewAnimationName);

            for (int i = 0; i < 10; i++)
            {
                this.PlayerAnimator.Update(Time.deltaTime == 0f ? 0.01f : Time.deltaTime);
            }

            this.PlayerAnimator.speed = 1f;
        }
    }
}
