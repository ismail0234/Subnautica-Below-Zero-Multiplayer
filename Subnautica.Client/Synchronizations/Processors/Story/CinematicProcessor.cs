namespace Subnautica.Client.Synchronizations.Processors.Story
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class CinematicProcessor : NormalProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private List<ServerModel.StoryCinematicTriggerArgs> CinematicQueue = new List<ServerModel.StoryCinematicTriggerArgs>();

        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.StoryCinematicTriggerArgs>();

            Log.Info("CINEMATIC DATA RECEIVED => " + packet.CinematicType + ", count: " + PlayerCinematicController.cinematicModeCount + ", T1: " + Network.Session.GetWorldTime() + ", T2: " + packet.StartTime);

            this.CinematicQueue.Add(packet);
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
            if (this.CinematicQueue.Count > 0)
            {
                if (PlayerCinematicController.cinematicModeCount <= 0)
                {
                    foreach (var packet in this.CinematicQueue.ToList())
                    {
                        Log.Error("Cinematic -> T1: " + Network.Session.GetWorldTime() + ", T2: " + packet.StartTime);
                        if (Network.Session.GetWorldTime() < packet.StartTime)
                        {
                            continue;
                        }

                        this.CinematicQueue.Remove(packet);
                        
                        Network.Story.StartCinematicMode(packet.UniqueId);
                    }
                }
                else
                {
                    Log.Error("Cinematic -> C1: " + PlayerCinematicController.cinematicModeCount + ", C2: " + this.CinematicQueue.Count);
                }
            }
        }

        /**
         *
         * Hikaye cinematic tetiklenirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnCinematicTriggering(CinematicTriggeringEventArgs ev)
        {
            ev.IsAllowed = false;

            ServerModel.StoryCinematicTriggerArgs result = new ServerModel.StoryCinematicTriggerArgs()
            {
                UniqueId      = ev.UniqueId,
                CinematicType = ev.StoryCinematicType,
                IsTypeClick   = ev.IsClicked,
            };

            NetworkClient.SendPacket(result);
        }
    }
}