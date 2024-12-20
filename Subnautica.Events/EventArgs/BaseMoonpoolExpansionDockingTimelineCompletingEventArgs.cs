namespace Subnautica.Events.EventArgs
{
    using System;

    using UnityEngine;

    public class BaseMoonpoolExpansionDockingTimelineCompletingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BaseMoonpoolExpansionDockingTimelineCompletingEventArgs(GameObject gameObject, bool isAllowed = true)
        {
            this.GameObject = gameObject;
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
         * Olayın çalıştırılıp/çalıştırılmayacağı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}
