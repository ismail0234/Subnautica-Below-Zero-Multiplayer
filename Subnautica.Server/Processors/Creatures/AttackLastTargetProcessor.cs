namespace Subnautica.Server.Processors.Creatures
{
    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;
    using Subnautica.Server.Extensions;

    using ServerModel = Subnautica.Network.Models.Server;

    public class AttackLastTargetProcessor : NormalProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnExecute(AuthorizationProfile profile, NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.CreatureAttackLastTargetArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (!Server.Instance.Logices.CreatureWatcher.TryGetCreature(packet.CreatureId, out var creature))
            {
                return false;
            }

            if (creature.LiveMixin.IsDead || creature.OwnerId != profile.PlayerId)
            {
                return false;
            }

            if (packet.IsStopped)
            {
                if (creature.GetActionType() == ProcessType.CreatureAttackLastTarget)
                {
                    Server.Instance.Logices.CreatureWatcher.ClearAction(creature);
                }

                profile.SendPacketToAllClient(packet);
            }
            else
            {
                var targetPlayerId = packet.Target.GetTargetOwnerId(profile.PlayerId);
                if (targetPlayerId <= 0)
                {
                    return false;
                }

                if (packet.Target.IsPlayer())
                {
                    var player = Server.Instance.GetPlayer(targetPlayerId);
                    if (player == null || !player.IsFullConnected || player.IsUnderAttack())
                    {
                        return false;
                    }
                }

                creature.SetAction(packet, targetPlayerId);

                if (Server.Instance.Logices.CreatureWatcher.ImmediatelyTrigger())
                {
                    Server.Instance.Logices.CreatureWatcher.TriggerAction(packet);
                    Server.Instance.Logices.CreatureWatcher.ClearAction(creature, 5f);
                }
            }

            return true;
        }
    }
}
