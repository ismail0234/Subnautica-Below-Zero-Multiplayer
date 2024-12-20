namespace Subnautica.Server.Processors.Building
{
    using Server.Core;
    
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Storage.Construction;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class GhostTryPlacingProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.ConstructionGhostTryPlacingArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (packet.IsError)
            {
                profile.SendPacketToAllClient(packet);
            }
            else
            {
                ConstructionItem item = new ConstructionItem()
                {
                    Id            = Server.Instance.Logices.World.GetNextConstructionId(),
                    UniqueId      = packet.UniqueId,
                    TechType      = packet.TechType,
                    LastRotation  = packet.LastRotation,
                    PlacePosition = packet.Position,
                    IsBasePiece   = packet.IsBasePiece
                };

                API.Features.Log.Info("TRYPLACE -> " + packet.UniqueId + ", TechType: " + packet.TechType);

                if (Server.Instance.Storages.Construction.AddConstructionItem(item))
                {
                    profile.SendPacketToAllClient(packet);
                }
            }

            return true;
        }
    }
}