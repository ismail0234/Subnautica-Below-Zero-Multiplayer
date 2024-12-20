namespace Subnautica.Events.EventArgs
{
    using System;

    using UnityEngine;

    public class BaseMoonpoolExpansionUndockTailEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BaseMoonpoolExpansionUndockTailEventArgs(GameObject gameObject, bool withEjection, bool isAllowed = true)
        {
            this.GameObject   = gameObject;
            this.WithEjection = withEjection;
            this.IsAllowed    = isAllowed;
        }

        /**
         *
         * GameObject değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GameObject GameObject { get; set; }

        /**
         *
         * WithEjection değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool WithEjection { get; set; }

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
