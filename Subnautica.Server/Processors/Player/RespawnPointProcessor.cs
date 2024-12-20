namespace Subnautica.Server.Processors.Player
{
    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class RespawnPointProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.PlayerRespawnPointArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            profile.SetRespawnPointId(packet.UniqueId);
            return true;
        }
    }
}
