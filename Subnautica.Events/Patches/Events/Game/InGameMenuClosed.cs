namespace Subnautica.Events.Patches.Events.Game
{
    using HarmonyLib;
    
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(IngameMenu), nameof(IngameMenu.OnDeselect))]
    public class InGameMenuClosed
    {
        private static void Postfix(IngameMenu __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    InGameMenuClosedEventArgs args = new InGameMenuClosedEventArgs();

                    Handlers.Game.OnInGameMenuClosed(args);
                }
                catch (Exception e)
                {
                    Log.Error($"InGameMenuClosed.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}
