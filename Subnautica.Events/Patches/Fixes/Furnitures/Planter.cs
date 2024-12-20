namespace Subnautica.Events.Patches.Fixes.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::Planter), nameof(global::Planter.AddItem), new Type[] { typeof(InventoryItem) })]
    public class PlanterAddItem
    {
        private static bool Prefix(InventoryItem item)
        {
            if (Network.IsMultiplayerActive && EventBlocker.IsEventBlocked(item.item.GetTechType()))
            {
                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(global::Planter), nameof(global::Planter.AddItem), new Type[] { typeof(Plantable), typeof(int) })]
    public class PlanterAddItemTwo
    {
        private static bool Prefix(Plantable plantable)
        {
            if (Network.IsMultiplayerActive && EventBlocker.IsEventBlocked(plantable.pickupable.GetTechType()))
            {
                return false;
            }

            return true;
        }
    }
}
