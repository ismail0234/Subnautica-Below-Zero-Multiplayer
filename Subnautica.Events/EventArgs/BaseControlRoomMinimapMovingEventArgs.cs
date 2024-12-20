namespace Subnautica.Events.EventArgs
{
    using System;

    using UnityEngine;

    public class BaseControlRoomMinimapMovingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BaseControlRoomMinimapMovingEventArgs(string uniqueId, Vector3 position)
        {
            this.UniqueId = uniqueId;
            this.Position = position;
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
    }
}
