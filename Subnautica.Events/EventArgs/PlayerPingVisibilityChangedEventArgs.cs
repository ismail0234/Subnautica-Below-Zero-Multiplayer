namespace Subnautica.Events.EventArgs
{
    using System;

    public class PlayerPingVisibilityChangedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public PlayerPingVisibilityChangedEventArgs(string uniqueId, bool isVisible)
        {
            this.UniqueId  = uniqueId;
            this.IsVisible = isVisible;
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
         * IsVisible Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsVisible { get; private set; }
    }
}