namespace Subnautica.Events.Patches.Events.Story
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch]
    public static class StoryCalling
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::uGUI_PopupNotification), nameof(global::uGUI_PopupNotification.Answer))]
        private static bool Answer(global::uGUI_PopupNotification __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.current.id != "Call")
            {
                return false;
            }

            if (!PDACalls.TryGet(__instance.current.data, out var callData))
            {
                return false;
            }

            try
            {
                StoryCallingEventArgs args = new StoryCallingEventArgs(__instance.current.data, callData.dialogue, true);

                Handlers.Story.OnStoryCalling(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"StoryCalling.Answer: {e}\n{e.StackTrace}");
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::uGUI_PopupNotification), nameof(global::uGUI_PopupNotification.Decline))]
        private static bool Decline(global::uGUI_PopupNotification __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.current.id != "Call")
            {
                return false;
            }

            if (!PDACalls.TryGet(__instance.current.data, out var callData))
            {
                return false;
            }

            try
            {
                StoryCallingEventArgs args = new StoryCallingEventArgs(__instance.current.data, callData.voiceMail, false);

                Handlers.Story.OnStoryCalling(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"StoryCalling.Decline: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }
}