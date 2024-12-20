namespace Subnautica.Server.Processors.Vehicle
{
    using System.Linq;

    using Server.Core;

    using Subnautica.API.Extensions;
    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel   = Subnautica.Network.Models.Server;
    using MetadataModel = Subnautica.Network.Models.Metadata;
    using Subnautica.API.Features;

    public class SeaTruckConnectionProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.SeaTruckConnectionArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (packet.IsEject)
            {
                if (Server.Instance.Logices.Interact.IsBlocked(packet.FrontModuleId))
                {
                    return false;
                }

                packet.BackModuleId = Server.Instance.Storages.World.RemoveSeaTruckConnection(packet.FrontModuleId);

                if (packet.BackModuleId.IsNotNull())
                {
                    var backModule = Core.Server.Instance.Storages.World.GetDynamicEntity(packet.BackModuleId);
                    if (backModule != null)
                    {
                        packet.ModuleId = backModule.Id;
                        packet.Position = backModule.Position;
                        packet.Rotation = backModule.Rotation;
                    }

                    profile.SendPacketToAllClient(packet);
                }
            }
            else if (packet.IsMoonpoolExpansion)
            {
                var construction = Server.Instance.Storages.Construction.GetConstruction(packet.FrontModuleId);
                if (construction == null)
                {
                    return false;
                }

                var component = construction.EnsureComponent<MetadataModel.BaseMoonpool>();
                if (component == null)
                {
                    return false;
                }

                if (packet.IsConnect)
                {
                    if (!component.ExpansionManager.IsTailDocked() && Server.Instance.Storages.World.AddSeaTruckConnection(packet.BackModuleId, packet.FirstModuleId, false))
                    {
                        component.ExpansionManager.DockTail(packet.BackModuleId);

                        profile.SendPacketToAllClient(packet);
                    }
                }
                else
                {
                    if (component.ExpansionManager.IsTailDocked())
                    {
                        Server.Instance.Storages.World.RemoveSeaTruckConnection(packet.FirstModuleId, false);

                        component.ExpansionManager.UndockTail();

                        profile.SendPacketToAllClient(packet);
                    }
                }
            }
            else if (packet.IsConnect)
            {
                if (Server.Instance.Storages.World.AddSeaTruckConnection(packet.FrontModuleId, packet.BackModuleId))
                {
                    profile.SendPacketToAllClient(packet);
                }
            }
            else
            {
                packet.BackModuleId = Server.Instance.Storages.World.RemoveSeaTruckConnection(packet.FrontModuleId);

                if (packet.BackModuleId.IsNotNull())
                {
                    profile.SendPacketToAllClient(packet);
                }
            }

            return true;
        }
    }
}
