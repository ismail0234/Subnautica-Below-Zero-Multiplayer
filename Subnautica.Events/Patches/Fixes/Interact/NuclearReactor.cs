namespace Subnautica.Events.Patches.Fixes.Interact
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::BaseNuclearReactorGeometry), nameof(global::BaseNuclearReactorGeometry.OnHover))]
    public class NuclearReactor
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::BaseNuclearReactorGeometry __instance)
        {
            if(!Network.IsMultiplayerActive)
            {
                return true;
            }

            BaseDeconstructable constructable = __instance.gameObject.GetComponent<BaseDeconstructable>();
            if (constructable == null)
            {
                return false;
            }

            if (Interact.IsBlocked(Network.Identifier.GetIdentityId(constructable.gameObject)))
            {
                Interact.ShowUseDenyMessage();
                return false;
            }

            return true;
        }
    }
}
