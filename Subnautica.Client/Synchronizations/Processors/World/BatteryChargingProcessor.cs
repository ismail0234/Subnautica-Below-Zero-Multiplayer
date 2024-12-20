namespace Subnautica.Client.Synchronizations.Processors.World
{
    using Subnautica.Network.Models.Core;
    using Subnautica.Client.Abstracts;
    using Subnautica.API.Features;

    using ServerModel = Subnautica.Network.Models.Server;

    public class BatteryChargingProcessor : NormalProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            if (!World.IsLoaded)
            {
                return false;
            }

            var packet = networkPacket.GetPacket<ServerModel.BatteryChargerTransmissionArgs>();
            if(packet == null)
            {
                return false;
            }

            foreach (var charger in packet.Chargers)
            {
                if (Multiplayer.Constructing.Builder.TryGetBuildingValue(charger.ConstructionId, out string uniqueId)) 
                {
                    var gameObject = Network.Identifier.GetComponentByGameObject<global::Charger>(uniqueId);
                    if (gameObject == null)
                    {
                        continue;
                    }

                    gameObject.ToggleUIPowered(charger.IsPowered);
                    gameObject.ToggleChargeSound(charger.IsCharging);

                    if (gameObject.TryGetComponent<global::Constructable>(out var constructable))
                    {
                        for (int i = 0; i < charger.Batteries.Length; i++)
                        {
                            string slotId = TechGroup.GetBatterySlotId(constructable.techType, i + 1);

                            if (gameObject.batteries.TryGetValue(slotId, out var battery) && battery != null && gameObject.slots.TryGetValue(slotId, out var definition))
                            {
                                if (battery.charge != charger.Batteries[i])
                                {
                                    battery.charge = charger.Batteries[i];

                                    gameObject.UpdateVisuals(definition, battery.charge / battery.capacity, gameObject.equipment.GetItemInSlot(slotId).item.GetTechType());
                                }
                            }

                        }
                    }
                }
            }

            packet.Chargers.Clear();
            return true;
        }
    }
}
