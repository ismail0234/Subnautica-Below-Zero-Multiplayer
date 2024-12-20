namespace Subnautica.Server.Processors.Creatures
{
    using Subnautica.API.Extensions;
    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class FreezeProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.CreatureFreezeArgs>();
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

            packet.UpdateEndTime(Server.Instance.Logices.World.GetServerTime());

            creature.SetAction(packet, profile.PlayerId);

            if (Server.Instance.Logices.CreatureWatcher.ImmediatelyTrigger())
            {
                Server.Instance.Logices.CreatureWatcher.TriggerAction(packet);
                Server.Instance.Logices.CreatureWatcher.ClearAction(creature, packet.LifeTime - 1f);
            }

            return true;
        }
    }
}
