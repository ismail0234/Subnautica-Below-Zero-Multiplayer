namespace Subnautica.Events.EventArgs
{
    using System;

    using UnityEngine;

    public class LifepodInterpolationEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public LifepodInterpolationEventArgs(GameObject dropObject, bool isAllowed = true)
        {
            this.DropObject  = dropObject;
            this.IsAllowed   = isAllowed;
        }

        /**
         *
         * DropObject değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GameObject DropObject { get; set; }

        /**
         *
         * IsCompleted değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsCompleted { get; set; }

        /**
         *
         * Rotation değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Quaternion Rotation { get; set; }

        /**
         *
         * TimeLeft değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float StartedTime { get; set; }

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