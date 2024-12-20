namespace Subnautica.Events.EventArgs
{
    using System;

    using UnityEngine;

    public class CosmeticItemPlacingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public CosmeticItemPlacingEventArgs(string uniqueId, string baseId, TechType techType, Vector3 position, Quaternion rotation, bool isAllowed = true)
        {
            this.UniqueId = uniqueId;
            this.BaseId   = baseId;
            this.TechType = techType;
            this.Position = position;
            this.Rotation = rotation;
            this.IsAllowed = isAllowed;
        }

        /**
         *
         * UniqueId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; private set; }

        /**
         *
         * BaseId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string BaseId { get; private set; }

        /**
         *
         * TechType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; private set; }

        /**
         *
         * Position Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 Position { get; private set; }

        /**
         *
         * Rotation Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Quaternion Rotation { get; private set; }

        /**
         *
         * IsAllowed Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}