namespace Subnautica.Events.Patches.Events.PDA
{
    using HarmonyLib;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    using static NotificationManager;

    [HarmonyPatch]
    public static class NotificationToggle
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(NotificationManager), nameof(NotificationManager.NotifyAdd))]
        private static void NotifyAdd(NotificationId id)
        {
            if (Network.IsMultiplayerActive && !EventBlocker.IsEventBlocked(ProcessType.NotificationAdded))
            {
                try
                {
                    NotificationToggleEventArgs args = new NotificationToggleEventArgs(id.group, id.key, true);

                    Handlers.PDA.OnNotificationToggle(args);
                }
                catch (Exception e)
                {
                    Log.Error($"NotificationToggle.NotifyAdd: {e}\n{e.StackTrace}");
                }
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(NotificationManager), nameof(NotificationManager.NotifyRemove))]
        private static void NotifyRemove(NotificationId id)
        {
            if (Network.IsMultiplayerActive && !EventBlocker.IsEventBlocked(ProcessType.NotificationAdded))
            {
                try
                {
                    NotificationToggleEventArgs args = new NotificationToggleEventArgs(id.group, id.key, false);

                    Handlers.PDA.OnNotificationToggle(args);
                }
                catch (Exception e)
                {
                    Log.Error($"NotificationToggle.NotifyRemove: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}
