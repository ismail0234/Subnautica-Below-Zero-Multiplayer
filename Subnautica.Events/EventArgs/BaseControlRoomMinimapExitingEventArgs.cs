namespace Subnautica.Events.EventArgs
{
    using System;

    using UnityEngine;

    public class BaseControlRoomMinimapExitingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BaseControlRoomMinimapExitingEventArgs(string uniqueId, Vector3 mapPosition)
        {
            this.UniqueId    = uniqueId;
            this.MapPosition = mapPosition;
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
         * MapPosition değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Vector3 MapPosition { get; set; }
    }
}
