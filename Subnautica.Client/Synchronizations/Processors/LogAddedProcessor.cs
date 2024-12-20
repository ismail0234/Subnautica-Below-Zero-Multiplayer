namespace Subnautica.Client.Synchronizations.Processors.PDA
{
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;
    using Subnautica.API.Features;
    using Subnautica.API.Enums;

    using ServerModel = Subnautica.Network.Models.Server;

    public class LogAddedProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.PDALogAddedArgs>();

            using (EventBlocker.Create(ProcessType.PDALogAdded))
            {
             //   PDALog.Add(packet.Key, true);
            }

            return true;
        }

        /**
         *
         * PDA log kaydı eklenince tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPDALogAdded(PDALogAddedEventArgs ev)
        {/*
            ServerModel.PDALogAddedArgs result = new ServerModel.PDALogAddedArgs()
            {
                Key = ev.Key,
                Timestamp = ev.Timestamp,
            };

            TcpClient.SendAsync(result);
            */
        }
    }
}