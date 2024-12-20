namespace Subnautica.Events.EventArgs
{
    using System;

    public class MobileExtractorMachineSampleAddingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public MobileExtractorMachineSampleAddingEventArgs(bool isAllowed = true)
        {
            this.IsAllowed = isAllowed;
        }

        /**
         *
         * IsAllowed Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}