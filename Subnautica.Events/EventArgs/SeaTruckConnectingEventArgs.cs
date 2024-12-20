namespace Subnautica.Events.EventArgs
{
    using System;

    public class SeaTruckConnectingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public SeaTruckConnectingEventArgs(string frontModuleId, string backModuleId, string firstModuleId, bool isConnect, bool isMoonpoolExpansion, bool isAllowed = true)
        {
            this.FrontModuleId = frontModuleId;
            this.BackModuleId  = backModuleId;
            this.FirstModuleId = firstModuleId;
            this.IsConnect     = isConnect;
            this.IsMoonpoolExpansion = isMoonpoolExpansion;
            this.IsAllowed     = isAllowed;
        }

        /**
         *
         * FrontModuleId Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string FrontModuleId { get; set; }

        /**
         *
         * BackModuleId Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string BackModuleId { get; set; }

        /**
         *
         * FirstModuleId Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string FirstModuleId { get; set; }

        /**
         *
         * IsConnect Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsConnect { get; set; }

        /**
         *
         * IsMoonpoolExpansion Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsMoonpoolExpansion { get; set; }

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
