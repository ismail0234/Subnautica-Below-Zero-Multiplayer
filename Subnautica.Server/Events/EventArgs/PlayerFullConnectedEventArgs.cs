namespace Subnautica.Server.Events.EventArgs
{
    using System;

    using Subnautica.Server.Core;

    public class PlayerFullConnectedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public PlayerFullConnectedEventArgs(AuthorizationProfile player)
        {
            this.Player = player;
        }

        /**
         *
         * Player değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public AuthorizationProfile Player { get; set; }
    }
}