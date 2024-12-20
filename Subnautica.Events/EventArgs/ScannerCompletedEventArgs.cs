namespace Subnautica.Events.EventArgs
{
    using System;

    public class ScannerCompletedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ScannerCompletedEventArgs(TechType techType)
        {
            this.TechType = techType;
        }

        /**
         *
         * TechType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; set; }
    }
}
