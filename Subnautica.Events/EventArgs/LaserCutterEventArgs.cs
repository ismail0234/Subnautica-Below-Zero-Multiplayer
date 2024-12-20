namespace Subnautica.Events.EventArgs
{
    using System;

    public class LaserCutterEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public LaserCutterEventArgs(string uniqueId, float amount, float maxAmount, bool isAllowed = true)
        {
            this.UniqueId  = uniqueId;
            this.Amount    = amount;
            this.MaxAmount = maxAmount;
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
         * Amount Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float Amount { get; set; }

        /**
         *
         * MaxAmount Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float MaxAmount { get; set; }

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
