namespace Subnautica.Client.Synchronizations.Processors.General
{
    using System.Collections.Generic;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class ResourceDiscoverProcessor : NormalProcessor
    {
        /**
         *
         * Görmezden gelinecek teknolojiler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static HashSet<TechType> IgnoreTechs = new HashSet<TechType>();

        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.ResourceDiscoverArgs>();
            if (packet == null)
            {
                return true;
            }

            Network.Session.AddDiscoveredTechType(packet.TechType);

            foreach (var uniqueId in packet.MapRooms)
            {
                var mapRoom = Network.Identifier.GetComponentByGameObject<BaseDeconstructable>(uniqueId)?.GetMapRoomFunctionality();
                if (mapRoom)
                {
                    var scanner = mapRoom.GetComponentInChildren<uGUI_MapRoomScanner>();
                    if (scanner)
                    {
                        UpdateMapRoomScanner(scanner);
                    }
                }
            }

            return true;
        }

        /**
         *
         * Yeni kaynak keşfedildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBaseMapRoomResourceDiscovering(BaseMapRoomResourceDiscoveringEventArgs ev)
        {
            ev.IsAllowed = false;

            if (ev.TechType != TechType.None && !IgnoreTechs.Contains(ev.TechType))
            {
                ResourceDiscoverProcessor.SendPacketToServer(ev.TechType);
            }
        }

        /**
         *
         * Harita odası başlatıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBaseMapRoomInitialized(BaseMapRoomInitializedEventArgs ev)
        {
            UpdateMapRoomScanner(ev.MapRoom);
        }

        /**
         *
         * Teknoloji listesini günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void UpdateMapRoomScanner(uGUI_MapRoomScanner scanner)
        {
            IgnoreTechs.AddRange(Network.Session.Current.DiscoveredTechTypes);

            scanner.availableTechTypes.Clear();
            scanner.availableTechTypes.AddRange(Network.Session.Current.DiscoveredTechTypes);
            scanner.RebuildResourceList();
        }

        /**
         *
         * Sunucuya Veri Gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(TechType techType)
        {
            IgnoreTechs.Add(techType);

            ServerModel.ResourceDiscoverArgs result = new ServerModel.ResourceDiscoverArgs()
            {
                TechType = techType,
            };

            NetworkClient.SendPacket(result);
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
            IgnoreTechs.Add(TechType.HeatArea);
            IgnoreTechs.Add(TechType.Databox);
            IgnoreTechs.Add(TechType.PrecursorIonCrystal);
            IgnoreTechs.Add(TechType.KelpRootPustule);
        }

        /**
         *
         * Ana menüye dönünce tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnDispose()
        {
            IgnoreTechs.Clear();
        }
    }
}