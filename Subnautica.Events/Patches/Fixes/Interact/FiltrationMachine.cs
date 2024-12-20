namespace Subnautica.Events.Patches.Fixes.Interact
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::FiltrationMachine), nameof(global::FiltrationMachine.OnHover))]
    public class FiltrationMachine
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::FiltrationMachine __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            var model = __instance.GetModel();
            if (model == null)
            {
                return true;
            }

            BaseDeconstructable constructable = model.GetComponent<BaseDeconstructable>();
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
