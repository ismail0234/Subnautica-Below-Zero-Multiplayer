namespace Subnautica.Events.Patches.Fixes.Interact
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(RadioTowerController), nameof(RadioTowerController.OnHandHover))]
    public class RadioTower
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::RadioTowerController __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.itemInsertedStoryGoal.IsComplete())
            {
                return false;
            }

            if (Network.HandTarget.IsBlocked(Network.Identifier.GetIdentityId(__instance.gameObject)))
            {
                Interact.ShowUseDenyMessage();
                return false;
            }

            return true;
        }
    }
}
