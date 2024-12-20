namespace Subnautica.Server.Processors.Building
{
    using Server.Core;
    
    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Logic;

    using ServerModel = Subnautica.Network.Models.Server;

    public class AmountChangedProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.ConstructionAmountChangedArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            var construction = Server.Instance.Storages.Construction.GetConstruction(packet.UniqueId);
            if (construction == null)
            {
                return false;
            }

            if (construction.ConstructedAmount == 1f)
            {
                API.Features.Log.Warn("AMOUNT CHANGED ERROR 1 => " + construction.UniqueId + ", TECH: " + construction.TechType + ", Amount: " + construction.ConstructedAmount);
            }

            if (packet.Amount == 1f)
            {
                API.Features.Log.Warn("AMOUNT CHANGED ERROR 2 => " + construction.UniqueId + ", TECH: " + construction.TechType + ", Amount: " + construction.ConstructedAmount);
                API.Features.Log.Warn("AMOUNT CHANGED ERROR 3 => " + packet.UniqueId + ", IsConstruct: " + packet.IsConstruct + ", Amount: " + packet.Amount);
            }

            if (packet.IsConstruct && construction.ConstructedAmount == 1f)
            {
                return false;
            }

            if (Server.Instance.Logices.Interact.IsBlocked(packet.UniqueId))
            {
                if (!Server.Instance.Logices.Interact.IsBlockedByPlayer(profile.UniqueId, packet.UniqueId))
                {
                    return false;
                }
            }

            if (Server.Instance.Storages.Construction.UpdateConstructionAmount(packet.UniqueId, packet.Amount))
            {
                if (!Server.Instance.Logices.Interact.IsBlocked(packet.UniqueId))
                {
                    Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, packet.UniqueId, true);
                }

                Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId, Interact.ConstructionTimeout);

                profile.SendPacketToOtherClients(packet);
            }

            return true;
        }
    }
}
