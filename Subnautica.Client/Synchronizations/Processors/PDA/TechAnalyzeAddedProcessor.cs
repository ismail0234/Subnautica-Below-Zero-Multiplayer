namespace Subnautica.Client.Synchronizations.Processors.PDA
{
    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class TechAnalyzeAddedProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.TechAnalyzeAddedArgs>();

            using (EventBlocker.Create(ProcessType.TechAnalyzeAdded))
            using (EventBlocker.Create(ProcessType.TechnologyAdded))
            {
                KnownTech.Analyze(packet.TechType, true, packet.Verbose);
            }
      
            return true;
        }

        /**
         *
         * Teknoloji analiz edildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnTechAnalyzeAdded(TechAnalyzeAddedEventArgs ev)
        {
            ServerModel.TechAnalyzeAddedArgs result = new ServerModel.TechAnalyzeAddedArgs()
            {
                TechType = ev.TechType,
                Verbose  = ev.Verbose,
            };

            NetworkClient.SendPacket(result);
        }
    }
}