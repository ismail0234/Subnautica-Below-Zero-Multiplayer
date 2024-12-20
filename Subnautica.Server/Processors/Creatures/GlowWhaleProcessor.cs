namespace Subnautica.Server.Processors.Creatures
{
    using Subnautica.API.Enums.Creatures;
    using Subnautica.Events.Handlers;
    using Subnautica.Network.Models.Creatures;
    using Subnautica.Network.Models.Server;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;
    using Subnautica.Server.Logic;

    using CreatureModel = Subnautica.Network.Models.Creatures;

    public class GlowWhaleProcessor : WorldCreatureProcessor
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
            var component = packet.Component.GetComponent<CreatureModel.GlowWhale>();
            if (component == null)
            {
                return false;
            }

            if (component.SFXType != GlowWhaleSFXType.None)
            {
                profile.SendPacketToOtherClients(packet);
            }
            else if (component.IsEyeInteract || component.IsRideStart)
            {
                if (creature.IsBusy() || creature.LiveMixin.IsDead || Server.Instance.Logices.Interact.IsBlocked(creatureId))
                {
                    return false;
                }

                if (Server.Instance.Logices.Interact.AddBlock(profile.UniqueId, creatureId, true))
                {
                    creature.SetAction(packet, profile.PlayerId);

                    if (component.IsEyeInteract)
                    {
                        Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId, Interact.CreatureGlowWhaleEyeInteract);
                        Server.Instance.Logices.Timing.AddQueue(creatureId, () => { creature.ClearAction(true); }, Interact.CreatureGlowWhaleEyeInteract);
                    }

                    if (Server.Instance.Logices.CreatureWatcher.ImmediatelyTrigger())
                    {
                        Server.Instance.Logices.CreatureWatcher.TriggerAction(packet);
                    }
                }
            }
            else if (component.IsRideEnd)
            {
                if (creature.BusyOwnerId == profile.PlayerId)
                {
                    creature.ClearAction(true);

                    Server.Instance.Logices.Interact.RemoveBlockByPlayerId(profile.UniqueId);

                    if (Server.Instance.Logices.CreatureWatcher.ImmediatelyTrigger())
                    {
                        Server.Instance.Logices.CreatureWatcher.TriggerAction(packet);
                    }
                }
            }

            return true;
        }
    }
}
