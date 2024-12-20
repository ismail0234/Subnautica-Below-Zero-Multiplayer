namespace Subnautica.Client.Synchronizations.Processors.Creatures
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Creatures;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Core.Components;

    using CreatureModel = Subnautica.Network.Models.Creatures;
    using ServerModel   = Subnautica.Network.Models.Server;

    public class CrashFishProcessor : WorldCreatureProcessor
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
            var component = networkPacket.GetComponent<CreatureModel.CrashFish>();
            if (component == null)
            {
                return false;
            }

            var action = new CreatureQueueAction();
            action.OnProcessCompleted = this.OnCreatureProcessCompleted;
            action.RegisterProperty("RequesterId", requesterId);

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
        private void OnCreatureProcessCompleted(MultiplayerCreature creature, CreatureQueueItem item)
        {
            if (creature.GameObject.TryGetComponent<global::Crash>(out var crash))
            {
                if (ZeroPlayer.IsPlayerMine(item.Action.GetProperty<byte>("RequesterId")))
                {
                    crash.inflateSound.Play();
                }
                else
                {
                    crash.OnState(Crash.State.Attacking, true);
                    crash.GetAnimator().SetFloat("aggressive", 1f);
                    crash.GetAnimator().SetBool(AnimatorHashID.attacking, true);

                    crash.attackSound.Play();
                    crash.inflateSound.Play();
                }
            }
        }

        /**
         *
         * CrashFish patlarken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnCrashFishInflating(CrashFishInflatingEventArgs ev)
        {
            ev.IsAllowed = false;

            CrashFishProcessor.SendPacketToServer(ev.UniqueId.ToCreatureId());
        }

        /**
         *
         * Sunucuya veri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void SendPacketToServer(ushort creatureId)
        {
            ServerModel.CreatureProcessArgs request = new ServerModel.CreatureProcessArgs()
            {
                CreatureId = creatureId,
                Component = new CreatureModel.CrashFish()
            };

            NetworkClient.SendPacket(request);
        }
    }
}
