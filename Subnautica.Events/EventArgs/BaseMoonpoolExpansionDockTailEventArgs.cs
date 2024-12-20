namespace Subnautica.Events.EventArgs
{
    using System;

    using UnityEngine;

    public class BaseMoonpoolExpansionDockTailEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BaseMoonpoolExpansionDockTailEventArgs(GameObject gameObject, global::SeaTruckSegment newTail, bool isAllowed = true)
        {
            this.GameObject = gameObject;
            this.NewTail    = newTail;
            this.IsAllowed  = isAllowed;
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
         * NewTail değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::SeaTruckSegment NewTail { get; set; }

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
