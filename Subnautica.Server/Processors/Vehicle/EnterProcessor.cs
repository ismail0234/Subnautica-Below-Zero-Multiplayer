namespace Subnautica.Server.Processors.Vehicle
{
    using System.Collections.Generic;
    using System.Linq;

    using Server.Core;

    using Subnautica.API.Extensions;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Structures;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Extensions;

    using MetadataModel = Subnautica.Network.Models.Metadata;
    using ServerModel   = Subnautica.Network.Models.Server;

    public class EnterProcessor : NormalProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnExecute(AuthorizationProfile profile, NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.VehicleEnterArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (profile.VehicleId.IsNotNull())
            {
                API.Features.Log.Info("VEHICLE ID NOT NULL => " + profile.VehicleId);
                return false;
            }

            if (packet.TechType == TechType.SpyPenguin && !this.TryActivateSpyPenguinFromDistance(packet, profile.Position))
            {
                return false;
            }

            if (packet.TechType == TechType.MapRoomCamera && !this.TryActivateMapRoomCamera(packet, profile.Position))
            {
                return false;
            }

            if (packet.UniqueId.IsNull())
            {
                return false;
            }

            if (Server.Instance.Logices.Interact.IsBlocked(packet.UniqueId))
            {
                return false;
            }

            var entity = this.GetVehicle(packet.UniqueId);
            if (entity == null)
            {
                return false;
            }
            
            if (packet.TechType == TechType.MapRoomCamera)
            {
                packet.Vehicle = entity;
            }

            entity.IsUsingByPlayer = true;
            entity.SetParent(null);

            var liveMixin = entity.GetLiveMixin();
            if (liveMixin != null)
            {
                packet.Health = liveMixin.Health;
            }

            Server.Instance.Logices.VehicleEnergyTransmission.VehicleEnergyUpdateQueue(entity, true);
            Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, packet.UniqueId, true);
            Server.Instance.Logices.EntityWatcher.ChangeEntityOwnership(entity, profile.UniqueId, true);

            packet.Position = entity.Position;
            packet.Rotation = entity.Rotation;

            profile.SetVehicle(entity.UniqueId);
            profile.SetUsingRoomId(packet.CustomId);
            profile.SendPacketToAllClient(packet);
            return true;
        }

        /**
         *
         * Aracı döner. harita odası kamerasını bulur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private WorldDynamicEntity GetVehicle(string vehicleId)
        {
            var entity = Server.Instance.Storages.World.GetDynamicEntity(vehicleId);
            if (entity != null)
            {
                return entity;
            }

            var currentTime = Server.Instance.Logices.World.GetServerTime();

            foreach (var item in Server.Instance.Storages.Construction.Storage.Constructions.Where(q => q.Value.TechType == TechType.BaseMapRoom))
            {
                var component = item.Value.Component.GetComponent<MetadataModel.BaseMapRoom>();
                if (component == null)
                {
                    continue;
                }

                if (component.LeftDock.IsDocked && component.LeftDock.Vehicle.UniqueId == vehicleId)
                {
                    if (component.LeftDock.Undock(currentTime, out var vehicle))
                    {
                        vehicle.RenewId();

                        Server.Instance.Storages.World.AddWorldDynamicEntity(vehicle);
                        return vehicle;
                    }

                    return null;
                }

                if (component.RightDock.IsDocked && component.RightDock.Vehicle.UniqueId == vehicleId)
                {
                    if (component.RightDock.Undock(currentTime, out var vehicle))
                    {
                        vehicle.RenewId();

                        Server.Instance.Storages.World.AddWorldDynamicEntity(vehicle);
                        return vehicle;
                    }

                    return null;
                }
            }

            return null;
        }

        /**
         *
         * Yakındaki casus penguen aracını bulur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool TryActivateSpyPenguinFromDistance(ServerModel.VehicleEnterArgs packet, ZeroVector3 playerPosition)
        {
            var spyPenguins = new Dictionary<string, float>();

            foreach (var item in Server.Instance.Storages.World.Storage.DynamicEntities.Where(q => q.TechType == TechType.SpyPenguin && !q.IsUsingByPlayer))
            {
                spyPenguins.Add(item.UniqueId, item.Position.Distance(playerPosition));
            }

            if (spyPenguins.Count <= 0)
            {
                return false;
            }

            packet.UniqueId = spyPenguins.OrderBy(key => key.Value).ElementAt(0).Key;
            return true;
        }

        /**
         *
         * Yakındaki harita odası kamerasını bulur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool TryActivateMapRoomCamera(ServerModel.VehicleEnterArgs packet, ZeroVector3 playerPosition)
        {
            if (packet.CustomId.IsNotNull())
            {
                return true;
            }

            foreach (var player in Server.Instance.GetPlayers())
            {
                if (player.UsingRoomId == packet.UniqueId)
                {
                    return false;
                }
            }

            var mapRoomCameras = new Dictionary<string, float>();

            foreach (var item in Server.Instance.Storages.World.Storage.DynamicEntities.Where(q => q.TechType == TechType.MapRoomCamera && !q.IsUsingByPlayer))
            {
                mapRoomCameras.Add(item.UniqueId, item.Position.Distance(playerPosition));
            }

            foreach (var item in Server.Instance.Storages.Construction.Storage.Constructions.Where(q => q.Value.TechType == TechType.BaseMapRoom))
            {
                var component = item.Value.Component.GetComponent<MetadataModel.BaseMapRoom>();
                if (component != null)
                {
                    if (component.LeftDock.IsDocked && !component.LeftDock.Vehicle.IsUsingByPlayer)
                    {
                        mapRoomCameras.Add(component.LeftDock.Vehicle.UniqueId, component.LeftDock.Vehicle.Position.Distance(playerPosition));
                    }

                    if (component.RightDock.IsDocked && !component.RightDock.Vehicle.IsUsingByPlayer)
                    {
                        mapRoomCameras.Add(component.RightDock.Vehicle.UniqueId, component.RightDock.Vehicle.Position.Distance(playerPosition));
                    }
                }
            }

            if (mapRoomCameras.Count <= 0)
            {
                return false;
            }

            packet.CustomId = packet.UniqueId;
            packet.UniqueId = mapRoomCameras.OrderBy(key => key.Value).ElementAt(0).Key;
            return true;
        }
    }
}
