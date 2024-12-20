namespace Subnautica.Client.Multiplayer.Cinematics
{
    using Subnautica.API.Extensions;
    using Subnautica.Client.MonoBehaviours.Player;

    public class SeaTruckTeleportationCinematic : CinematicController
    {
        /**
         *
         * Yatağı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::SeaTruckTeleporter Teleporter { get; set; }

        /**
         *
         * Animasyonu resetler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnResetAnimations(PlayerCinematicQueueItem item)
        {
            this.Teleporter = this.Target.GetComponentInChildren<global::SeaTruckTeleporter>();
        }

        /**
         *
         * Yatma animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SeaTruckTeleportationStartCinematic()
        {
            this.Teleporter.arrivalVFX.Play();

            this.SetCinematic(this.Teleporter.arrivalCinematic);
            this.SetCinematicEndMode(this.TeleportationEnd);
            this.StartCinematicMode();
        }

        /**
         *
         * Işınlanma bittiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void TeleportationEnd()
        {
            if (this.Teleporter && this.ZeroPlayer.IsInSeaTruck)
            {
                this.ZeroPlayer.GetComponent<PlayerAnimation>().UpdateIsInSeaTruck(true);
            }
        }
    }
}
