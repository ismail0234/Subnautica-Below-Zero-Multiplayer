namespace Subnautica.Events.Patches.Events.Game
{
    using HarmonyLib;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;
    using System;

    [HarmonyPatch(typeof(IngameMenu), nameof(IngameMenu.OnSelect))]
    public class InGameMenuOpened
    {
        private static void Postfix(IngameMenu __instance)
        {
            if(Network.IsMultiplayerActive)
            {
                try
                {
                    InGameMenuOpenedEventArgs args = new InGameMenuOpenedEventArgs();

                    Handlers.Game.OnInGameMenuOpened(args);
                }
                catch (Exception e)
                {
                    Log.Error($"InGameMenuOpened.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}
