namespace Subnautica.Client.Synchronizations.Processors.Technology
{
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;
    using Subnautica.API.Features;
    using Subnautica.API.Enums;

    using ServerModel  = Subnautica.Network.Models.Server;

    public class AddedProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.TechnologyAddedArgs>();

            using (EventBlocker.Create(ProcessType.TechnologyAdded))
            {
                if (PDAScanner.GetPartialEntryByKey(packet.TechType, out var entry))
                {
                    PDAScanner.partial.Remove(entry);
                }

                if (!PDAScanner.complete.Contains(packet.TechType))
                {
                    PDAScanner.complete.Add(packet.TechType);
                }

                KnownTech.Add(packet.TechType, false, packet.Verbose);
            }

            return true;
        }

        /**
         *
         * Teknoloji taraması yapıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnTechnologyAdded(TechnologyAddedEventArgs ev)
        {
            ServerModel.TechnologyAddedArgs result = new ServerModel.TechnologyAddedArgs()
            {
                TechType = ev.TechType,
                Verbose  = ev.Verbose,
            };

            NetworkClient.SendPacket(result);
        }
    }
}