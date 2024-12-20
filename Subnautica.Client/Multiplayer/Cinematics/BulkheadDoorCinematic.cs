namespace Subnautica.Client.Multiplayer.Cinematics
{
    using Subnautica.API.Features;
    using Subnautica.Client.MonoBehaviours.Player;

    public class BulkheadDoorCinematic : CinematicController
    {
        /**
         *
         * Yatağı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::BulkheadDoor Door { get; set; }

        /**
         *
         * Animasyonu resetler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnResetAnimations(PlayerCinematicQueueItem item)
        {
            this.Door = this.Target.GetComponentInChildren<global::BulkheadDoor>();
        }

        /**
         *
         * Kapıyı açma animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OpenDoorStartCinematic()
        {
            var doorSide = this.GetProperty<bool>("Side");

            this.Door.animator.SetBool(global::BulkheadDoor.animPlayerInFront, doorSide);

            this.RegisterProperty("IsOpen", true);
            this.SetCinematic(this.GetCinematic(doorSide, true));
            this.SetCinematicEndMode(this.CinematicModeEnd);
            this.StartCinematicMode();
        }

        /**
         *
         * Kapıyı kapatma animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void CloseDoorStartCinematic()
        {
            var doorSide = this.GetProperty<bool>("Side");

            this.Door.animator.SetBool(global::BulkheadDoor.animPlayerInFront, doorSide);

            this.RegisterProperty("IsOpen", false);
            this.SetCinematic(this.GetCinematic(doorSide, false));
            this.SetCinematicEndMode(this.CinematicModeEnd);
            this.StartCinematicMode();
        }

        /**
         *
         * Sinematik bittiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void CinematicModeEnd()
        {
            this.Door.SetState(this.GetProperty<bool>("IsOpen"));
        }

        /**
         *
         * Sinematiği döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::PlayerCinematicController GetCinematic(bool side, bool isOpen)
        {
            if (!side)
            {
                if (!this.Door.shouldPassThrough || !this.Door.backPassthroughCinematicController)
                {
                    return isOpen ? this.Door.backOpenCinematicController : this.Door.backCloseCinematicController;
                }

                return this.Door.backPassthroughCinematicController;
            }
            else
            {
                if (!this.Door.shouldPassThrough || !this.Door.backPassthroughCinematicController)
                {
                    return isOpen ? this.Door.frontOpenCinematicController : this.Door.frontCloseCinematicController;
                }

                return this.Door.frontPassthroughCinematicController;
            }
        }
    }
}
