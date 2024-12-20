namespace Subnautica.Events.EventArgs
{
    using System;

    public class ShieldBaseEnterTriggeringEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ShieldBaseEnterTriggeringEventArgs(bool isAllowed = true)
        {
            this.IsAllowed = isAllowed;
        }

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
