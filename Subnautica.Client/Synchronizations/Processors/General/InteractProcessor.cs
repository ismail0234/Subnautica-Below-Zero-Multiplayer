namespace Subnautica.Client.Synchronizations.Processors.General
{
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ClientModel = Subnautica.Network.Models.Client;
    using ServerModel = Subnautica.Network.Models.Server;

    public class InteractProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ClientModel.InteractArgs>();
            if (packet == null)
            {
                return true;
            }

            if (packet.List.Count <= 0)
            {
                Interact.ClearAll();
            }
            else
            {
                Interact.SetList(packet.List);
            }

            return true;
        }

        /**
         *
         * PDA kapatıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnClosing(PDAClosingEventArgs ev)
        {
            if (Interact.IsBlockedByMe() && !ZeroGame.IsPlayerPiloting())
            {
                InteractProcessor.SendDataToServer(false);
            }
        }

        /**
         *
         * Tabela seçimi kaldırıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSignDeselect(SignDeselectEventArgs ev)
        {
            InteractProcessor.SendDataToServer(false);
        }

        /**
         *
         * Pil yerleştirilme kapatıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEnergyMixinClosed(EnergyMixinClosedEventArgs ev)
        {
            if (Interact.IsBlockedByMe(ev.BatterySlotId))
            {
                InteractProcessor.SendDataToServer(false);
            }
        }

        /**
         *
         * Sunucuya Veri Gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool SendDataToServer(bool isOpening = false)
        {
            ServerModel.InteractArgs result = new ServerModel.InteractArgs()
            {
                IsOpening = isOpening,
            };

            NetworkClient.SendPacket(result);
            return true;
        }
    }
}