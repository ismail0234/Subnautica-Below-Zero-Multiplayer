namespace Subnautica.Client.Synchronizations.Processors.PDA
{
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class NotificationProcessor : NormalProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            return true;
        }

        /**
         *
         * Oyuncu istatistikleri alındığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPingVisibilityChanged(PlayerPingVisibilityChangedEventArgs ev)
        {
            NotificationProcessor.SendPacketToServer(ev.UniqueId, isVisible: ev.IsVisible);
        }

        /**
         *
         * Oyuncu istatistikleri alındığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPingColorChanged(PlayerPingColorChangedEventArgs ev)
        {
            NotificationProcessor.SendPacketToServer(ev.UniqueId, colorIndex: (sbyte) ev.ColorIndex);
        }

        /**
         *
         * PDA'dan bildirim kaldırılınca/eklenince tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnNotificationToggle(NotificationToggleEventArgs ev)
        {
            if (ev.Group != NotificationManager.Group.Gallery && ev.Group != NotificationManager.Group.Inventory)
            {
                NotificationProcessor.SendPacketToServer(ev.Key, ev.Group, ev.IsAdded, true);
            }
        }

        /**
         *
         * PDA'dan bildirim kaldırılınca/eklenince tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string key, NotificationManager.Group group = default, bool isAdded = false, bool isNotification = false, bool isVisible = false, sbyte colorIndex = -1)
        {
            ServerModel.NotificationAddedArgs result = new ServerModel.NotificationAddedArgs()
            {
                Key            = key,
                Group          = group,
                IsAdded        = isAdded,
                IsNotification = isNotification,
                IsVisible      = isVisible,
                ColorIndex     = colorIndex,
            };

            NetworkClient.SendPacket(result);
        }
    }
}