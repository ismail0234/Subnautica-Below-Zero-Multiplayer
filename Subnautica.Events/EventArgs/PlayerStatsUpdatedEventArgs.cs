namespace Subnautica.Events.EventArgs
{
    using System;

    public class PlayerStatsUpdatedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public PlayerStatsUpdatedEventArgs(float health, float food, float water)
        {
            this.Health = health;
            this.Food = food;
            this.Water = water;
        }

        /**
         *
         * Sağlık değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float Health { get; }

        /**
         *
         * Animasyonun çalıştırılıp/çalıştırılmayacağı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float Food { get; }

        /**
         *
         * Su değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float Water { get; }
    }
}
