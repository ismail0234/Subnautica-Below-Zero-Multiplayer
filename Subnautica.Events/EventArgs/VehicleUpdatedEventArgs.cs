namespace Subnautica.Events.EventArgs
{
    using System;

    using UnityEngine;

    public class VehicleUpdatedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public VehicleUpdatedEventArgs(string uniqueId, Vector3 position, Quaternion rotation, TechType techType, GameObject gameObject)
        {
            this.UniqueId = uniqueId;
            this.Position = position;
            this.Rotation = rotation;
            this.TechType = techType;
            this.Instance = gameObject;
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
         * TechType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; private set; }

        /**
         *
         * Instance Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public GameObject Instance { get; private set; }
    }
}
