namespace Subnautica.Client.Multiplayer.Cinematics
{
    using Subnautica.Client.MonoBehaviours.Player;

    public class ClimbCinematic : CinematicController
    {
        /**
         *
         * Yatağı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::CinematicModeTriggerBase Ladder;

        /**
         *
         * Yatağı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::CinematicModeTrigger ConstructorLadder;

        /**
         *
         * Animasyonu resetler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnResetAnimations(PlayerCinematicQueueItem item)
        {
            if (!this.Target.TryGetComponent<global::CinematicModeTriggerBase>(out this.Ladder))
            {
                this.ConstructorLadder = this.Target.GetComponentInChildren<global::CinematicModeTrigger>();
            }
        }

        /**
         *
         * Tırmanma sinematiğini çalıştırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void ClimbStartCinematic()
        {
            if (this.Ladder)
            {
                this.SetCinematic(this.Ladder.cinematicController);
            }

            if (this.ConstructorLadder)
            {
                this.SetCinematic(this.ConstructorLadder.cinematicController);
            }

            if (this.Ladder || this.ConstructorLadder)
            {
                this.StartCinematicMode();
            }
        }
    }
}
