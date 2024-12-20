namespace Subnautica.Client.Synchronizations.Processors.Creatures
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Creatures;
    using Subnautica.API.Features.Creatures.MonoBehaviours.Shared;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Structures;

    using UnityEngine;

    using ServerModel = Subnautica.Network.Models.Server;

    public class LeviathanMeleeAttackProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.CreatureLeviathanMeleeAttackArgs>();
            if (packet == null)
            {
                return false;
            }

            var action = new CreatureQueueAction();
            action.OnProcessCompleted = this.OnCreatureProcessCompleted;
            action.RegisterProperty("Target"     , packet.Target);
            action.RegisterProperty("ProcessTime", packet.ProcessTime);

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

            if (creature.GameObject.TryGetComponent<MultiplayerLeviathanMeleeAttack>(out var meleeAttack))
            {
                if (creature.CreatureItem.IsMine())
                {
                    meleeAttack.StartMeleeAttack(target);
                }
                else
                {
                    if (processTime + 3 >= Network.Session.GetWorldTime())
                    {
                        if (target.IsPlayer())
                        {
                            if (target.IsDead)
                            {
                                var player = ZeroPlayer.GetPlayerById(target.TargetId);
                                if (player != null)
                                {
                                    player.StartLeviathanMeleeAttackCinematic(creature.CreatureItem.Id.ToCreatureStringId());
                                }
                            }
                            else
                            {
                                meleeAttack.SimulateMeleeAttack(target);
                            }
                        }
                        else if (target.IsVehicle())
                        {
                            meleeAttack.SimulateMeleeAttack(target);
                        }
                    }
                }
            }
        }

        /**
         *
         * Yaratık bir nesne ile temasa geçtiğinde (saldırdığında) tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnLeviathanMeleeAttacking(CreatureLeviathanMeleeAttackingEventArgs ev)
        {
            ev.IsAllowed = false;
            ev.Instance.timeLastBite = Time.time;
            
            if (Network.Creatures.IsMine(ev.UniqueId))
            {
                LeviathanMeleeAttackProcessor.SendPacketToServer(ev.UniqueId.ToCreatureId(), ev.TargetId, ev.TargetType, ev.BiteDamage);
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
            ServerModel.CreatureLeviathanMeleeAttackArgs request = new ServerModel.CreatureLeviathanMeleeAttackArgs()
            {
                CreatureId = creatureId,
                BiteDamage = biteDamage,
                Target     = new ZeroLastTarget(targetId, techType)
            };

            NetworkClient.SendPacket(request);
        }
    }
}
