namespace Subnautica.Server.Processors.Creatures
{
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Creatures;
    using Subnautica.Network.Models.Server;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using CreatureModel = Subnautica.Network.Models.Creatures;

    public class CrashFishProcessor : WorldCreatureProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(AuthorizationProfile profile, CreatureProcessArgs packet, MultiplayerCreatureItem creature, string creatureId)
        {
            var component = packet.Component.GetComponent<CreatureModel.CrashFish>();
            if (component == null)
            {
                return false;
            }

            if (creature.IsBusy() || creature.LiveMixin.IsDead) 
            {
                return false;
            }

            creature.SetBusyOwnerId(profile.PlayerId);

            if (Server.Instance.Logices.CreatureWatcher.ImmediatelyTrigger())
            {
                profile.SendPacketToAllClient(packet);

                Server.Instance.Logices.Timing.AddQueue(() =>
                {
                    if (creature != null && creature.LiveMixin.TakeDamage(9999f))
                    {
                        if (creature.LiveMixin.IsDead)
                        {
                            Server.Instance.Logices.CreatureWatcher.ImmediatelyTrigger();
                        }
                    }
                }, 2f);
            }

            return true;
        }
    }
}
