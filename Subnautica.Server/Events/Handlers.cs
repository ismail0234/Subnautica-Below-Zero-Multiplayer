namespace Subnautica.Server.Events
{
    using Subnautica.Server.Events.EventArgs;

    using static Subnautica.API.Extensions.EventExtensions;

    public class Handlers
    {
        /**
         *
         * PlayerFullConnected İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PlayerFullConnectedEventArgs> PlayerFullConnected;

        /**
         *
         * PlayerFullConnected Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlayerFullConnected(PlayerFullConnectedEventArgs ev) => PlayerFullConnected.CustomInvoke(ev);

        /**
         *
         * PlayerDisconnected İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<PlayerDisconnectedEventArgs> PlayerDisconnected;

        /**
         *
         * PlayerDisconnected Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlayerDisconnected(PlayerDisconnectedEventArgs ev) => PlayerDisconnected.CustomInvoke(ev);
    }
}
