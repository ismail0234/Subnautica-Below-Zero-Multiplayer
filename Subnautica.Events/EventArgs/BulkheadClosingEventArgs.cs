namespace Subnautica.Events.EventArgs
{
    using System;

    using Subnautica.API.Features;

    public class BulkheadClosingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BulkheadClosingEventArgs(string uniqueId, bool side, bool isAllowed = true)
        {
            this.UniqueId            = uniqueId;
            this.Side                = side;
            this.IsAllowed           = isAllowed;
            this.IsStaticWorldEntity = Network.StaticEntity.IsStaticEntity(uniqueId);
        }

        /**
         *
         * UniqueId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; set; }

        /**
         *
         * Side Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Side { get; set; }

        /**
         *
         * IsStaticWorldEntity değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsStaticWorldEntity { get; set; }

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
