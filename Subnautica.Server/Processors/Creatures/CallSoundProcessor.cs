namespace Subnautica.Server.Processors.Creatures
{
    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class CallSoundProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.CreatureCallArgs>();
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

            Server.Instance.Logices.CreatureWatcher.OnCallSoundTriggered(creature, packet);
            return true;
        }
    }
}
