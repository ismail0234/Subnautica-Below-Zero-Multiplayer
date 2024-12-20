namespace Subnautica.Server.Processors.Creatures
{
    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;
    using Subnautica.Server.Extensions;

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
        public override bool OnExecute(AuthorizationProfile profile, NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.CreatureMeleeAttackArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (!Server.Instance.Logices.CreatureWatcher.TryGetCreature(packet.CreatureId, out var creature))
            {
                return false;
            }

            if (creature.LiveMixin.IsDead)
            {
                return false;
            }

            var targetPlayerId = packet.Target.GetTargetOwnerId(profile.PlayerId);
            if (targetPlayerId <= 0)
            {
                return false;
            }

            var targetPlayer = Server.Instance.GetPlayer(targetPlayerId);
            if (targetPlayer == null || !targetPlayer.IsFullConnected)
            {
                return false;
            }

            Server.Instance.Logices.CreatureWatcher.TriggerAction(packet);
            return true;
        }
    }
}
