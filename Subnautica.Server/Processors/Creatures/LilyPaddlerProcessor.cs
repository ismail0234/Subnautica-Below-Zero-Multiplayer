namespace Subnautica.Server.Processors.Creatures
{
    using Subnautica.Network.Models.Creatures;
    using Subnautica.Network.Models.Server;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;
    using Subnautica.Server.Logic;

    using CreatureModel = Subnautica.Network.Models.Creatures;

    public class LilyPaddlerProcessor : WorldCreatureProcessor
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
            if (creature.IsBusy() || creature.LiveMixin.IsDead)
            {
                return false;
            }

            var component = packet.Component.GetComponent<CreatureModel.LilyPaddler>();
            if (component == null)
            {
                return false;
            }

            var targetProfile = Server.Instance.GetPlayer(component.TargetId);
            if (targetProfile == null || !targetProfile.IsFullConnected || targetProfile.IsHypnotized() || targetProfile.Health <= 0f)
            {
                return false;
            }

            targetProfile.SetLastHypnotizeTime(Server.Instance.Logices.World.GetServerTime());

            component.LastHypnotizeTime = targetProfile.LastHypnotizeTime;

            creature.SetAction(packet, targetProfile.PlayerId);

            if (Server.Instance.Logices.CreatureWatcher.ImmediatelyTrigger())
            {
                Server.Instance.Logices.CreatureWatcher.TriggerAction(packet);
                Server.Instance.Logices.CreatureWatcher.ClearAction(creature, Interact.CreatureLilyPaddlerHypnotize);
            }

            return true;
        }
    }
}
