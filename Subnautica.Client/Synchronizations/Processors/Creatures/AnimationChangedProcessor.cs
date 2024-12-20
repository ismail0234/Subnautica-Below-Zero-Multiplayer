namespace Subnautica.Client.Synchronizations.Processors.Creatures
{
    using Subnautica.API.Features;
    using Subnautica.API.Features.Creatures;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using System.Collections.Generic;

    using System.Linq;

    using ServerModel = Subnautica.Network.Models.Server;

    public class AnimationChangedProcessor : NormalProcessor
    {
        /**
         *
         * Animasyon isteklerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static ServerModel.CreatureAnimationArgs AnimationRequest = new ServerModel.CreatureAnimationArgs();

        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.CreatureAnimationArgs>();
            if (packet == null)
            {
                return false;
            }

            foreach (var item in packet.Animations)
            {
                var action = new CreatureQueueAction();
                action.OnProcessCompleted = this.OnCreatureProcessCompleted;
                action.RegisterProperty("Animations", item.Animations);

                Network.Creatures.ProcessToQueue(item.CreatureId, action);
            }

            return true;
        }

        /**
         *
         * İşlem tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void OnCreatureProcessCompleted(MultiplayerCreature creature, CreatureQueueItem item)
        {
            foreach (var animation in item.Action.GetProperty<Dictionary<byte, byte>>("Animations"))
            {
                if (creature.Creature)
                {
                    var tracker = creature.CreatureItem.Data.GetAnimationTrackerById(animation.Key);
                    if (tracker != null) 
                    {
                        tracker.OnTrackerExecuting(creature.Creature, animation.Value);
                    }
                }
            }
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
            AnimationRequest.Animations.Clear();
        }

        /**
         *
         * Her kare sonunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnLateUpdate()
        {
            if (AnimationRequest.Animations.Count > 0)
            {
                NetworkClient.SendPacket(AnimationRequest);

                AnimationRequest.Animations.Clear();
            }
        }

        /**
         *
         * Balık animasyonu değiştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnCreatureAnimationChanged(CreatureAnimationChangedEventArgs ev)
        {
            var item = AnimationRequest.Animations.Where(q => q.CreatureId == ev.CreatureId).FirstOrDefault();
            if (item != null)
            {
                item.CreatureId = ev.CreatureId;
                item.Animations.Add(ev.AnimationId, ev.Result);
            }
            else
            {
                item = new ServerModel.CreatureAnimationItem();
                item.CreatureId = ev.CreatureId;
                item.Animations.Add(ev.AnimationId, ev.Result);

                AnimationRequest.Animations.Add(item);
            }
        }
    }
}
