namespace Subnautica.Server.Processors.Items
{
    using Subnautica.Network.Models.Server;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;
    using ItemModel        = Subnautica.Network.Models.Items;
    using Metadata         = Subnautica.Network.Models.Metadata;

    public class DeployableStorageProcessor : PlayerItemProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(AuthorizationProfile profile, PlayerItemActionArgs packet)
        {
            var component = packet.Item.GetComponent<ItemModel.DeployableStorage>();
            if (component == null)
            {
                return false;
            }

            var entity = Server.Instance.Storages.World.GetDynamicEntity(packet.Item.UniqueId);
            if (entity == null)
            {
                return false;
            }

            if (component.IsSignProcess)
            {
                if (component.IsSignSelect)
                {
                    if (Server.Instance.Logices.Interact.IsBlocked(component.UniqueId))
                    {
                        return false;
                    }

                    Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, component.UniqueId, true);
                }
                else
                {
                    if (component.TechType == TechType.SmallStorage)
                    {
                        var smallStorage = entity.Component.GetComponent<WorldEntityModel.SmallStorage>();
                        if (smallStorage.StorageContainer.Sign == null)
                        {
                            smallStorage.StorageContainer.Sign = new Metadata.Sign();
                        }

                        smallStorage.StorageContainer.Sign.Text       = component.SignText;
                        smallStorage.StorageContainer.Sign.ColorIndex = component.SignColorIndex;

                        profile.SendPacketToOtherClients(packet);
                    }
                }
            }
            else
            {
                if (component.TechType == TechType.SmallStorage)
                {
                    if (Server.Instance.Logices.Interact.IsBlocked(component.UniqueId, profile.UniqueId))
                    {
                        return false;
                    }

                    var smallStorage = entity.Component.GetComponent<WorldEntityModel.SmallStorage>();
                    if (smallStorage?.StorageContainer == null)
                    {
                        return false;
                    }

                    if (component.IsAdded)
                    {
                        if (Server.Instance.Logices.Storage.TryPickupItem(component.WorldPickupItem, smallStorage.StorageContainer, profile.InventoryItems))
                        {
                            profile.SendPacketToAllClient(packet);
                        }
                    }
                    else
                    {
                        if (Server.Instance.Logices.Storage.TryPickupItem(component.WorldPickupItem, profile.InventoryItems, smallStorage.StorageContainer))
                        {
                            profile.SendPacketToAllClient(packet);
                        }
                    }
                }
                else
                {
                    if (component.IsAdded)
                    {
                        if (Server.Instance.Logices.Storage.TryPickupItem(component.WorldPickupItem, Server.Instance.Storages.World.Storage.QuantumLocker, profile.InventoryItems))
                        {
                            profile.SendPacketToAllClient(packet);
                        }
                    }
                    else
                    {
                        if (Server.Instance.Logices.Storage.TryPickupItem(component.WorldPickupItem, profile.InventoryItems, Server.Instance.Storages.World.Storage.QuantumLocker))
                        {
                            profile.SendPacketToAllClient(packet);
                        }
                    }
                }
            }

            return true;
        }
    }
}
