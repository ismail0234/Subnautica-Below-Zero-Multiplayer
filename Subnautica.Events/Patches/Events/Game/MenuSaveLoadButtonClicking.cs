namespace Subnautica.Events.Patches.Events.Game
{
    using HarmonyLib;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;
    using System;

    [HarmonyPatch(typeof(MainMenuLoadButton), nameof(MainMenuLoadButton.Load))]
    public class MenuSaveLoadButtonClicking
    {        
        private static bool Prefix(MainMenuLoadButton __instance)
        {
            try
            {
                if (__instance.IsEmpty())
                {
                    return false;
                }
                
                MenuSaveLoadButtonClickingEventArgs args = new MenuSaveLoadButtonClickingEventArgs(__instance.sessionId);

                Handlers.Game.OnMenuSaveLoadButtonClicking(args);

                __instance.sessionId = args.SessionId;

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"MenuSaveLoadButtonClicking.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}