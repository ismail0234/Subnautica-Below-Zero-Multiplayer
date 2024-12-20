namespace Subnautica.Client.Synchronizations.Processors.Building
{
    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using Constructing = Subnautica.Client.Multiplayer.Constructing;
    using ServerModel  = Subnautica.Network.Models.Server;

    public class AmountChangedProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.ConstructionAmountChangedArgs>();
            if (packet.TechType == TechType.None || string.IsNullOrEmpty(packet.UniqueId))
            {
                return false;
            }

            if (Constructing.Builder.GetBuildingProgressType(packet.UniqueId) == BuildingProgressType.Constructing)
            {
                using (EventBlocker.Create(ProcessType.ConstructingAmountChanged))
                {
                    Constructing.Builder.GetBuilder(packet.UniqueId).SetConstructedAmount(packet.Amount);
                }
            }

            return true;
        }

        /**
         *
         * Yapı inşaa değeri değiştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnConstructingAmountChanged(ConstructionAmountChangedEventArgs ev)
        {
            ServerModel.ConstructionAmountChangedArgs request = new ServerModel.ConstructionAmountChangedArgs()
            {
                UniqueId    = ev.UniqueId,
                TechType    = ev.TechType,
                IsConstruct = ev.IsConstruct,
                Amount      = ev.Amount,
            };

            NetworkClient.SendPacket(request);
        }
    }
}
