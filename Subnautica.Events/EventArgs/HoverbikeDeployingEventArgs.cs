namespace Subnautica.Events.EventArgs
{
    using System;

    using UnityEngine;

    public class HoverbikeDeployingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public HoverbikeDeployingEventArgs(string uniqueId, Hoverbike hoverbike, Vector3 deployPosition, Vector3 forward, bool isAllowed = true)
        {
            this.UniqueId       = uniqueId;
            this.Hoverbike      = hoverbike;
            this.DeployPosition = deployPosition;
            this.Forward        = forward;
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
         * Hoverbike Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Hoverbike Hoverbike { get; set; }

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
         * Forward Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 Forward { get; set; }

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