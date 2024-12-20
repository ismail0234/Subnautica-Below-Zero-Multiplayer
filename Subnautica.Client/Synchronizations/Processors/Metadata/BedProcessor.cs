namespace Subnautica.Client.Synchronizations.Processors.Metadata
{
    using System;
    using System.Linq;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Client.Modules;
    using Subnautica.Client.MonoBehaviours.General;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Server;

    using Metadata    = Subnautica.Network.Models.Metadata;
    using ServerModel = Subnautica.Network.Models.Server;

    public class BedProcessor : MetadataProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(string uniqueId, TechType techType, MetadataComponentArgs packet, bool isSilence)
        {
            var component = packet.Component.GetComponent<Metadata.Bed>();
            if (component == null)
            {
                return false;
            }

            if (isSilence)
            {
                var bedSide = component.Sides.Where(q => q.IsUsing()).FirstOrDefault();
                if (bedSide != null)
                {
                    component.IsSleeping  = true;
                    component.CurrentSide = bedSide;

                    packet.SetPacketOwnerId(bedSide.PlayerId_v2);

                    MultiplayerChannelProcessor.AddPacketToProcessor(NetworkChannel.StartupWorldLoaded, packet);
                }

                return true;
            }
            else 
            {
                var player = ZeroPlayer.GetPlayerById(packet.GetPacketOwnerId());
                if (player == null)
                {
                    return false;
                }

                if (player.IsMine)
                {
                    if (component.IsSleeping)
                    {
                        player.OnHandClickBed(packet.UniqueId, component.CurrentSide.Side);
                    }
                }
                else
                {
                    if (component.IsSleeping)
                    {
                        player.LieDownStartCinematicBed(packet.UniqueId, component.CurrentSide.Side);
                    }
                    else
                    {
                        player.StandupStartCinematicBed(packet.UniqueId, component.CurrentSide.Side);
                    }
                }
            }

            return true;
        }

        /**
         *
         * Oyuncu ekranı tamamen karardığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSleepScreenStartingCompleted()
        {
            SleepScreen.Instance.Initialize();
            SleepScreen.Instance.Enable();
        }

        /**
         *
         * Oyuncu ekranı aydınlanma başlatıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSleepScreenStopingStarted()
        {
            SleepScreen.Instance.Disable();
        }

        /**
         *
         * Yatağı kullanabilirlik durumunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBedIsCanSleepChecking(BedIsCanSleepCheckingEventArgs ev)
        {
            if (Network.HandTarget.IsBlocked(ev.UniqueId))
            {
                ev.IsAllowed = false;

                Interact.ShowUseDenyMessage();
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
            if (!ev.IsSeaTruckModule)
            {
                ev.IsAllowed = false;

                if (!Network.HandTarget.IsBlocked(ev.UniqueId))
                {
                    BedProcessor.SendPacketToServer(ev.UniqueId, side: ev.Side, isSleeping: true);
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
            if (!ev.IsSeaTruckModule)
            {
                BedProcessor.SendPacketToServer(ev.UniqueId, isSleeping: false);
            }
        }

        /**
         *
         * Sunucuya paketi gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId, global::Bed.BedSide side = global::Bed.BedSide.None, bool isSleeping = false)
        {
            ServerModel.MetadataComponentArgs result = new ServerModel.MetadataComponentArgs()
            {
                UniqueId  = uniqueId,
                Component = new Metadata.Bed()
                {
                    IsSleeping  = isSleeping,
                    CurrentSide = new Metadata.BedSideItem(0, side),
                }
            };

            NetworkClient.SendPacket(result);
        }
    }
}