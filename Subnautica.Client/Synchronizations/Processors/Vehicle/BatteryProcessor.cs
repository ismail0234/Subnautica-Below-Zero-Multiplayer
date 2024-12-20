namespace Subnautica.Client.Synchronizations.Processors.Vehicle
{
    using System.Collections.Generic;

    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;

    using ServerModel = Subnautica.Network.Models.Server;

    public class BatteryProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.VehicleBatteryArgs>();
            if (string.IsNullOrEmpty(packet.UniqueId))
            {
                return false;
            }

            if (packet.IsOpening)
            {
                var gameObject = Network.Identifier.GetComponentByGameObject<global::GenericHandTarget>(ZeroGame.GetVehicleBatteryLabelUniqueId(packet.BatterySlotId));
                if (gameObject)
                {
                    using (EventBlocker.Create(TechType.PictureSamHand))
                    {
                        gameObject.OnHandClick(global::Player.main.guiHand);
                    }
                }
            }
            else
            {
                if (packet.IsAdding)
                {
                    Vehicle.ApplyPowerCells(packet.UniqueId, new List<PowerCell>() { 
                        new PowerCell() 
                        {
                            UniqueId = packet.BatterySlotId,
                            TechType = packet.BatteryType,
                            Charge   = packet.Charge,
                        }
                    });
                }
                else
                {
                    var action = new ItemQueueAction();
                    action.OnProcessCompleted = this.OnProcessCompleted;
                    action.RegisterProperty("Packet", packet);

                    Entity.ProcessToQueue(action);
                }
            }

            return true;
        }

        /**
         *
         * işlem tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void OnProcessCompleted(ItemQueueProcess item)
        {
            InventoryItem storedItem = null;

            var packet = item.Action.GetProperty<ServerModel.VehicleBatteryArgs>("Packet");
            if (packet != null)
            {
                var vehicle = Network.Identifier.GetGameObject(packet.UniqueId);
                if (vehicle)
                {
                    var exosuit = vehicle.GetComponent<global::Exosuit>();
                    if (exosuit)
                    {
                        foreach (var energyMixin in exosuit.energyInterface.sources)
                        {
                            if (Network.Identifier.GetIdentityId(energyMixin.storageRoot.gameObject, false) == packet.BatterySlotId)
                            {
                                storedItem = energyMixin.batterySlot.storedItem;

                                energyMixin.batterySlot.RemoveItem();
                                
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (var batterySource in vehicle.GetComponentsInChildren<global::BatterySource>())
                        {
                            if (Network.Identifier.GetIdentityId(batterySource.storageRoot.gameObject, false) == packet.BatterySlotId)
                            {
                                storedItem = batterySource.batterySlot.storedItem;

                                batterySource.batterySlot.RemoveItem();
                                break;
                            }
                        }
                    }
                }
            }

            if (storedItem?.item != null)
            {
                World.DestroyGameObject(storedItem.item.gameObject);
            }
        }

        /**
         *
         * Pil yerleştirilme alanına tıklanınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEnergyMixinClicking(EnergyMixinClickingEventArgs ev)
        {
            if (ev.TechType == TechType.Exosuit || ev.TechType == TechType.SeaTruck)
            {
                ev.IsAllowed = false;
                
                if (!Interact.IsBlocked(ev.BatterySlotId))
                {
                    BatteryProcessor.SendPacketToServer(ev.UniqueId, batterySlotId: ev.BatterySlotId, isOpening: true);
                }
            }
        }

        /**
         *
         * Pil yerleştirildiğinde/çıkarıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEnergyMixinSelecting(EnergyMixinSelectingEventArgs ev)
        {
            if (ev.TechType == TechType.Exosuit || ev.TechType == TechType.SeaTruck)
            {
                if (Interact.IsBlockedByMe(ev.BatterySlotId))
                {
                    BatteryProcessor.SendPacketToServer(ev.UniqueId, batterySlotId: ev.BatterySlotId, batteryType: ev.BatteryType, isAdding: ev.IsAdding || ev.IsChanging, charge: ev.Item == null ? 0f : ev.Item.GetComponent<IBattery>().charge);
                }
            }
        }

        /**
         *
         * Sunucuya veri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId, string batterySlotId, TechType batteryType = TechType.None, bool isOpening = false, bool isAdding = false, float charge = -1f)
        {
            ServerModel.VehicleBatteryArgs request = new ServerModel.VehicleBatteryArgs()
            {
                UniqueId      = uniqueId,
                BatterySlotId = batterySlotId,
                BatteryType   = batteryType,
                IsOpening     = isOpening,
                IsAdding      = isAdding,
                Charge        = charge,
            };

            NetworkClient.SendPacket(request);
        }
    }
}