namespace Subnautica.Events.Patches.Events.Inventory
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::Equipment), nameof(global::Equipment.NotifyEquip))]
    public class EquipmentEquiped
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
                            UpgradeConsoleModuleAddedEventArgs args = new UpgradeConsoleModuleAddedEventArgs(Network.Identifier.GetIdentityId(lwe.gameObject, false), slot, Network.Identifier.GetIdentityId(item.item.gameObject), item.item.GetTechType());

                            Handlers.Vehicle.OnUpgradeConsoleModuleAdded(args);
                        }
                    }
                    else if (slot.Contains("NuclearReactor"))
                    {
                        var reactor = __instance.tr.GetComponentInParent<BaseNuclearReactor>();
                        if (reactor != null)
                        {
                            NuclearReactorItemAddedEventArgs args = new NuclearReactorItemAddedEventArgs(Network.Identifier.GetIdentityId(reactor.GetModel().gameObject, false), slot, Network.Identifier.GetIdentityId(item.item.gameObject), item.item);

                            Handlers.Storage.OnNuclearReactorItemAdded(args);
                        }
                    }
                    else if (TechGroup.PowerCellChargerSlots.Contains(slot) || TechGroup.BatteryChargerSlots.Contains(slot))
                    {
                        var charger = __instance.tr.GetComponentInParent<Constructable>();
                        if (charger != null)
                        {
                            ChargerItemAddedEventArgs args = new ChargerItemAddedEventArgs(Network.Identifier.GetIdentityId(charger.gameObject, false), slot, charger.techType, Network.Identifier.GetIdentityId(item.item.gameObject), item.item);

                            Handlers.Storage.OnChargerItemAdded(args);
                        }
                    }
                    else if (global::Inventory.main.equipment.equipment.ContainsKey(slot))
                    {
                        Handlers.Inventory.OnEquipmentEquiped();
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"EquipmentEquiped.Postfix: slot: {slot}, {e}\n{e.StackTrace}");
                }
            }
        }
    }
}