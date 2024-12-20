namespace Subnautica.Server.Processors.Building
{
    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class CompletedProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.ConstructionCompletedArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            var construction = Server.Instance.Storages.Construction.GetConstruction(packet.UniqueId);
            if (construction == null || construction.ConstructedAmount == 1f)
            {
                API.Features.Log.Warn("COMLEPTE ERROR => " + (construction == null) + ", A: " + (construction != null ? construction.ConstructedAmount : -1f) + ", U: " + packet.UniqueId + ", B: " + packet.BaseId + ", CB: " + construction?.BaseId + ", P: " + profile.PlayerName);
                return false;
            }

            if (Server.Instance.Storages.Construction.ConstructionComplete(packet.UniqueId, packet.BaseId, packet.CellPosition, packet.IsFaceHasValue, packet.LocalPosition, packet.LocalRotation, packet.FaceDirection, packet.FaceType))
            {
                if (packet.BaseId != construction.BaseId)
                {
                    packet.BaseId = construction.BaseId;
                }
                
                packet.Id = construction.Id;

                profile.SendPacketToAllClient(packet);
            }

            return true;
        }
    }
}