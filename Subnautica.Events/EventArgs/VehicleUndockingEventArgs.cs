namespace Subnautica.Events.EventArgs
{
    using System;

    using UnityEngine;

    public class VehicleUndockingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public VehicleUndockingEventArgs(string uniqueId, string vehicleId, TechType MoonpoolType, Vector3 undockPosition, Quaternion undockRotation, bool isLeft, bool isAllowed = true)
        {
            this.UniqueId       = uniqueId;
            this.VehicleId      = vehicleId;
            this.MoonpoolType   = MoonpoolType;
            this.UndockPosition = undockPosition;
            this.UndockRotation = undockRotation;
            this.IsLeft         = isLeft;
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
         * VehicleId Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string VehicleId { get; set; }

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
         * UndockPosition Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 UndockPosition { get; set; }

        /**
         *
         * UndockRotation Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Quaternion UndockRotation { get; set; }

        /**
         *
         * IsLeft Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsLeft { get; set; }

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