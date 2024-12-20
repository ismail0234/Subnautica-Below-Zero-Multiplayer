namespace Subnautica.Client.Synchronizations.Processors.Creatures
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Creatures;
    using Subnautica.API.Features.Creatures.MonoBehaviours.Shared;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Structures;

    using UnityEngine;

    using ServerModel = Subnautica.Network.Models.Server;

    public class MeleeAttackProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.CreatureMeleeAttackArgs>();
            if (packet == null)
            {
                return false;
            }

           
            var action = new CreatureQueueAction();
            action.OnProcessCompleted = this.OnCreatureProcessCompleted;
            action.RegisterProperty("Target", packet.Target);

            Network.Creatures.ProcessToQueue(packet.CreatureId, action);
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
            var target      = item.Action.GetProperty<ZeroLastTarget>("Target");
            var processTime = item.Action.GetProperty<double>("ProcessTime");

            if (creature.GameObject.TryGetComponent<MultiplayerMeleeAttack>(out var meleeAttack))
            {
                meleeAttack.StartMeleeAttack(target.GetGameObject(true));
            }
        }

        /**
         *
         * Yaratık bir nesne ile temasa geçtiğinde (saldırdığında) tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnMeleeAttacking(CreatureMeleeAttackingEventArgs ev)
        {
            ev.IsAllowed = false;
            ev.Instance.timeLastBite = Time.time;

            if (Network.Creatures.IsMine(ev.UniqueId))
            {
                if (ev.TargetType.IsCreature())
                {
                    if (ev.TargetType.IsSynchronizedCreature())
                    {
                        MeleeAttackProcessor.SendPacketToServer(ev.UniqueId.ToCreatureId(), ev.TargetId, ev.TargetType, ev.BiteDamage);
                    }
                }
                else
                {
                    MeleeAttackProcessor.SendPacketToServer(ev.UniqueId.ToCreatureId(), ev.TargetId, ev.TargetType, ev.BiteDamage);
                }
            }
        }

        /**
         *
         * Sunucuya veri gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void SendPacketToServer(ushort creatureId, string targetId, TechType techType, float biteDamage)
        {
            ServerModel.CreatureMeleeAttackArgs request = new ServerModel.CreatureMeleeAttackArgs()
            {
                CreatureId = creatureId,
                BiteDamage = biteDamage,
                Target     = new ZeroLastTarget(targetId, techType)
            };

            NetworkClient.SendPacket(request);
        }
    }
}
