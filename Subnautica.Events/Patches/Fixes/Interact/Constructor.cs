namespace Subnautica.Events.Patches.Fixes.Interact
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::ConstructorInput), nameof(global::ConstructorInput.OnHandHover))]
    public class Constructor
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::ConstructorInput __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (Network.HandTarget.IsBlocked(Network.Identifier.GetIdentityId(__instance.constructor.gameObject)))
            {
                Interact.ShowUseDenyMessage();
                return false;
            }

            return true;
        }
    }
}
