namespace Subnautica.Server.Processors.World
{
    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class PrecursorTeleporterProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.PrecursorTeleporterArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (packet.IsTerminal)
            {
                if (Server.Instance.Storages.World.ActivateTeleportPortal(packet.TeleporterId) && Server.Instance.Storages.World.ActivateTeleportPortal(packet.UniqueId))
                {
                    profile.SendPacketToAllClient(packet);
                }
            }
            else if (packet.IsTeleportStart)
            {
                profile.SendPacketToOtherClients(packet);
            }
            else if (packet.IsTeleportCompleted)
            {
                profile.SendPacketToOtherClients(packet);
            }

            return true;
        }
    }
}
