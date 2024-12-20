namespace Subnautica.Client.Synchronizations.Processors.Building
{
    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using Constructing = Subnautica.Client.Multiplayer.Constructing;
    using ServerModel  = Subnautica.Network.Models.Server;

    public class CompletedProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.ConstructionCompletedArgs>();
            if (packet.TechType == TechType.None || string.IsNullOrEmpty(packet.UniqueId))
            {
                return false;
            }

            if (Constructing.Builder.GetBuildingProgressType(packet.UniqueId) == BuildingProgressType.Constructing)
            {
                using (EventBlocker.Create(ProcessType.ConstructingCompleted))
                {
                    Constructing.Builder.GetBuilder(packet.UniqueId).Complete(packet.BaseId, packet.Id, true);

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
         * Yapı inşaası tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnConstructingCompleted(ConstructionCompletedEventArgs ev)
        {
            ServerModel.ConstructionCompletedArgs request = new ServerModel.ConstructionCompletedArgs()
            {
                UniqueId       = ev.UniqueId,
                BaseId         = ev.BaseId,
                TechType       = ev.TechType,
                CellPosition   = ev.CellPosition.ToZeroVector3(),
                IsFaceHasValue = ev.IsFaceHasValue,
                LocalPosition  = ev.LocalPosition.ToZeroVector3(),
                LocalRotation  = ev.LocalRotation.ToZeroQuaternion(),
                FaceDirection  = ev.FaceDirection,
                FaceType       = ev.FaceType,
            };

            NetworkClient.SendPacket(request);
        }
    }
}