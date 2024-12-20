namespace Subnautica.Events.EventArgs
{
    using System;
    using System.Collections;

    public class IntroCheckingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public IntroCheckingEventArgs(bool isAllowed = true)
        {
            this.IsAllowed = isAllowed;
        }

        /**
         *
         * WaitingMethod değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public IEnumerator WaitingMethod { get; set; }

        /**
         *
         * IsAllowed değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}