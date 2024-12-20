namespace Subnautica.Events.EventArgs
{
    using System;

    public class BenchSitdownEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BenchSitdownEventArgs(string uniqueId, Bench.BenchSide side, TechType techType, bool isAllowed = true)
        {
            this.UniqueId  = uniqueId;
            this.Side      = side;
            this.TechType  = techType;
            this.IsAllowed = isAllowed;
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
         * Side Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Bench.BenchSide Side { get; set; }

        /**
         *
         * TechType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; set; }

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
