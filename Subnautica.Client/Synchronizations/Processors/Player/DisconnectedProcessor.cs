namespace Subnautica.Client.Synchronizations.Processors.Player
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Extensions;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;
    
    public class DisconnectedProcessor : NormalProcessor
    {        
        /**
        *
        * Gelen veriyi işler
        *
        * @author Ismail <ismaiil_0234@hotmail.com>
        *
        */
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.PlayerDisconnectedArgs>();
            if (packet.UniqueId.IsNull())
            {
                return false;
            }

            var player = ZeroPlayer.GetPlayerByUniqueId(packet.UniqueId);
            if (player == null || player.IsMine)
            {
                return false;
            }

            player.SendMessageToParent("OnMultiplayerPlayerDisconnected", player);

            foreach (var cinematic in player.GetCinematics())
            {
                cinematic.OnPlayerDisconnected();
            }

            Multiplayer.Furnitures.Bed.ClearBed(packet.UniqueId);

            ZeroPlayer.Destroy(packet.UniqueId);

            Discord.UpdateRichPresence(null, ZeroLanguage.GetServerPlayerCount());
            return true;
        }
    }
}