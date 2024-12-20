namespace Subnautica.Events.Patches.Events.Game
{
    using HarmonyLib;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;
    using System;

    [HarmonyPatch(typeof(MainMenuLoadButton), nameof(MainMenuLoadButton.Delete))]
    public class MenuSaveDeleteButtonClicking
    {        
        private static bool Prefix(MainMenuLoadButton __instance)
        {
            try
            {
                MenuSaveDeleteButtonClickingEventArgs args = new MenuSaveDeleteButtonClickingEventArgs(__instance.sessionId);

                Handlers.Game.OnMenuSaveDeleteButtonClicking(args);

                __instance.sessionId = args.SessionId;

                if (args.IsAllowed)
                {
                    return true;
                }

                if (args.IsRunAnimation)
                {
                    __instance.StartCoroutine(__instance.ShiftPos(__instance.deleteCg, MainMenuLoadButton.target.left, MainMenuLoadButton.target.centre, __instance.animTime, __instance.posPower));
                    __instance.StartCoroutine(__instance.ShiftAlpha(__instance.deleteCg, 0.0f, __instance.animTime, __instance.alphaPower, false));
                    __instance.StartCoroutine(__instance.FreeSlot(__instance.contentArea, __instance.slotAnimTime, __instance.slotPosPower));
                }

                return false;
            }
            catch (Exception e)
            {
                Log.Error($"MenuSaveDeleteButtonClickingEventArgs.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}
