namespace Subnautica.Events.Patches.Events.Game
{
    using HarmonyLib;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;
    using System;

    [HarmonyPatch(typeof(MainMenuLoadButton), nameof(MainMenuLoadButton.CancelDelete))]
    public class MenuSaveCancelDeleteButtonClicking
    {        
        private static bool Prefix(MainMenuLoadButton __instance)
        {
            try
            {
                MenuSaveCancelDeleteButtonClickingEventArgs args = new MenuSaveCancelDeleteButtonClickingEventArgs(__instance.sessionId);

                Handlers.Game.OnMenuSaveCancelDeleteButtonClicking(args);

                __instance.sessionId = args.SessionId;

                if (args.IsAllowed)
                {
                    return true;
                }

                if (args.IsRunAnimation)
                {
                    __instance.StartCoroutine(__instance.ShiftAlpha(__instance.loadCg, 1f, __instance.animTime, __instance.alphaPower, true));
                    __instance.StartCoroutine(__instance.ShiftAlpha(__instance.deleteCg, 0.0f, __instance.animTime, __instance.alphaPower, false));
                    __instance.StartCoroutine(__instance.ShiftPos(__instance.loadCg, MainMenuLoadButton.target.centre, MainMenuLoadButton.target.left, __instance.animTime, __instance.posPower));
                    __instance.StartCoroutine(__instance.ShiftPos(__instance.deleteCg, MainMenuLoadButton.target.right, MainMenuLoadButton.target.centre, __instance.animTime, __instance.posPower));
                }

                return false;
            }
            catch (Exception e)
            {
                Log.Error($"MenuSaveCancelDeleteButtonClicking.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}
