namespace Subnautica.Events.Patches.Events.Inventory
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::Equipment), nameof(global::Equipment.NotifyUnequip))]
    public class EquipmentUnequiped
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Postfix(global::Equipment __instance, string slot, InventoryItem item)
        {
            if (Network.IsMultiplayerActive)
            {
                if (EventBlocker.IsEventBlocked(item.item.GetTechType()))
                {
                    return;
                }

                try
                {
                    if (slot.Contains("HoverbikeModule") || slot.Contains("Exosuit") || slot.Contains("SeaTruckModule"))
                    {
                        var lwe = __instance.tr.GetComponentInParent<LargeWorldEntity>();
                        if (lwe)
                        {
                            UpgradeConsoleModuleRemovedEventArgs args = new UpgradeConsoleModuleRemovedEventArgs(Network.Identifier.GetIdentityId(lwe.gameObject, false), slot, Network.Identifier.GetIdentityId(item.item.gameObject), item.item.GetTechType());

                            Handlers.Vehicle.OnUpgradeConsoleModuleRemoved(args);
                        }
                    }                    
                    else if (slot.Contains("NuclearReactor"))
                    {
                        var reactor = __instance.tr.GetComponentInParent<BaseNuclearReactor>();
                        if (reactor != null)
                        {
                            NuclearReactorItemRemovedEventArgs args = new NuclearReactorItemRemovedEventArgs(Network.Identifier.GetIdentityId(reactor.GetModel().gameObject, false), slot, Network.Identifier.GetIdentityId(item.item.gameObject), item.item);

                            Handlers.Storage.OnNuclearReactorItemRemoved(args);
                        }
                    }
                    else if (TechGroup.PowerCellChargerSlots.Contains(slot) || TechGroup.BatteryChargerSlots.Contains(slot))
                    {
                        var charger = __instance.tr.GetComponentInParent<Constructable>();
                        if (charger != null)
                        {
                            ChargerItemRemovedEventArgs args = new ChargerItemRemovedEventArgs(Network.Identifier.GetIdentityId(charger.gameObject, false), slot, charger.techType, Network.Identifier.GetIdentityId(item.item.gameObject), item.item);

                            Handlers.Storage.OnChargerItemRemoved(args);
                        }
                    }
                    else if (global::Inventory.main.equipment.equipment.ContainsKey(slot))
                    {
                        Handlers.Inventory.OnEquipmentUnequiped();
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"EquipmentUnequiped.Postfix: slot: {slot}, {e}\n{e.StackTrace}");
                }
            }
        }
    }
}