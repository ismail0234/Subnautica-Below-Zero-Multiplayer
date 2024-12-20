namespace Subnautica.Events.Patches.Fixes.Interact
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch]
    public class Pickupable
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Pickupable), nameof(global::Pickupable.OnHandHover))]
        private static bool OnHandHover(global::Pickupable __instance, GUIHand hand)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!hand.IsFreeToInteract())
            {
                return false;
            }
            
            if (Interact.IsBlocked(Network.Identifier.GetIdentityId(__instance.gameObject)))
            {
                Interact.ShowUseDenyMessage();
                return false;
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Pickupable), nameof(global::Pickupable.OnHandClick))]
        private static bool OnHandClick(global::Pickupable __instance, GUIHand hand)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!hand.IsFreeToInteract() || !__instance.AllowedToPickUp())
            {
                return false;
            }

            if (Interact.IsBlocked(Network.Identifier.GetIdentityId(__instance.gameObject)))
            {
                Interact.ShowUseDenyMessage();
                return false;
            }

            return true;
        }
    }
}
