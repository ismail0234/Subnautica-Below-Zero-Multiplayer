namespace Subnautica.Client.Synchronizations.Processors.Story
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ClientModel = Subnautica.Network.Models.Client;
    using ServerModel = Subnautica.Network.Models.Server;

    public class PlayerVisibilityProcessor : NormalProcessor
    {
        /**
         *
         * Aktif sinematik
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static string CurrentCinematic { get; set; } = null;

        /**
         *
         * Görünürlük kuyruğu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Queue<string> VisibilityQueue { get; set; } = new Queue<string>();

        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ClientModel.StoryPlayerVisibilityArgs>();

            foreach (var visibility in packet.Visibility)
            {
                var player = ZeroPlayer.GetPlayerByUniqueId(visibility.Key);
                if (player != null)
                {
                    player.IsStoryCinematicModeActive = visibility.Value;
                }

                this.VisibilityQueue.Enqueue(visibility.Key);
            }

            return true;
        }

        /**
         *
         * Her Sabit karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnFixedUpdate()
        {
            if (ZeroPlayer.CurrentPlayer.IsStoryCinematicModeActive)
            {
                foreach (var player in ZeroPlayer.GetPlayers())
                {
                    player.Hide();
                }
            }
            else
            {
                while (this.VisibilityQueue.Count > 0)
                {
                    var player = ZeroPlayer.GetPlayerByUniqueId(this.VisibilityQueue.Dequeue());
                    if (player != null)
                    {
                        if (player.IsStoryCinematicModeActive)
                        {
                            player.Hide();
                        }
                        else
                        {
                            player.Show();
                        }
                    }
                }
            }
        }

        /**
         *
         * Cinematic başladığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnStoryCinematicStarted(StoryCinematicStartedEventArgs ev)
        {
            if (Network.Story.StoryCinematics.Any(q => ev.CinematicName.Contains(q)))
            {
                CurrentCinematic = ev.CinematicName;

                PlayerVisibilityProcessor.SendPacketToServer(true);
            }
        }

        /**
         *
         * Cinematic bittiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnStoryCinematicCompleted(StoryCinematicCompletedEventArgs ev)
        {
            if (CurrentCinematic.IsNotNull() && ev.CinematicName == CurrentCinematic)
            {
                CurrentCinematic = null;

                PlayerVisibilityProcessor.SendPacketToServer(false);
            }
        }

        /**
         *
         * Sunucuya veri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void SendPacketToServer(bool isCinematicActive)
        {
            ServerModel.StoryPlayerVisibilityArgs result = new ServerModel.StoryPlayerVisibilityArgs()
            {
                IsCinematicActive = isCinematicActive,
            };

            NetworkClient.SendPacket(result);
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
            CurrentCinematic = null;
        }
    }
}