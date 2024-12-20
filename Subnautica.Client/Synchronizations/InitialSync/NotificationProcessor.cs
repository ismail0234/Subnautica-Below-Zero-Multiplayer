namespace Subnautica.Client.Synchronizations.InitialSync
{
    using Subnautica.API.Enums;
    using Subnautica.API.Features;

    public class NotificationProcessor
    {
        /**
         *
         * Bildirim verileri yüklendikten sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnNotificationInitialized()
        {
            if (Network.Session.Current.PlayerNotifications != null)
            {
                using (EventBlocker.Create(ProcessType.NotificationAdded))
                using (EventBlocker.Create(TechType.Sign))
                {
                    foreach (var notification in Network.Session.Current.PlayerNotifications)
                    {
                        if (notification.IsPing)
                        {
                            NotificationProcessor.HandlePingInstance(notification.Key, notification.IsVisible, notification.ColorIndex);
                        }

                        if (notification.IsViewed)
                        {
                            NotificationManager.main.Remove(notification.Group, notification.Key);
                            continue;
                        }

                        if (notification.Group == NotificationManager.Group.Blueprints)
                        {
                            if (notification.Key.DecodeKey() == TechType.None)
                            {
                                continue;
                            }
                        }
                        else if (notification.Group == NotificationManager.Group.Builder)
                        {
                            if (notification.Key.DecodeKey() == TechType.None || !TechData.GetBuildable(notification.Key.DecodeKey()))
                            {
                                continue;
                            }
                        }
                        else if (notification.Group == NotificationManager.Group.CraftTree)
                        {
                            if (notification.Key.DecodeKey() == TechType.None || !CraftTree.IsCraftable(notification.Key.DecodeKey()))
                            {
                                continue;
                            }
                        }
                        else if (notification.Group == NotificationManager.Group.Gallery)
                        {
                            if (!ScreenshotManager.HasScreenshotForFile(notification.Key))
                            {
                                continue;
                            }
                        }
                        else if (notification.Group == NotificationManager.Group.Encyclopedia)
                        {
                            if (!PDAEncyclopedia.HasEntryData(notification.Key))
                            {
                                continue;
                            }
                        }
                        else if (notification.Group == NotificationManager.Group.Log)
                        {
                            if (!PDALog.Contains(notification.Key))
                            {
                                continue;
                            }
                        }

                        NotificationManager.main.Add(notification.Group, notification.Key, 2f);
                    }

                    foreach (var notification in NotificationManager.main.notifications.Keys.ToSet())
                    {
                        if (notification.group == NotificationManager.Group.Gallery || notification.group == NotificationManager.Group.Inventory)
                        {
                            NotificationManager.main.Remove(notification.group, notification.key);
                        }
                    }
                }
            }
        }

        /**
         *
         * Ping işaretini ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void HandlePingInstance(string uniqueId, bool isVisible, sbyte colorIndex)
        {
            var pingInstance = Network.Identifier.GetComponentByGameObject<global::PingInstance>(uniqueId, true);
            if (pingInstance)
            {
                pingInstance.visible    = isVisible;
                pingInstance.colorIndex = colorIndex;

                if (!pingInstance.initialized)
                {
                    pingInstance.Start();
                }

                PingManager.NotifyVisible(pingInstance);
                PingManager.NotifyColor(pingInstance);
            }
        }
    }
}