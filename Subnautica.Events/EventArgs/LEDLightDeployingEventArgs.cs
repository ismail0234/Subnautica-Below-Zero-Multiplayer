namespace Subnautica.Events.EventArgs
{
    using System;

    using UnityEngine;

    public class LEDLightDeployingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public LEDLightDeployingEventArgs(string uniqueId, Vector3 position, Quaternion rotation, bool isAllowed = true)
        {
            this.UniqueId  = uniqueId;
            this.Position  = position;
            this.Rotation  = rotation;
            this.IsAllowed = isAllowed;
        }

        /**
         *
         * UniqueId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; set; }

        /**
         *
         * Position değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 Position { get; set; }

        /**
         *
         * Rotation değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Quaternion Rotation { get; set; }

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