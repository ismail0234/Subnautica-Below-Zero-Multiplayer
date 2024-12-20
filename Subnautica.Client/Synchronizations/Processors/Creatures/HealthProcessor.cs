namespace Subnautica.Client.Synchronizations.Processors.Creatures
{
    using Discord;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

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
        public override bool OnDataReceived(NetworkPacket networkPacket)
        { 
            var packet = networkPacket.GetPacket<ServerModel.CreatureHealthArgs>();
            if (packet == null)
            {
                return false;
            }

            var creatureLiveMixin = Network.Identifier.GetComponentByGameObject<global::LiveMixin>(packet.CreatureId.ToCreatureStringId(), true);
            if (creatureLiveMixin)
            {
                ZeroLiveMixin.SendDamageInfoNotify(creatureLiveMixin, packet.Damage, packet.DamageType);
            }
            
            return true;
        }

        /**
         *
         * Bir nesne hasar aldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnTakeDamaging(TakeDamagingEventArgs ev)
        {
            if (ev.TechType.IsCreature() && ev.TechType.IsSynchronizedCreature())
            {
                ev.IsAllowed = false;

                if (ev.Damage > 0f && ev.TechType.IsCanBeAttacked())
                {
                    Log.Info("Damage: " + ev.Damage + ", Type: " + ev.DamageType + ", ID: " + ev.UniqueId);
                    
                    ServerModel.CreatureHealthArgs request = new ServerModel.CreatureHealthArgs()
                    {
                        CreatureId = ev.UniqueId.ToCreatureId(),
                        DamageType = ev.DamageType,
                        Damage     = ev.Damage,
                    };

                    NetworkClient.SendPacket(request);
                }
            }
        }
    }
}