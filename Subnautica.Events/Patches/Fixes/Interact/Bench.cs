namespace Subnautica.Events.Patches.Fixes.Interact
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::Bench), nameof(global::Bench.OnHandHover))]
    public class Bench
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::Bench __instance, GUIHand hand)
        {
            if(!Network.IsMultiplayerActive)
            {
                return true;
            }

            __instance.isValidHandTarget = __instance.GetCanBeUsedBy(hand.player);
            if (!__instance.isValidHandTarget || !hand.IsFreeToInteract())
            {
                return false;
            }

            Constructable component = __instance.gameObject.GetComponent<Constructable>();
            if (component && !component.constructed)
            {
                return false;

            }

            if (Network.HandTarget.IsBlocked(Network.Identifier.GetIdentityId(component.gameObject)))
            {
                Interact.ShowUseDenyMessage();
                return false;
            }

            return true;
        }
    }
}
