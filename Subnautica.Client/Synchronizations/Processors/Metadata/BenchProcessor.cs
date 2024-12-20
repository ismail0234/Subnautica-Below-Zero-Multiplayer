namespace Subnautica.Client.Synchronizations.Processors.Metadata
{
    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Client.MonoBehaviours.General;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Server;

    using Metadata    = Subnautica.Network.Models.Metadata;
    using ServerModel = Subnautica.Network.Models.Server;

    public class BenchProcessor : MetadataProcessor
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
            var component = packet.Component.GetComponent<Metadata.Bench>();
            if (component == null)
            {
                return false;
            }

            if (isSilence)
            {
                if (component.IsSitdown)
                {
                    packet.SetPacketOwnerId(component.PlayerId_v2);

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
                    if (component.IsSitdown)
                    {
                        player.OnHandClickBench(packet.UniqueId, component.Side);
                    }
                }
                else
                {
                    if (component.IsSitdown)
                    {
                        player.SitDownStartCinematicBench(packet.UniqueId, component.Side);
                    }
                    else
                    {
                        player.StandupStartCinematicBench(packet.UniqueId, component.Side);
                    }
                }
            }

            return true;
        }

        /**
         *
         * Oturma animasyonu başladığında tetiklenir.
         *
         * @author Ismail  <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBenchSitdown(BenchSitdownEventArgs ev)
        {
            ev.IsAllowed = false;

            if (!Network.HandTarget.IsBlocked(ev.UniqueId))
            {
                BenchProcessor.SendPacketToServer(ev.UniqueId, ev.Side, true);
            }
        }

        /**
         *
         * Kalkma animasyonu başladığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBenchStandup(BenchStandupEventArgs ev)
        {
           BenchProcessor.SendPacketToServer(ev.UniqueId, ev.Side, false);
        }

        /**
         *
         * Sunucuya paketi gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId, global::Bench.BenchSide side, bool isSitdown)
        {
            ServerModel.MetadataComponentArgs result = new ServerModel.MetadataComponentArgs()
            {
                UniqueId  = uniqueId,
                Component = new Metadata.Bench(side, isSitdown)
            };

            NetworkClient.SendPacket(result);
        }
    }
}