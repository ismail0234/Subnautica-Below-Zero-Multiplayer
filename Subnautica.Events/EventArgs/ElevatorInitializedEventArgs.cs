namespace Subnautica.Events.EventArgs
{
    using System;

    public class ElevatorInitializedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ElevatorInitializedEventArgs(Rocket rocket)
        {
            this.Instance = rocket;
        }

        /**
         *
         * Instance Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Rocket Instance { get; set; }
    }
}