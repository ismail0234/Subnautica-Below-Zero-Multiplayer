namespace Subnautica.Client.Synchronizations.Processors.Technology
{
    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using UnityEngine;

    using ServerModel  = Subnautica.Network.Models.Server;

    public class FragmentAddedProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.TechnologyFragmentAddedArgs>();

            using (EventBlocker.Create(ProcessType.TechnologyFragmentAdded))
            using (EventBlocker.Create(ProcessType.TechnologyAdded))
            {
                PDAScanner.Add(packet.TechType, packet.Unlocked);

                if (packet.TotalFragment > 1 && packet.TechType != TechType.None)
                {
                    float num2 = (float)Mathf.RoundToInt((float)((double)packet.Unlocked / (double)packet.TotalFragment * 100.0));
                    ErrorMessage.AddError(global::Language.main.GetFormat("ScannerInstanceScanned", global::Language.main.Get(packet.TechType.AsString()), num2, packet.Unlocked, packet.TotalFragment));
                }

                if (packet.UniqueId.IsNotNull())
                {
                    PDAScanner.fragments.Add(packet.UniqueId, 1f);
                }
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
        public static void OnTechnologyFragmentAdded(TechnologyFragmentAddedEventArgs ev)
        {
            ServerModel.TechnologyFragmentAddedArgs result = new ServerModel.TechnologyFragmentAddedArgs()
            {
                UniqueId      = ev.UniqueId,
                TechType      = ev.TechType,
                TotalFragment = ev.TotalFragment,
                Unlocked      = ev.Unlocked,
            };

            NetworkClient.SendPacket(result);
        }
    }
}
