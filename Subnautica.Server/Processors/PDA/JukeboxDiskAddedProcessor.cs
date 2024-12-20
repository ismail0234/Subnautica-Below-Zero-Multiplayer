namespace Subnautica.Server.Processors.PDA
{
    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class JukeboxDiskAddedProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.JukeboxDiskAddedArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (Server.Instance.Storages.World.AddJukeboxDisk(packet.TrackFile))
            {
                profile.SendPacketToOtherClients(packet);
            }

            return true;
        }
    }
}
