namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;
    
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;
    using Subnautica.Events.Patches.Fixes.Interact;

    [HarmonyPatch(typeof(global::GhostCrafter), nameof(global::GhostCrafter.OnHandClick))]
    public static class FabricatorOpening
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::GhostCrafter __instance, global::GUIHand hand)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!__instance.enabled || __instance.logic == null || __instance.logic.inProgress)
            {
                return false;
            }

            var uniqueId = GhostCrafter.GetUniqueId(__instance.gameObject);
            var techType = GhostCrafter.GetTechType(__instance.gameObject);
            if (string.IsNullOrEmpty(uniqueId))
            {
                return false;
            }

            if (EventBlocker.IsEventBlocked(techType))
            {
                return true;
            }

            try
            {
                if (__instance.HasCraftedItem())
                {
                    CrafterItemPickupEventArgs args = new CrafterItemPickupEventArgs(uniqueId, __instance, __instance.logic.numCrafted, techType, __instance.logic.craftingTechType);

                    Handlers.Furnitures.OnCrafterItemPickup(args);

                    return args.IsAllowed;
                }
                else
                {
                    if (!__instance.HasEnoughPower() || !__instance.isValidHandTarget)
                    {
                        return false;
                    }

                    CrafterOpeningEventArgs args = new CrafterOpeningEventArgs(uniqueId, techType);

                    Handlers.Furnitures.OnCrafterOpening(args);

                    return args.IsAllowed;
                }
            }
            catch (Exception e)
            {
                Log.Error($"Furnitures.FabricatorOpening: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}
