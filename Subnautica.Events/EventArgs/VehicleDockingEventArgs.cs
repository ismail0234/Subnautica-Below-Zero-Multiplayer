namespace Subnautica.Events.EventArgs
{
    using System;

    using Subnautica.API.Features;

    using UnityEngine;

    public class VehicleDockingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public VehicleDockingEventArgs(string uniqueId, GameObject vehicle, TechType MoonpoolType, Vector3 backModulePosition, Vector3 endPosition, Quaternion endRotation, bool isAllowed = true)
        {
            this.UniqueId     = uniqueId;
            this.VehicleId    = Network.Identifier.GetIdentityId(vehicle, false);
            this.Vehicle      = vehicle;
            this.MoonpoolType = MoonpoolType;
            this.EndPosition  = endPosition;
            this.EndRotation  = endRotation;
            this.BackModulePosition = backModulePosition;
            this.IsAllowed    = isAllowed;
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
         * VehicleId Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string VehicleId { get; set; }

        /**
         *
         * Vehicle Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GameObject Vehicle { get; set; }

        /**
         *
         * MoonpoolType Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType MoonpoolType { get; set; }

        /**
         *
         * BackModulePosition Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 BackModulePosition { get; set; }

        /**
         *
         * EndPosition Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 EndPosition { get; set; }

        /**
         *
         * EndRotation Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Quaternion EndRotation { get; set; }

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