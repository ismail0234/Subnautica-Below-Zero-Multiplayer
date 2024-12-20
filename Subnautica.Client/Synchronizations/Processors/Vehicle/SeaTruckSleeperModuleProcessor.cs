namespace Subnautica.Client.Synchronizations.Processors.Vehicle
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Client.MonoBehaviours.Vehicle;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class SeaTruckSleeperModuleProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.SeaTruckSleeperModuleArgs>();
            if (packet.UniqueId.IsNull())
            {
                return false;
            }

            if (packet.IsOpeningPictureFrame)
            {
                var gameObject = Network.Identifier.GetComponentByGameObject<global::PictureFrame>(packet.UniqueId);
                if (gameObject == null)
                {
                    return false;
                }

                using (EventBlocker.Create(TechType.PictureFrame))
                {
                    gameObject.OnHandClick(null);
                }
            }
            else if (packet.IsSelectingPictureFrame)
            {
                var action = new ItemQueueAction();
                action.RegisterProperty("UniqueId" , packet.UniqueId);
                action.RegisterProperty("ImageName", packet.PictureFrameName);
                action.RegisterProperty("ImageData", packet.PictureFrameData);
                action.OnProcessCompleted = this.OnPictureFrameSelected;
                
                Entity.ProcessToQueue(action);
            }
            else if (packet.SleepingSide != global::Bed.BedSide.None)
            {
                var action = new ItemQueueAction();
                action.RegisterProperty("UniqueId"    , packet.UniqueId);
                action.RegisterProperty("PlayerId"    , packet.GetPacketOwnerId());
                action.RegisterProperty("SleepingSide", packet.SleepingSide);
                action.RegisterProperty("IsSleeping"  , packet.IsSleeping);
                action.OnProcessCompleted = this.OnBedSleepProcessCompleted;

                Entity.ProcessToQueue(action);
            }

            return true;
        }

        /**
         *
         * işlem tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void OnBedSleepProcessCompleted(ItemQueueProcess item)
        {
            var uniqueId     = item.Action.GetProperty<string>("UniqueId");
            var playerId     = item.Action.GetProperty<byte>("PlayerId");
            var sleepingSide = item.Action.GetProperty<global::Bed.BedSide>("SleepingSide");
            var isSleeping   = item.Action.GetProperty<bool>("IsSleeping");

            var player = ZeroPlayer.GetPlayerById(playerId);
            if (player == null)
            {
                return;
            }

            if (player.IsMine)
            {
                if (isSleeping)
                {
                    player.OnHandClickBed(uniqueId, sleepingSide);
                }
            }
            else
            {
                if (isSleeping)
                {
                    player.LieDownStartCinematicBed(uniqueId, sleepingSide);
                }
                else
                {
                    player.StandupStartCinematicBed(uniqueId, sleepingSide);
                }
            }
        }

        /**
         *
         * işlem tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void OnPictureFrameSelected(ItemQueueProcess item)
        {
            var pictureFrame = Network.Identifier.GetComponentByGameObject<global::PictureFrame>(item.Action.GetProperty<string>("UniqueId"));
            if (pictureFrame)
            {
                pictureFrame.MultiplayerSelectImage(item.Action.GetProperty<string>("ImageName"), item.Action.GetProperty<byte[]>("ImageData"));
            }
        }

        /**
         *
         * SeaTruck Resim çerçevesi açılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSeaTruckPictureFrameOpening(SeaTruckPictureFrameOpeningEventArgs ev)
        {
            ev.IsAllowed = false;

            SeaTruckSleeperModuleProcessor.SendPacketToServer(ev.UniqueId, isOpeningPictureFrame: true);
        }

        /**
         *
         * SeaTruck Resim çerçevesi resim seçilirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSeaTruckPictureFrameImageSelecting(SeaTruckPictureFrameImageSelectingEventArgs ev)
        {
            ev.IsAllowed = false;

            SeaTruckSleeperModuleProcessor.SendPacketToServer(ev.UniqueId, isSelectingPictureFrame: true, pictureFrameData: ev.ImageData);
        }

        /**
         *
         * Şarkı kutusunda veri değişimi olduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnJukeboxUsed(JukeboxUsedEventArgs ev)
        {
            if (ev.IsSeaTruckModule)
            {
                SeaTruckSleeperModuleProcessor.SendPacketToServer(ev.UniqueId, jukeboxData: ev.Data);
            }
        }

        /**
         *
         * Kullanıcı yatağa tıkladığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBedEnterInUseMode(BedEnterInUseModeEventArgs ev)
        {
            if (ev.IsSeaTruckModule)
            {
                ev.IsAllowed = false;

                if (!Network.HandTarget.IsBlocked(ev.UniqueId))
                {
                    SeaTruckSleeperModuleProcessor.SendPacketToServer(ev.UniqueId, sleepingSide: ev.Side, isSleeping: true);
                }
            }
        }

        /**
         *
         * Kullanıcı yatak'dan kalktığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBedExitInUseMode(BedExitInUseModeEventArgs ev)
        {
            if (ev.IsSeaTruckModule)
            {
                SeaTruckSleeperModuleProcessor.SendPacketToServer(ev.UniqueId, sleepingSide: global::Bed.BedSide.Right, isSleeping: false);
            }
        }

        /**
         *
         * SeaTruck modülü başlatılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSeaTruckModuleInitialized(SeaTruckModuleInitializedEventArgs ev)
        {
            if (ev.TechType == TechType.SeaTruckSleeperModule)
            {
                ev.Module.gameObject.EnsureComponent<MultiplayerSeaTruckSleeperModule>();
            }
        }

        /**
         *
         * Sunucuya paketi gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId, bool isOpeningPictureFrame = false, bool isSelectingPictureFrame = false, byte[] pictureFrameData = null, CustomProperty jukeboxData = null, global::Bed.BedSide sleepingSide = global::Bed.BedSide.None, bool isSleeping = false)
        {
            ServerModel.SeaTruckSleeperModuleArgs request = new ServerModel.SeaTruckSleeperModuleArgs()
            {
                UniqueId                = uniqueId,
                IsOpeningPictureFrame   = isOpeningPictureFrame,
                IsSelectingPictureFrame = isSelectingPictureFrame,
                PictureFrameData        = pictureFrameData,
                JukeboxData             = jukeboxData,
                SleepingSide            = sleepingSide,
                IsSleeping              = isSleeping,
            };

            NetworkClient.SendPacket(request);
        }
    }
}