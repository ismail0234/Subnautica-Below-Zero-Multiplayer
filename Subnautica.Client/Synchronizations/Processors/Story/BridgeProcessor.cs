namespace Subnautica.Client.Synchronizations.Processors.Story
{
    using System;

    using global::Story;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class BridgeProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.StoryBridgeArgs>();
            if (packet.IsClickedFluid)
            {
                using (EventBlocker.Create(ProcessType.StoryTrigger))
                {
                    StoryGoalManager.main.OnGoalComplete(packet.StoryKey);
                }

                if (ZeroPlayer.IsPlayerMine(packet.GetPacketOwnerId()))
                {
                    World.DestroyItemFromPlayer(TechType.HydraulicFluid);
                }

                var bridge = Network.Identifier.GetComponentByGameObject<GlacialBasinBridgeController>(packet.UniqueId, true);
                if (bridge)
                {
                    bridge.insertedItem.SetActive(true);
                    bridge.itemHatchTimeline.Play();
                }
            }
            else if (packet.IsClickedExtend || packet.IsClickedRetract)
            {
                Network.Session.Current.Story.Bridge.Time       = packet.Time;
                Network.Session.Current.Story.Bridge.IsExtended = packet.IsClickedExtend;

                if (packet.IsFirstExtension)
                {
                    Network.Session.Current.Story.Bridge.IsFirstExtension = true;
                }

                BridgeToggle(packet.UniqueId, packet.Time, packet.IsClickedExtend, Network.Session.Current.Story.Bridge.IsFirstExtension);
            }

            return true;
        }

        /**
         *
         * Koprüyü açar/kapatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool BridgeToggle(string uniqueId, float endTime, bool isExtend, bool isFirstExtension)
        {
            var bridge = Network.Identifier.GetComponentByGameObject<GlacialBasinBridgeController>(uniqueId, true);
            if (bridge == null)
            {
                return false;
            }

            if (!bridge.itemInsertedStoryGoal.IsComplete())
            {
                return false;
            }

            if (isExtend)
            {
                bridge.retractBridgeTimeline.Stop();
                bridge.extendBridgeTimeline.MultiplayerPlay();
            }
            else
            {
                bridge.extendBridgeTimeline.Stop();
                bridge.retractBridgeTimeline.MultiplayerPlay();
            }

            if (isFirstExtension)
            {
                if (!bridge.firstExtension)
                {
                    bridge.firstExtension = true;

                    bridge.icicleBreakTimeine.MultiplayerPlay();
                }
            }

            var animationTime = (float) bridge.retractBridgeTimeline.duration + 0.1f;

            if (isExtend)
            {
                animationTime = (float) bridge.extendBridgeTimeline.duration + 0.1f;
            }

            var animatonDiff  = (endTime - DayNightCycle.main.timePassedAsFloat);
            if (animatonDiff <= 1f)
            {
                if (isExtend)
                {
                    bridge.extendBridgeTimeline.time = bridge.extendBridgeTimeline.duration;
                }
                else
                {
                    bridge.retractBridgeTimeline.time = bridge.retractBridgeTimeline.duration;
                }
            }
            else
            {
                var differentTime = Math.Abs(animationTime - animatonDiff);
                if (differentTime > 1f)
                {
                    if (isExtend)
                    {
                        bridge.extendBridgeTimeline.time = animatonDiff;
                    }
                    else
                    {
                        bridge.retractBridgeTimeline.time = animatonDiff;
                    }
                }
            }

            return true;
        }

        /**
         *
         * Koprü spawn olduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBridgeInitialized(BridgeInitializedEventArgs ev)
        {
            BridgeToggle(ev.UniqueId, Network.Session.Current.Story.Bridge.Time, Network.Session.Current.Story.Bridge.IsExtended, Network.Session.Current.Story.Bridge.IsFirstExtension);
        }

        /**
         *
         * Koprü terminaline tıklanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBridgeTerminalClicking(BridgeTerminalClickingEventArgs ev)
        {
            ev.IsAllowed = false;

            if (ev.IsExtend)
            {
                BridgeProcessor.SendPacketToServer(ev.UniqueId, isClickedExtend: true, time: ev.Time);
            }
            else
            {
                BridgeProcessor.SendPacketToServer(ev.UniqueId, isClickedRetract: true, time: ev.Time);
            }
        }

        /**
         *
         * Köprü sol konsola tıklanınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBridgeFluidClicking(BridgeFluidClickingEventArgs ev)
        {
            ev.IsAllowed = false;

            BridgeProcessor.SendPacketToServer(ev.UniqueId, storyKey: ev.StoryKey, isClickedFluid: true);
        }

        /**
         *
         * Sunucuya veri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqued, string storyKey = null, float time = 0, bool isClickedFluid = false, bool isClickedExtend = false, bool isClickedRetract = false)
        {
            ServerModel.StoryBridgeArgs result = new ServerModel.StoryBridgeArgs()
            {
                UniqueId         = uniqued,
                StoryKey         = storyKey,
                IsClickedFluid   = isClickedFluid,
                IsClickedExtend  = isClickedExtend,
                IsClickedRetract = isClickedRetract,
                Time             = time,
            };

            NetworkClient.SendPacket(result);
        }
    }
}
