namespace Subnautica.Events.EventArgs
{
    using System;

    public class SpyPenguinSnowStalkerInteractingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public SpyPenguinSnowStalkerInteractingEventArgs(string uniqueId, float spawnChance, bool isAllowed = true)
        {
            this.UniqueId    = uniqueId;
            this.SpawnChance = spawnChance;
            this.IsAllowed   = isAllowed;
        }

        /**
         *
         * UniqueId Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; set; }

        /**
         *
         * SpawnChance Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float SpawnChance { get; set; }

        /**
         *
         * Olayın çalıştırılıp/çalıştırılmayacağı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}
