namespace Subnautica.Client.Synchronizations.Processors.Building
{
    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class DeconstructionBeginProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.DeconstructionBeginArgs>();
            if (packet.UniqueId.IsNull())
            {
                return false;
            }

            if (packet.IsFailed)
            {
                ErrorMessage.AddMessage(global::Language.main.Get("DeconstructAttachedError"));
            }
            else
            {
                using (EventBlocker.Create(ProcessType.DeconstructionBegin))
                {
                    Multiplayer.Constructing.Builder.Deconstruct(packet.UniqueId, packet.Id);

                    ConstructionSyncedProcessor.UpdateConstructionSync();
                }
            }

            return true;
        }

        /**
         *
         * Sınıf başlatılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnStart()
        {
            this.SetWaitingForNextFrame(true);
        }

        /**
         *
         * Yapı inşaası ilk kaldırma işlemi olduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnDeconstructionBegin(DeconstructionBeginEventArgs ev)
        {
            ev.IsAllowed = false;

            if (!Network.HandTarget.IsBlocked(ev.UniqueId))
            {
                ServerModel.DeconstructionBeginArgs request = new ServerModel.DeconstructionBeginArgs()
                {
                    UniqueId = ev.UniqueId,
                };

                NetworkClient.SendPacket(request);
            }
        }
    }
}