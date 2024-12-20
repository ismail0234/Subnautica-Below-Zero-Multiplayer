namespace Subnautica.Events.Patches.Fixes.Player
{
    using System.Collections.Generic;

    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::Inventory), nameof(global::Inventory.LoseItems))]
    public class LoseItems
    {
        private static bool Prefix(global::Inventory __instance, ref bool __result)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            var inventoryItemList = new List<InventoryItem>();

            foreach (InventoryItem inventoryItem in __instance.container)
            {
                if (inventoryItem.item.destroyOnDeath && inventoryItem.item.GetTechType() != TechType.Beacon)
                {
                    inventoryItemList.Add(inventoryItem);
                }
            }

            foreach (InventoryItem inventoryItem in (IItemsContainer)__instance.equipment)
            {
                if (inventoryItem.item.destroyOnDeath)
                {
                    inventoryItemList.Add(inventoryItem);
                }
            }

            if (inventoryItemList.Count > 0 )
            {
                foreach (InventoryItem inventoryItem in __instance.container)
                {
                    if (inventoryItem.item.GetTechType() == TechType.Beacon)
                    {
                        inventoryItemList.Add(inventoryItem);
                        break;
                    }
                }
            }

            for (int index = 0; index < inventoryItemList.Count; ++index)
            {
                if (inventoryItemList[index].item.GetTechType() == TechType.Beacon && inventoryItemList[index].item.TryGetComponent<global::Beacon>(out var beacon))
                {
                    string label = global::Language.main.Get("DroppedBeacon");

                    using (EventBlocker.Create(TechType.Beacon))
                    {
                        beacon.beaconLabel.SetLabel(label);
                        beacon.label = label;
                    }
                }

                __instance.InternalDropItem(inventoryItemList[index].item, false, true);
            }

            __result = inventoryItemList.Count > 0;
            return false;
        }
    }
}
