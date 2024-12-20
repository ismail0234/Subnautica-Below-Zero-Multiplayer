namespace Subnautica.Events.EventArgs
{
    using System;

    using UnityEngine;

    public class MapRoomCameraDockingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public MapRoomCameraDockingEventArgs(string uniqueId, string vehicleId, Vector3 endPosition, Quaternion endRotation, bool isLeft, bool isAllowed = true)
        {
            this.UniqueId    = uniqueId;
            this.VehicleId   = vehicleId;
            this.EndPosition = endPosition;
            this.EndRotation = endRotation;
            this.IsLeft      = isLeft;
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
         * VehicleId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string VehicleId { get; set; }

        /**
         *
         * EndPosition değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 EndPosition { get; set; }

        /**
         *
         * EndRotation değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Quaternion EndRotation { get; set; }

        /**
         *
         * IsLeft değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsLeft { get; set; }

        /**
         *
         * Olayın çalıştırılıp/çalıştırılmayacağı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}
