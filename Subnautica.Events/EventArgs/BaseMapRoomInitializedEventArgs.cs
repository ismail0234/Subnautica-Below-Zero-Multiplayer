namespace Subnautica.Events.EventArgs
{
    using System;

    public class BaseMapRoomInitializedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BaseMapRoomInitializedEventArgs(uGUI_MapRoomScanner mapRoom)
        {
            this.MapRoom = mapRoom;
        }

        /**
         *
         * MapRoom Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public uGUI_MapRoomScanner MapRoom { get; set; }
    }
}
