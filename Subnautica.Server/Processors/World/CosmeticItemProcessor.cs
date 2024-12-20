namespace Subnautica.Server.Processors.World
{
    using Server.Core;

    using Subnautica.API.Extensions;
    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class CosmeticItemProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.CosmeticItemArgs>();
            if (packet.UniqueId.IsNull())
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (packet.PickupItem != null)
            {
                if (Server.Instance.Logices.Storage.TryPickupItem(packet.PickupItem, profile.InventoryItems))
                {
                    profile.SendPacketToAllClient(packet);
                }
            }
            else
            {
                if (Server.Instance.Storages.World.AddCosmeticItem(packet.UniqueId, packet.BaseId, packet.TechType, packet.Position, packet.Rotation))
                {
                    packet.CosmeticItem = Server.Instance.Storages.World.GetCosmeticItem(packet.UniqueId);

                    profile.SendPacketToAllClient(packet);
                }
            }

            return true;
        }
    }
}
