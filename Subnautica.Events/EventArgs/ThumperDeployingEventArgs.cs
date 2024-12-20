namespace Subnautica.Events.EventArgs
{
    using System;

    using UnityEngine;

    public class ThumperDeployingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ThumperDeployingEventArgs(string uniqueId, Pickupable pickupable, Vector3 deployPosition, float charge, bool isAllowed = true)
        {
            this.UniqueId       = uniqueId;
            this.Pickupable     = pickupable;
            this.DeployPosition = deployPosition;
            this.Charge         = charge;
            this.IsAllowed      = isAllowed;
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
         * Pickupable Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Pickupable Pickupable { get; set; }

        /**
         *
         * DeployPosition Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 DeployPosition { get; set; }

        /**
         *
         * Charge Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float Charge { get; set; }

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