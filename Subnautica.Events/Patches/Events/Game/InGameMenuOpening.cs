namespace Subnautica.Events.Patches.Events.Game
{
    using HarmonyLib;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;
    using System;

    [HarmonyPatch(typeof(IngameMenu), nameof(IngameMenu.OnSelect))]
    public class InGameMenuOpening
    {
        private static bool Prefix(IngameMenu __instance)
        {
            if(!Network.IsMultiplayerActive)
            {
                return true;
            }

            try
            {
                InGameMenuOpeningEventArgs args = new InGameMenuOpeningEventArgs();

                Handlers.Game.OnInGameMenuOpening(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"InGameMenuOpening.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}
