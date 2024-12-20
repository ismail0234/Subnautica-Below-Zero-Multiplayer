namespace Subnautica.Events.Patches.Events.Player
{
    using System;

    using HarmonyLib;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::UseableDiveHatch), nameof(global::UseableDiveHatch.StartCinematicMode))]
    public static class UseableDiveHatchClicking
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::UseableDiveHatch __instance, global::PlayerCinematicController cinematicController, global::Player player)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (PlayerCinematicController.cinematicModeCount > 0)
            {
                return false;
            }

            var uniqueId = UseableDiveHatchClicking.GetUniqueId(__instance);
            if (uniqueId.IsNull())
            {
                return true;
            }

            try
            {
                UseableDiveHatchClickingEventArgs args = new UseableDiveHatchClickingEventArgs(uniqueId, __instance.enterCinematicController == cinematicController, cinematicController.playerViewAnimationName, __instance.GetComponentInParent<MoonpoolExpansionManager>());

                Handlers.Player.OnUseableDiveHatchClicking(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"UseableDiveHatchClicking.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }

        /**
         *
         * Yapı id'sini döner. yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetUniqueId(global::UseableDiveHatch __instance)
        {
            var lifepod = __instance.GetComponentInParent<LifepodDrop>();
            if (lifepod)
            {
                return lifepod.gameObject.GetIdentityId();
            }            

            var deconstructable = __instance.GetComponentInParent<BaseDeconstructable>();
            if (deconstructable)
            {
                return deconstructable.gameObject.GetIdentityId();
            }

            return null;
        }
    }
}