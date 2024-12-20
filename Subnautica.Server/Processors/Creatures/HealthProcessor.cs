namespace Subnautica.Server.Processors.Creatures
{
    using Server.Core;

    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel = Subnautica.Network.Models.Server;

    public class HealthProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.CreatureHealthArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (!Server.Instance.Logices.CreatureWatcher.TryGetCreature(packet.CreatureId, out var creature))
            {
                return false;
            }

            if (creature.Data.IsCanBeAttacked && creature.LiveMixin.TakeDamage(packet.Damage))
            {
                profile.SendPacketToAllClient(packet);

                if (creature.LiveMixin.IsDead)
                {
                    if (creature.IsBusy() && !creature.IsFrozen())
                    {
                        creature.LiveMixin.SetHealth(5f);
                    }
                    else
                    {
                        Server.Instance.Logices.CreatureWatcher.OnCreatureDead(creature);
                        Server.Instance.Logices.CreatureWatcher.ImmediatelyTrigger();
                    }
                }
            }

            return true;
        }
    }
}
