namespace Subnautica.Client.Synchronizations.Processors.Building
{
    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using Constructing = Subnautica.Client.Multiplayer.Constructing;
    using ServerModel  = Subnautica.Network.Models.Server;

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
            var packet = networkPacket.GetPacket<ServerModel.ConstructionHealthArgs>();
            if (string.IsNullOrEmpty(packet.UniqueId))
            {
                return false;
            }

            using (EventBlocker.Create(ProcessType.ConstructingRemoved))
            {
                if (!Constructing.Builder.Destroy(packet.UniqueId))
                {
                    var construction = Network.Identifier.GetGameObject(packet.UniqueId, true);
                    if (construction)
                    {
                        World.DestroyGameObject(construction.gameObject);
                    }
                }
            }

            if (ZeroPlayer.IsPlayerMine(packet.GetPacketOwnerId()))
            {
                ConstructionSyncedProcessor.UpdateConstructionSync();
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
            if (ev.TechType == TechType.Spotlight || ev.TechType == TechType.SolarPanel)
            {
                ev.IsAllowed = false;

                if (ev.Damage > 0)
                {
                    HealthProcessor.SendPacketToServer(ev.UniqueId, ev.Damage, ev.MaxHealth);
                }
            }
        }

        /**
         *
         * Sunucuya paketi gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SendPacketToServer(string uniqueId, float damage, float maxHealth)
        {
            ServerModel.ConstructionHealthArgs request = new ServerModel.ConstructionHealthArgs()
            {
                UniqueId  = uniqueId,
                MaxHealth = maxHealth,
                Damage    = damage,
            };

            NetworkClient.SendPacket(request);
        }
    }
}