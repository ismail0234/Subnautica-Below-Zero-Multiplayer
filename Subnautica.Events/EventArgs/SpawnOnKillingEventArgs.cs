namespace Subnautica.Events.EventArgs
{
    using System;

    using UnityEngine;

    public class SpawnOnKillingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public SpawnOnKillingEventArgs(string uniqueId, TechType techType, Vector3 position, Quaternion rotation, Vector3 velocity, ForceMode forceMode, bool isAllowed = true)
        {
            this.UniqueId  = uniqueId;
            this.TechType  = techType;
            this.Position  = position;
            this.Rotation  = rotation;
            this.Velocity  = velocity;
            this.ForceMode = forceMode;
            this.IsAllowed = isAllowed;
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
         * TechType Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; set; }

        /**
         *
         * Position Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 Position { get; set; }

        /**
         *
         * Rotation Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Quaternion Rotation { get; set; }

        /**
         *
         * Velocity Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 Velocity { get; set; }

        /**
         *
         * ForceMode Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ForceMode ForceMode { get; set; }

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
