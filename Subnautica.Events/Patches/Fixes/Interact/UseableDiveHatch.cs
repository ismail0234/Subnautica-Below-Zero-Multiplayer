namespace Subnautica.Events.Patches.Fixes.Interact
{
    using HarmonyLib;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;

    using Subnautica.Events.Patches.Events.Player;

    [HarmonyPatch(typeof(global::UseableDiveHatch), nameof(global::UseableDiveHatch.OnHandHover))]
    public class UseableDiveHatch
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::UseableDiveHatch __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!__instance.enabled)
            {
                return false;
            }

            var uniqueId = UseableDiveHatchClicking.GetUniqueId(__instance);
            if (uniqueId.IsNull())
            {
                return true;
            }

            if (Network.HandTarget.IsBlocked(uniqueId))
            {
                Interact.ShowUseDenyMessage();
                return false;
            }

            return true;
        }
    }
}
