namespace Subnautica.Client.Multiplayer.Cinematics
{
    using Subnautica.API.Extensions;
    using Subnautica.Client.MonoBehaviours.Player;

    public class SeaTruckDetachCinematic : CinematicController
    {
        /**
         *
         * Yatağı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::SeaTruckMotor SeaTruckMotor { get; set; }

        /**
         *
         * Animasyonu resetler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnResetAnimations(PlayerCinematicQueueItem item)
        {
            this.SeaTruckMotor = this.Target.GetComponentInChildren<global::SeaTruckMotor>();
            this.SeaTruckMotor.animator.Rebind();
        }

        /**
         *
         * Çözme animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void DetachSeaTruckCinematic()
        {
            if (this.SeaTruckMotor.seatruckanimation)
            {
                this.SetCinematic(this.SeaTruckMotor.seatruckanimation.GetController(SeaTruckAnimation.Animation.EjectModules));
                this.SetCinematicEndMode(this.DetachSeaTruckEndMode);
                this.StartCinematicMode();
            }
        }

        /**
         *
         * Binme bittiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void DetachSeaTruckEndMode()
        {
            if (this.ZeroPlayer.IsInSeaTruck)
            {
                this.ZeroPlayer.GetComponent<PlayerAnimation>().UpdateIsInSeaTruck(true);
            }
        }
    }
}
