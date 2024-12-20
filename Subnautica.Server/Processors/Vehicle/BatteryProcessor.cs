namespace Subnautica.Server.Processors.Vehicle
{
    using System.Linq;

    using Server.Core;

    using Subnautica.API.Features;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel      = Subnautica.Network.Models.Server;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class BatteryProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.VehicleBatteryArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (packet.IsOpening)
            {
                if (Server.Instance.Logices.Interact.IsBlocked(packet.BatterySlotId))
                {
                    return false;
                }

                Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, packet.BatterySlotId, true);

                profile.SendPacket(packet);
            }
            else
            {
                if (Server.Instance.Logices.Interact.IsBlocked(packet.BatterySlotId, profile.UniqueId))
                {
                    return false;
                }
                
                var entity = Server.Instance.Storages.World.GetVehicle(packet.UniqueId);
                if (entity == null)
                {
                    return false;
                }

                PowerCell powerCell = null;
                if (entity.TechType == TechType.SeaTruck)
                {
                    var component = entity.Component.GetComponent<WorldEntityModel.SeaTruck>();
                    if (component != null)
                    {
                        powerCell = component.PowerCells.Where(q => q.UniqueId == packet.BatterySlotId).FirstOrDefault();
                    }
                }
                else if (entity.TechType == TechType.Exosuit)
                {
                    var component = entity.Component.GetComponent<WorldEntityModel.Exosuit>();
                    if (component != null)
                    {
                        powerCell = component.PowerCells.Where(q => q.UniqueId == packet.BatterySlotId).FirstOrDefault();
                    }
                }

                if (powerCell == null)
                {
                    return false;
                }

                powerCell.SetBatteryType(packet.BatteryType);
                powerCell.Charge = packet.IsAdding ? packet.Charge : -1f;

                profile.SendPacketToOtherClients(packet);
            }

            return true;
        }
    }
}
