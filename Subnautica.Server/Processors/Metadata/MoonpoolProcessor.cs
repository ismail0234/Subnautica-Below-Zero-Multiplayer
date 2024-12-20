namespace Subnautica.Server.Processors.Metadata
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Server;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Structures;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;
    using Subnautica.Server.Extensions;

    using Metadata         = Subnautica.Network.Models.Metadata;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class MoonpoolProcessor : MetadataProcessor
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
            var moonpool  = construction.EnsureComponent<Metadata.BaseMoonpool>();
            var component = packet.Component.GetComponent<Metadata.BaseMoonpool>();
            if (component == null)
            {
                return false;
            }
            
            if (!construction.IsConstructed())
            {
                return false;
            }

            if (component.IsDocking)
            {
                if (Server.Instance.Logices.Interact.IsBlocked(construction.UniqueId))
                {
                    return false;
                }

                var entity = Server.Instance.Storages.World.GetDynamicEntity(component.VehicleId);
                if (entity == null || !entity.TechType.IsVehicle(isModule: false))
                {
                    return false;
                }

                if (moonpool.Dock(entity, component.EndPosition, component.EndRotation, Server.Instance.Logices.World.GetServerTimeAsDouble()))
                {
                    if (construction.TechType == TechType.BaseMoonpoolExpansion)
                    {
                        var rearModule = entity.GetSeaTruckRearModule();
                        if (rearModule != null)
                        {
                            moonpool.ExpansionManager.DockTail(rearModule.UniqueId);
                        }
                    }
                    else
                    {
                        this.RemoveSeaTruckBackModule(component.VehicleId, profile.UniqueId, component.BackModulePosition);
                    }

                    Server.Instance.Storages.World.RemoveDynamicEntity(component.VehicleId);

                    if (Server.Instance.Storages.Construction.UpdateMetadata(construction.UniqueId, moonpool))
                    {
                        component.DockingStartTime = moonpool.DockingStartTime;

                        if (entity.IsUsingByPlayer)
                        {
                            var player = Server.Instance.GetPlayer(entity.OwnershipId);
                            if (player != null)
                            {
                                player.SetInterior(null);
                                player.SetVehicle(null);

                                if (entity.TechType == TechType.Exosuit)
                                {
                                    Server.Instance.Logices.Interact.RemoveBlockByPlayerId(player.UniqueId, Logic.Interact.MoonpoolExosuitDock);
                                }
                                else
                                {
                                    Server.Instance.Logices.Interact.RemoveBlockByPlayerId(player.UniqueId, Logic.Interact.MoonpoolSeaTruckDock);
                                }
                            }
                        }

                        profile.SendPacketToAllClient(packet);
                    }
                }
            }
            else if (component.IsUndocking)
            {
                if (Server.Instance.Logices.Interact.IsBlocked(construction.UniqueId) || Server.Instance.Logices.Interact.IsBlocked(moonpool.VehicleId))
                {
                    return false;
                }

                if (moonpool.Undock(out var vehicle))
                {
                    this.MoonpoolUndockTail(construction, moonpool);
                    this.MoonpoolUndock(vehicle, profile, construction.UniqueId);

                    component.Vehicle = vehicle;

                    profile.SendPacketToAllClient(packet);
                }
            }
            else if (component.IsCustomizerOpening)
            {
                if (!Server.Instance.Logices.Interact.IsBlocked(construction.UniqueId))
                {
                    Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, construction.UniqueId, true);
                }
            }
            else if (component.ColorCustomizer != null)
            {
                if (moonpool.IsDocked && Server.Instance.Logices.Interact.IsBlockedByPlayer(profile.UniqueId, construction.UniqueId))
                {
                    Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId);

                    var colorCustomizer = this.GetColorCustomizer(moonpool.Vehicle.Component);
                    if (colorCustomizer != null)
                    {
                        colorCustomizer.CopyFrom(component.ColorCustomizer);

                        component.VehicleId = moonpool.Vehicle.UniqueId;

                        profile.SendPacketToOtherClients(packet);
                    }
                }
            }

            return true;
        }

        /**
         *
         * SeaTruck arka modül bağlantısını keser.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private string RemoveSeaTruckBackModule(string vehicleId, string playerUniqueId, ZeroVector3 position)
        {
            var backModuleId = Server.Instance.Storages.World.RemoveSeaTruckConnection(vehicleId);
            if (backModuleId.IsNull())
            {
                return null;
            }

            var backModule = Core.Server.Instance.Storages.World.GetDynamicEntity(backModuleId);
            if (backModule == null)
            {
                return null;
            }

            backModule.SetPosition(position);

            Server.Instance.Logices.EntityWatcher.ChangeEntityOwnership(backModule, playerUniqueId);
            return backModule.UniqueId;
        }

        /**
         *
         * Color Customizer döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private ZeroColorCustomizer GetColorCustomizer(NetworkDynamicEntityComponent component) 
        {
            if (component is WorldEntityModel.SeaTruck)
            {
                return component.GetComponent<WorldEntityModel.SeaTruck>().ColorCustomizer;
            }
            
            if (component is WorldEntityModel.Exosuit)
            {
                return component.GetComponent<WorldEntityModel.Exosuit>().ColorCustomizer;
            }

            return null;
        }

        /**
         *
         * Demirlemeyi çözer.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void MoonpoolUndock(WorldDynamicEntity vehicle, AuthorizationProfile profile, string moonpoolId)
        {
            var customPlayerId = Interact.GetCustomId(profile.UniqueId);

            Server.Instance.Logices.Interact.AddBlock(customPlayerId  , moonpoolId      , true);
            Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, vehicle.UniqueId, true);

            if (vehicle.TechType == TechType.Exosuit)
            {
                Server.Instance.Logices.Interact.RemoveBlockByPlayerId(customPlayerId, Logic.Interact.MoonpoolExosuitUndock);
            }
            else
            {
                Server.Instance.Logices.Interact.RemoveBlockByPlayerId(customPlayerId, Logic.Interact.MoonpoolSeaTruckUndock);
            }

            vehicle.IsUsingByPlayer = true;
            vehicle.RenewId();

            Server.Instance.Storages.World.AddWorldDynamicEntity(vehicle);
            Server.Instance.Logices.EntityWatcher.ChangeEntityOwnership(vehicle, profile.UniqueId);

            profile.SetVehicle(vehicle.UniqueId);
        }

        /**
         *
         * Kuyruk Demirlemesini çözer.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool MoonpoolUndockTail(ConstructionItem construction, Metadata.BaseMoonpool moonpool)
        {
            if (construction.TechType != TechType.BaseMoonpoolExpansion || moonpool.ExpansionManager.IsTailDocked() == false)
            {
                return false;
            }

            var fakePlayerId = Network.Identifier.GenerateUniqueId();

            Server.Instance.Logices.Interact.AddBlock(fakePlayerId, moonpool.ExpansionManager.TailId, true);
            Server.Instance.Logices.Interact.RemoveBlockByPlayerId(fakePlayerId, Logic.Interact.MoonpoolSeaTruckUndock);

            moonpool.ExpansionManager.UndockTail();
            return true;
        }
    }
}
