namespace Subnautica.Events.EventArgs
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    public class SpyPenguinDeployingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public SpyPenguinDeployingEventArgs(string uniqueId, Pickupable pickupable, float health, string name, Vector3 position, Quaternion rotation,  bool isAllowed = true)
        {
            this.UniqueId   = uniqueId;
            this.Pickupable = pickupable;
            this.Name       = name;
            this.Health     = health;
            this.Position   = position;
            this.Rotation   = rotation;
            this.IsAllowed  = isAllowed;
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
         * Name Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string Name { get; set; }

        /**
         *
         * Health Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float Health { get; set; }

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
         * IsAllowed Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}