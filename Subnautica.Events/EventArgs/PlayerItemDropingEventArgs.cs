namespace Subnautica.Events.EventArgs
{
    using System;

    using UnityEngine;

    public class PlayerItemDropingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public PlayerItemDropingEventArgs(string uniqueId, Pickupable item, Vector3 position, Quaternion rotation,  bool isAllowed = true)
        {
            this.UniqueId  = uniqueId;
            this.Item      = item;
            this.Position  = position;
            this.Rotation  = rotation;
            this.IsAllowed = isAllowed;
        }

        /**
         *
         * Yapı Kimliği değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; private set; }

        /**
         *
         * Item değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Pickupable Item { get; private set; }

        /**
         *
         * Position değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 Position { get; private set; }

        /**
         *
         * Rotation değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Quaternion Rotation { get; private set; }

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
