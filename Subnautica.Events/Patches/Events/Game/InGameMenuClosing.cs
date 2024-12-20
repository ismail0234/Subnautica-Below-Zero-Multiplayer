namespace Subnautica.Events.Patches.Events.Game
{
    using HarmonyLib;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;
    using System;

    [HarmonyPatch(typeof(IngameMenu), nameof(IngameMenu.OnDeselect))]
    public class InGameMenuClosing
    {
        private static bool Prefix(IngameMenu __instance)
        {
            if(!Network.IsMultiplayerActive)
            {
                return true;
            }

            try
            {
                InGameMenuClosingEventArgs args = new InGameMenuClosingEventArgs();

                Handlers.Game.OnInGameMenuClosing(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"InGameMenuClosing.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}
