namespace Subnautica.Client.Multiplayer.Cinematics
{
    using Subnautica.Client.MonoBehaviours.Player;

    using UnityEngine;

    public class BenchCinematic : CinematicController
    {
        /**
         *
         * Yatağı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::Bench Bench { get; set; }

        /**
         *
         * Animasyonu resetler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnResetAnimations(PlayerCinematicQueueItem item)
        {
            this.Bench = this.Target.GetComponent<global::Bench>();
            this.Bench.animator.Rebind();
        }

        /**
         *
         * Oturma animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SitDownStartCinematic()
        {
            this.Bench.animator.transform.localEulerAngles = this.GetPlayerSitdownAngles(this.GetProperty<global::Bench.BenchSide>("Side"));
            this.Bench.ResetAnimParams(this.PlayerAnimator);

            this.SetCinematic(this.Bench.cinematicController);
            this.StartCinematicMode();
        }

        /**
         *
         * Kalkma animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void StandupStartCinematic()
        {
            this.Bench.animator.transform.localEulerAngles = this.GetPlayerSitdownAngles(this.GetProperty<global::Bench.BenchSide>("Side"));
            this.Bench.ResetAnimParams(this.PlayerAnimator);

            this.SetCinematic(this.Bench.standUpCinematicController, true);
            this.PlayerViewAnimationName = this.GetPlayerStandupAnimationName();
            this.StartCinematicMode();
        }

        /**
         *
         * Oyuncu oturma konumunu döner..
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Vector3 GetPlayerSitdownAngles(global::Bench.BenchSide side)
        {
            return side == global::Bench.BenchSide.Front ? this.Bench.frontAnimRotation : this.Bench.backAnimRotation;
        }

        /**
         *
         * Oyuncu kalkma animasyon adını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private string GetPlayerStandupAnimationName()
        {
            if (this.Target.TryGetComponent<Constructable>(out var constructable) && constructable.techType == TechType.Bench)
            {
                return "stand_up";
            }

            return "chair_stand_up";
        }
    }
}
