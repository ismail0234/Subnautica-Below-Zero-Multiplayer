namespace Subnautica.Client.Synchronizations.Processors.Story
{
    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using UnityEngine;

    using ServerModel = Subnautica.Network.Models.Server;

    public class InteractProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.StoryInteractArgs>();
            if (packet.CinematicType == StoryCinematicType.StoryBuildAlanTerminal)
            {
                this.BuildAlanStartCinematic(packet.UniqueId, packet.GoalKey);
            }

            return true;
        }

        /**
         *
         * AL-AN Beden inşa cinematiğini başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void BuildAlanStartCinematic(string uniqueId, string goalKey)
        {
            var gameObject = Network.Identifier.GetComponentByGameObject<StoryHandTarget>(uniqueId);
            if (gameObject)
            {
                if (gameObject.informGameObject)
                {
                    gameObject.informGameObject.SendMessage("OnStoryHandTarget", SendMessageOptions.DontRequireReceiver);
                }

                if (gameObject.destroyGameObject)
                {
                    Object.Destroy(gameObject.destroyGameObject);
                }
            }

            Network.Story.GoalExecute(goalKey, global::Story.GoalType.Story, false);
        }

        /**
         *
         * Terminal tıklanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnStoryHandClicking(StoryHandClickingEventArgs ev)
        {
            ev.IsAllowed = false;

            ServerModel.StoryInteractArgs result = new ServerModel.StoryInteractArgs()
            {
                UniqueId      = ev.UniqueId,
                GoalKey       = ev.GoalKey,
                CinematicType = ev.CinematicType,
            };

            NetworkClient.SendPacket(result);
        }
    }
}