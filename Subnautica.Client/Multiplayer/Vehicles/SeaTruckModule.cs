namespace Subnautica.Client.Multiplayer.Vehicles
{
    public class SeaTruckModule : VehicleController
    {
        /**
         *
         * SeaTruck aracını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::SeaTruckMotor Vehicle { get; set; }

        /**
         *
         * Her karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        /**
         *
         * Oyuncu araca bindiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnEnterVehicle()
        {
            base.OnEnterVehicle();

            if (this.Management.Vehicle)
            {
                this.Vehicle = this.Management.Vehicle.GetComponent<global::SeaTruckMotor>();

                this.SetPlayerParent(this.Vehicle.pilotPosition.transform);

                this.Vehicle.truckSegment.animator.SetBool("piloting", true);
                this.Management.Player.Animator.SetBool("seatruck_pushing", true);
            }
        }

        /**
         *
         * Oyuncu araçtan indiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnExitVehicle()
        {
            if (this.Vehicle)
            {
                this.Vehicle.truckSegment.animator.SetBool("piloting", false);
            }
            
            this.Management.Player.Animator.SetBool("seatruck_pushing", false);

            base.OnExitVehicle();
        }
    }
}
