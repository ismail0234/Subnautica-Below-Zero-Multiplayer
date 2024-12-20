namespace Subnautica.Events.EventArgs
{
    using System;

    public class PlayerPingColorChangedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public PlayerPingColorChangedEventArgs(string uniqueId, int colorIndex)
        {
            this.UniqueId   = uniqueId;
            this.ColorIndex = colorIndex;
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
         * ColorIndex Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public int ColorIndex { get; private set; }
    }
}