namespace Subnautica.Client.Synchronizations.Processors.World
{
    using Subnautica.API.Features;
    using Subnautica.API.Extensions;
    using Subnautica.Client.Core;
    using Subnautica.Client.Abstracts;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;
    using EntityModel = Subnautica.Network.Models.WorldEntity;

    public class EntityScannerProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.EntityScannerCompletedArgs>();

            if (packet.Entity.UniqueId.IsWorldStreamer())
            {
                Network.WorldStreamer.DisableSlot(packet.Entity.UniqueId.WorldStreamerToSlotId(), -1);
            }
            else
            {
                Network.StaticEntity.AddStaticEntity(packet.Entity);

                var player = ZeroPlayer.GetPlayerByUniqueId(packet.ScannerPlayerUniqueId);
                if (player != null && player.IsMine)
                {
                    return false;
                }

                var gameObject = Network.Identifier.GetGameObject(packet.Entity.UniqueId, true);
                if (gameObject == null)
                {
                    return false;
                }

                World.DestroyGameObject(gameObject);
            }

            return true;
        }

        /**
         *
         * Oyuncu nesne taraması tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEntityScannerCompleted(EntityScannerCompletedEventArgs ev)
        {
            if (ev.TechType.IsFragment() && ev.TechType.IsDestroyAfterScan() && ev.UniqueId.IsNotNull())
            {
                ServerModel.EntityScannerCompletedArgs request = new ServerModel.EntityScannerCompletedArgs()
                {
                    Entity = new EntityModel.RestrictedEntity()
                    {
                        UniqueId = ev.UniqueId,
                    }
                };

                NetworkClient.SendPacket(request);
            }
        }
    }
}