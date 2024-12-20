namespace Subnautica.Events.EventArgs
{
    using System;

    public class BaseHullStrengthCrushingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BaseHullStrengthCrushingEventArgs(global::BaseHullStrength instance, bool isAllowed = true)
        {
            this.Instance  = instance;
            this.IsAllowed = isAllowed;
        }

        /**
         *
         * Instance Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::BaseHullStrength Instance { get; set; }

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
