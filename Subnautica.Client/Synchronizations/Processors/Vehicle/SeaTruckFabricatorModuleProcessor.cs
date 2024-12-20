namespace Subnautica.Client.Synchronizations.Processors.Vehicle
{
    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;

    using ServerModel = Subnautica.Network.Models.Server;

    public class SeaTruckFabricatorModuleProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.SeaTruckFabricatorModuleArgs>();
            if (string.IsNullOrEmpty(packet.UniqueId))
            {
                return false;
            }

            if (packet.IsSignProcess)
            {
                if (!packet.IsSignSelect)
                {
                    using (EventBlocker.Create(TechType.Sign))
                    {
                        var signInput = Network.Identifier.GetComponentByGameObject<global::uGUI_SignInput>(ZeroGame.GetSeaTruckColoredLabelUniqueId(packet.UniqueId));
                        if (signInput)
                        {
                            signInput.text       = packet.SignText;
                            signInput.colorIndex = packet.SignColorIndex;
                        }
                    }
                }
            }
            else
            {
                if (packet.IsAdded)
                {
                    Network.Storage.AddItemToStorage(packet.UniqueId, packet.GetPacketOwnerId(), packet.WorldPickupItem);
                }
                else
                {
                    Network.Storage.AddItemToInventory(packet.GetPacketOwnerId(), packet.WorldPickupItem);
                }
            }

            return true;
        }

        /**
         *
         * Depolamaya eşya eklendiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnStorageItemAdding(StorageItemAddingEventArgs ev)
        {
            if (ev.TechType == TechType.SeaTruckFabricatorModule)
            {
                ev.IsAllowed = false;

                SeaTruckFabricatorModuleProcessor.SendPacketToServer(ev.UniqueId, pickupItem: WorldPickupItem.Create(ev.Item, PickupSourceType.PlayerInventory), isAdded: true);
            }
        }

        /**
         *
         * Depolama'dan eşya kaldırıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnStorageItemRemoving(StorageItemRemovingEventArgs ev)
        {
            if (ev.TechType == TechType.SeaTruckFabricatorModule)
            {
                ev.IsAllowed = false;

                SeaTruckFabricatorModuleProcessor.SendPacketToServer(ev.UniqueId, pickupItem: WorldPickupItem.Create(ev.Item, PickupSourceType.StorageContainer));
            }
        }

        /**
         *
         * Tabela seçildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSignSelect(SignSelectEventArgs ev)
        {
            if (ev.TechType == TechType.SeaTruckFabricatorModule)
            {
                if (Interact.IsBlocked(ev.UniqueId))
                {
                    ev.IsAllowed = false;
                }
                else
                {
                    SeaTruckFabricatorModuleProcessor.SendPacketToServer(ev.UniqueId, isSignProcess: true, isSignSelect: true);
                }
            }
        }

        /**
         *
         * Tabela da veri değişimi olduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSignDataChanged(SignDataChangedEventArgs ev)
        {
            if (ev.TechType == TechType.SeaTruckFabricatorModule)
            {
                SeaTruckFabricatorModuleProcessor.SendPacketToServer(ev.UniqueId, isSignProcess: true, signText: ev.Text, signColorIndex: ev.ColorIndex);
            }
        }
        
        /**
         *
         * Sunucuya paketi gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId, bool isSignProcess = false, bool isSignSelect = false, string signText = null, int signColorIndex = -1, WorldPickupItem pickupItem = null, bool isAdded = false)
        {
            ServerModel.SeaTruckFabricatorModuleArgs request = new ServerModel.SeaTruckFabricatorModuleArgs()
            {
                UniqueId        = uniqueId,
                IsSignProcess   = isSignProcess,
                IsSignSelect    = isSignSelect,
                IsAdded         = isAdded,
                SignText        = signText,
                SignColorIndex  = signColorIndex,
                WorldPickupItem = pickupItem,
            };

            NetworkClient.SendPacket(request);
        }
    }
}