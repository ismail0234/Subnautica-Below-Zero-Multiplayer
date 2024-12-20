namespace Subnautica.Server.Processors.Metadata
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.Network.Models.Metadata;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using MetadataModel = Subnautica.Network.Models.Metadata;
    using ServerModel   = Subnautica.Network.Models.Server;

    public class BaseMapRoomProcessor : MetadataProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(AuthorizationProfile profile, MetadataComponentArgs packet, ConstructionItem construction)
        {
            if (packet.SecretTechType == TechType.Fabricator)
            {
                packet.TechType = TechType.Fabricator;
                return MetadataProcessor.ExecuteProcessor(profile, packet, construction, TechType.Fabricator);
            }

            var component = packet.Component.GetComponent<MetadataModel.BaseMapRoom>();
            if (component == null)
            {
                return false;
            }

            var baseMapRoom = construction.EnsureComponent<MetadataModel.BaseMapRoom>();
            if (baseMapRoom == null)
            {
                return false;
            }

            if (baseMapRoom.StorageContainer == null)
            {
                baseMapRoom.StorageContainer = MetadataModel.StorageContainer.Create(2, 2);
            }

            if (component.ProcessType == 1)
            {
                if (Server.Instance.Logices.Storage.TryPickupItem(component.PickupItem, baseMapRoom.StorageContainer, profile.InventoryItems))
                {
                    Server.Instance.Logices.BaseMapRoom.UpdateScanRange(construction, baseMapRoom);

                    profile.SendPacketToAllClient(packet);
                }
            }
            else if (component.ProcessType == 2)
            {
                if (Server.Instance.Logices.Storage.TryPickupItem(component.PickupItem, profile.InventoryItems, baseMapRoom.StorageContainer))
                {
                    Server.Instance.Logices.BaseMapRoom.UpdateScanRange(construction, baseMapRoom);

                    profile.SendPacketToAllClient(packet);
                }
            }
            else if (component.ProcessType == 3)
            {
                if (baseMapRoom.StartScan(component.ScanTechType))
                {
                    profile.SendPacketToAllClient(packet);
                }
            }
            else if (component.ProcessType == 4)
            {
                if (baseMapRoom.StopScan())
                {
                    profile.SendPacketToAllClient(packet);
                }
            }
            else if (component.ProcessType == 5)
            {
                var entity = Server.Instance.Storages.World.GetDynamicEntity(component.LeftDock.VehicleId);
                if (entity == null || entity.TechType != TechType.MapRoomCamera)
                {
                    return false;
                }

                if (baseMapRoom.Dock(entity, component.LeftDock.IsDocked, component.LeftDock.Position, component.LeftDock.Rotation, Server.Instance.Logices.World.GetServerTime()))
                {
                    Server.Instance.Storages.World.RemoveDynamicEntity(entity.UniqueId);

                    component.LeftDock.Vehicle = entity;

                    if (entity.IsUsingByPlayer)
                    {
                        var player = Server.Instance.GetPlayer(entity.OwnershipId);
                        if (player != null)
                        {
                            player.SetVehicle(null);
                            player.SetUsingRoomId(null);

                            Server.Instance.Logices.Interact.RemoveBlockByPlayerId(player.UniqueId);
                        }

                        entity.IsUsingByPlayer = false;
                        entity.SetOwnership(null);
                    }

                    profile.SendPacketToAllClient(packet);
                }
            }
            else if (component.ProcessType == 7)
            {
                if (Server.Instance.Logices.Interact.IsBlocked(packet.UniqueId, profile.UniqueId) || profile.VehicleId.IsNull() || profile.UsingRoomId.IsNull())
                {
                    return false;
                }

                var vehicleId = this.GetNextVehicleId(profile.VehicleId, component.IsNextCamera);
                if (vehicleId.IsNull())
                {
                    return false;
                }

                var oldVehicle = Server.Instance.Storages.World.GetDynamicEntity(profile.VehicleId);
                var newVehicle = Server.Instance.Storages.World.GetDynamicEntity(vehicleId);
                if (oldVehicle == null || newVehicle == null)
                {
                    return false;
                }

                var mapRoom = Server.Instance.Storages.Construction.GetConstruction(profile.UsingRoomId);
                if (mapRoom == null || mapRoom.TechType != TechType.BaseMapRoom)
                {
                    return false;
                }

                ServerModel.VehicleExitArgs exitRequest = new ServerModel.VehicleExitArgs()
                {
                    UniqueId = oldVehicle.UniqueId,
                    TechType = oldVehicle.TechType,
                };

                ServerModel.VehicleEnterArgs enterRequest = new ServerModel.VehicleEnterArgs()
                {
                    CustomId = mapRoom.UniqueId,
                    UniqueId = newVehicle.UniqueId,
                    TechType = newVehicle.TechType,
                };

                NormalProcessor.ExecuteProcessor(profile, exitRequest);
                NormalProcessor.ExecuteProcessor(profile, enterRequest);
            }
            else if (component.ProcessType == 8)
            {
                if (component.PickupItem == null || Server.Instance.Logices.Interact.IsBlocked(packet.UniqueId, profile.UniqueId))
                {
                    return false;
                }

                var vehicle = this.FindDockedVehicle(baseMapRoom, component.PickupItem.Item.ItemId);
                if (vehicle == null)
                {
                    return false;
                }

                component.PickupItem.SetSource(PickupSourceType.NoSource);

                if (vehicle.Undock(Server.Instance.Logices.World.GetServerTime(), out var _))
                {
                    if (Server.Instance.Logices.Storage.TryPickupItem(component.PickupItem, profile.InventoryItems))
                    {
                        profile.SendPacketToAllClient(packet);
                    }
                }
            }

            return true;
        }

        /**
         *
         * Kenetlenmiş kamera arar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private VehicleDockingBayItem FindDockedVehicle(MetadataModel.BaseMapRoom baseMapRoom, string vehicleId)
        {
            if (baseMapRoom.LeftDock.IsDocked && baseMapRoom.LeftDock.Vehicle.UniqueId == vehicleId)
            {
                return baseMapRoom.LeftDock;
            }

            if (baseMapRoom.RightDock.IsDocked && baseMapRoom.RightDock.Vehicle.UniqueId == vehicleId)
            {
                return baseMapRoom.RightDock;
            }

            return null;
        }

        /**
         *
         * Sonraki kamera id'sini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private string GetNextVehicleId(string currentVehicleId, bool isNext)
        {
            var vehicleId = string.Empty;
            var cameras   = new List<WorldDynamicEntity>();

            foreach (var item in Server.Instance.Storages.World.Storage.DynamicEntities.Where(q => q.TechType == TechType.MapRoomCamera))
            {
                cameras.Add(item);
            }

            foreach (var item in Server.Instance.Storages.Construction.Storage.Constructions.Where(q => q.Value.TechType == TechType.BaseMapRoom))
            {
                var component = item.Value.Component.GetComponent<MetadataModel.BaseMapRoom>();
                if (component != null)
                {
                    if (component.LeftDock.IsDocked)
                    {
                        cameras.Add(component.LeftDock.Vehicle);
                    }

                    if (component.RightDock.IsDocked)
                    {
                        cameras.Add(component.RightDock.Vehicle);
                    }
                }
            }

            var check = 0;
            var count = cameras.Count;
            int index = cameras.FindIndex(q => q.IsUsingByPlayer && q.UniqueId == currentVehicleId);
            if (index == -1)
            {
                return null;
            }

            while (check != count)
            {
                check++;

                if (isNext)
                {
                    if (index + 1 >= count)
                    {
                        index = -1;
                    }

                    index++;
                }
                else
                {
                    if (index <= 0)
                    {
                        index = count;
                    }

                    index--;
                }

                var camera = cameras.ElementAt(index);
                if (!camera.IsUsingByPlayer && camera.UniqueId != currentVehicleId)
                {
                    return camera.UniqueId;
                }
            }

            return null;
        }
    }
}