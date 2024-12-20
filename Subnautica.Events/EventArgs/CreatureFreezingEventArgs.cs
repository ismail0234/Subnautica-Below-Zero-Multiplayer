namespace Subnautica.Events.EventArgs
{
    using System;

    public class CreatureFreezingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public CreatureFreezingEventArgs(string uniqueId, float lifeTime, string brinicleId = null, bool isAllowed = true)
        {
            this.UniqueId   = uniqueId;
            this.LifeTime   = lifeTime;
            this.BrinicleId = brinicleId;
            this.IsAllowed  = isAllowed;
        }

        /**
         *
         * Yaratık benzersiz ID değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; set; }

        /**
         *
         * LifeTime değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float LifeTime { get; set; }

        /**
         *
         * InIce değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string BrinicleId { get; set; }

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
