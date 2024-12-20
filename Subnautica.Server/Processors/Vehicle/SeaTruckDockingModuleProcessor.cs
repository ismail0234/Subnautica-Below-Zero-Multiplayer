namespace Subnautica.Server.Processors.Vehicle
{
    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Extensions;

    using ServerModel      = Subnautica.Network.Models.Server;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class SeaTruckDockingModuleProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.SeaTruckDockingModuleArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            var dockingModule = Server.Instance.Storages.World.GetDynamicEntity(packet.UniqueId);
            if (dockingModule == null || dockingModule.TechType != TechType.SeaTruckDockingModule)
            {
                return false;
            }

            var component = dockingModule.Component.GetComponent<WorldEntityModel.SeaTruckDockingModule>();
            if (component == null)
            {
                return false;
            }

            if (packet.IsDocking)
            {
                var exosuitModule = Server.Instance.Storages.World.GetDynamicEntity(packet.VehicleId);
                if (exosuitModule == null || exosuitModule.TechType != TechType.Exosuit)
                {
                    return false;
                }

                if (component.Dock(exosuitModule))
                {
                    if (Server.Instance.Storages.World.RemoveDynamicEntity(exosuitModule.UniqueId))
                    {
                        if (exosuitModule.IsUsingByPlayer)
                        {
                            var player = Server.Instance.GetPlayer(exosuitModule.OwnershipId);
                            if (player != null)
                            {
                                player.SetInterior(null);
                                player.SetVehicle(null);

                                Server.Instance.Logices.Interact.RemoveBlockByPlayerId(player.UniqueId);
                            }
                        }

                        profile.SendPacketToAllClient(packet);
                    }
                }
            }
            else
            {
                if (Server.Instance.Logices.Interact.IsBlocked(packet.VehicleId))
                {
                    return false;
                }

                if (component.Undock(out var exosuit))
                {
                    exosuit.SetPositionAndRotation(packet.UndockPosition, packet.UndockRotation);

                    this.UndockExosuit(exosuit, profile, packet.IsEnterUndock);

                    packet.Vehicle = exosuit;

                    profile.SendPacketToAllClient(packet);
                }
            }

            return true;
        }

        /**
         *
         * Demirlemeyi çözer.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void UndockExosuit(WorldDynamicEntity vehicle, AuthorizationProfile profile, bool isEnterUndock)
        {
            vehicle.RenewId();

            if (isEnterUndock)
            {
                vehicle.IsUsingByPlayer = true;

                profile.SetVehicle(vehicle.UniqueId);

                Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, vehicle.UniqueId, true);
            }
            else
            {
                vehicle.IsUsingByPlayer = false;
            }

            Server.Instance.Logices.EntityWatcher.ChangeEntityOwnership(vehicle, profile.UniqueId);
            Server.Instance.Storages.World.AddWorldDynamicEntity(vehicle);
        }
    }
}
