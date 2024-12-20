namespace Subnautica.Server.Processors.Vehicle
{
    using Server.Core;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Server.Abstracts.Processors;

    using System.Linq;

    using MetadataModel    = Subnautica.Network.Models.Metadata;
    using ServerModel      = Subnautica.Network.Models.Server;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class HealthProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.VehicleHealthArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            var entity = Server.Instance.Storages.World.GetVehicle(packet.UniqueId, true);
            if (entity == null || !entity.TechType.IsVehicle())
            {
                return false;
            }

            var liveMixin = entity.GetLiveMixin();
            if (liveMixin != null)
            {
                if (liveMixin.TakeDamage(liveMixin.CalculateDamage(packet.Damage, packet.DamageType)))
                {
                    Log.Info("VEHICLE Dead: " + liveMixin.IsDead + ", Type: " + entity.TechType + ", NewHealth: " + liveMixin.Health + ", CalculateDamage: " + liveMixin.CalculateDamage(packet.Damage, packet.DamageType) + ", Damage: " + packet.Damage + ", DamageType: " + packet.DamageType);
                    
                    packet.NewHealth = liveMixin.Health;

                    profile.SendPacketToAllClient(packet);

                    if (liveMixin.IsDead)
                    {
                        this.KillVehicle(entity);
                    }
                }
            }

            return true;
        }

        /**
         *
         * Aracı yok eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void KillVehicle(WorldDynamicEntity entity)
        {
            foreach (var connection in Server.Instance.Storages.World.Storage.SeaTruckConnections.ToSet())
            {
                if (connection.Key == entity.UniqueId || connection.Value == entity.UniqueId)
                {
                    Server.Instance.Storages.World.Storage.SeaTruckConnections.Remove(connection.Key);
                }

                if (connection.Value == entity.UniqueId)
                {
                    var ent = Server.Instance.Storages.World.GetDynamicEntity(connection.Key);
                    if (ent != null)
                    {
                        ent.SetParent(null);
                    }
                }
            }

            foreach (var construction in Server.Instance.Storages.Construction.Storage.Constructions.Where(q => q.Value.TechType == TechType.BaseMoonpool || q.Value.TechType == TechType.BaseMoonpoolExpansion))
            {
                var component = construction.Value.EnsureComponent<MetadataModel.BaseMoonpool>();
                if (component.IsDocked && component.Vehicle.UniqueId == entity.UniqueId)
                {
                    component.Undock(out var _);
                }
            }

            foreach (var item in Server.Instance.Storages.World.Storage.DynamicEntities.Where(q => q.TechType == TechType.SeaTruckDockingModule))
            {
                var component = item.Component.GetComponent<WorldEntityModel.SeaTruckDockingModule>();
                if (component.IsDocked() && component.Vehicle.UniqueId == entity.UniqueId)
                {
                    component.Undock(out var _);
                }
            }

            foreach (var player in Server.Instance.GetPlayers())
            {
                player.RemoveNotification(entity.UniqueId);
                
                if (player.VehicleId == entity.UniqueId)
                {
                    player.SetVehicle(null);
                }
            }

            if (entity.TechType == TechType.SeaTruckFabricatorModule)
            {
                var component = entity.Component.GetComponent<WorldEntityModel.SeaTruckFabricatorModule>();
                if (component != null)
                {
                    Server.Instance.Storages.Construction.ConstructionRemove(component.FabricatorUniqueId);
                }
            }

            Server.Instance.Storages.World.RemoveDynamicEntity(entity.UniqueId);
        }
    }
}