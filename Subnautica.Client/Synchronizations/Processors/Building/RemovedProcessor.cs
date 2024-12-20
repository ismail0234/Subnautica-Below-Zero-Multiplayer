namespace Subnautica.Client.Synchronizations.Processors.Building
{
    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.API.Extensions;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ServerModel  = Subnautica.Network.Models.Server;
    using Constructing = Subnautica.Client.Multiplayer.Constructing;

    public class RemovedProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.ConstructionRemovedArgs>();
            if (packet.TechType == TechType.None || string.IsNullOrEmpty(packet.UniqueId))
            {
                return false;
            }

            if (Constructing.Builder.GetBuildingProgressType(packet.UniqueId) == BuildingProgressType.Constructing)
            {
                using (EventBlocker.Create(ProcessType.ConstructingRemoved))
                {
                    Constructing.Builder.Destroy(packet.UniqueId, callSound: true);

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
         * Yapı yıkıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnConstructingRemoved(ConstructionRemovedEventArgs ev)
        {
            ServerModel.ConstructionRemovedArgs request = new ServerModel.ConstructionRemovedArgs()
            {
                UniqueId = ev.UniqueId,
                TechType = ev.TechType,
                Cell     = ev.Cell?.ToZeroInt3()
            };

            NetworkClient.SendPacket(request);
        }
    }
}
