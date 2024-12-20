namespace Subnautica.Events.EventArgs
{
    using System;

    using UnityEngine;

    public class OxygenPipePlacingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public OxygenPipePlacingEventArgs(string uniqueId, string parentId, string pipeId, Pickupable pickupable, Vector3 deployPosition, Quaternion deployRotation, bool isAllowed = true)
        {
            this.UniqueId       = uniqueId;
            this.ParentId       = parentId;   
            this.PipeId         = pipeId;
            this.Pickupable     = pickupable;
            this.DeployPosition = deployPosition;
            this.DeployRotation = deployRotation;
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
         * ParentId Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string ParentId { get; set; }

        /**
         *
         * PipeId Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string PipeId { get; set; }

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
         * DeployRotation Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Quaternion DeployRotation { get; set; }

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