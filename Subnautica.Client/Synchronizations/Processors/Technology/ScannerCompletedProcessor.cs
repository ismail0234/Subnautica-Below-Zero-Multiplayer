namespace Subnautica.Client.Synchronizations.Processors.Technology
{
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class ScannerCompletedProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.ScannerCompletedArgs>();

            if (!PDAScanner.complete.Contains(packet.TechType))
            {
                PDAScanner.complete.Add(packet.TechType);
            }

            return true;
        }

        /**
         *
         * Tarama tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnScannerCompleted(ScannerCompletedEventArgs ev)
        {
            ServerModel.ScannerCompletedArgs result = new ServerModel.ScannerCompletedArgs()
            {
                TechType = ev.TechType,
            };

            NetworkClient.SendPacket(result);
        }
    }
}

