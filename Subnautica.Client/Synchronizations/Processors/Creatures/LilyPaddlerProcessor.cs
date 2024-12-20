namespace Subnautica.Client.Synchronizations.Processors.Creatures
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Creatures;
    using Subnautica.API.Features.Creatures.MonoBehaviours;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Core.Components;

    using CreatureModel = Subnautica.Network.Models.Creatures;
    using ServerModel   = Subnautica.Network.Models.Server;

    public class LilyPaddlerProcessor : WorldCreatureProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkCreatureComponent networkPacket, byte requesterId, double processTime, TechType creatureType, ushort creatureId)
        {
            var component = networkPacket.GetComponent<CreatureModel.LilyPaddler>();
            if (component == null)
            {
                return false;
            }

            var action = new CreatureQueueAction();
            action.OnProcessCompleted = this.OnCreatureProcessCompleted;
            action.RegisterProperty("RequesterId"  , requesterId);
            action.RegisterProperty("TargetId"     , component.TargetId);
            action.RegisterProperty("HypnotizeTime", component.LastHypnotizeTime);

            Network.Creatures.ProcessToQueue(creatureId, action);
            return true;
        }

        /**
         *
         * İşlem tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCreatureProcessCompleted(MultiplayerCreature creature, CreatureQueueItem item)
        {
            var target = ZeroPlayer.GetPlayerById(item.Action.GetProperty<byte>("TargetId"));
            if (target != null && creature.GameObject.TryGetComponent<LilyPaddlerMonoBehaviour>(out var lilyPaddler))
            {
                lilyPaddler.StartHypnotize(target, item.Action.GetProperty<float>("HypnotizeTime"));
            }
        }

        /**
         *
         * Hipnoz başlarken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnLilyPaddlerHypnotizeStarting(LilyPaddlerHypnotizeStartingEventArgs ev)
        {
            ev.IsAllowed = false;

            LilyPaddlerProcessor.SendPacketToServer(ev.CreatureId.ToCreatureId(), ev.TargetId);
        }

        /**
         *
         * Sunucuya veri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void SendPacketToServer(ushort creatureId, byte targetId)
        {
            ServerModel.CreatureProcessArgs request = new ServerModel.CreatureProcessArgs()
            {
                CreatureId = creatureId,
                Component = new CreatureModel.LilyPaddler(targetId)
            };

            NetworkClient.SendPacket(request);
        }
    }
}
