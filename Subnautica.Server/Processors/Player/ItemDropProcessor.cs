namespace Subnautica.Server.Processors.Player
{
    using Server.Core;

    using Subnautica.API.Extensions;
    using Subnautica.Network.Core.Components;
    
    using Subnautica.Network.Models.Core;
    using Subnautica.Server.Abstracts.Processors;

    using ServerModel      = Subnautica.Network.Models.Server;
    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;

    public class ItemDropProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.ItemDropArgs>();
            if (packet == null)
            {
                return this.SendEmptyPacketErrorLog(networkPacket);
            }

            if (packet.WorldPickupItem.Item.TechType.IsCreatureEgg())
            {
                packet.WorldPickupItem.Item.SetItem(packet.WorldPickupItem.Item.TechType.ToCreatureEgg());
            }

            if (Server.Instance.Logices.Storage.TryPickupToWorld(packet.WorldPickupItem, profile.InventoryItems, out var entity))
            {
                entity.SetPositionAndRotation(packet.Entity.Position, packet.Entity.Rotation);
                entity.SetOwnership(profile.UniqueId);

                packet.Entity = entity;
                
                profile.SendPacketToAllClient(packet);
            }

            return true;
        }

        /**
         *
         * Bileşeni döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static NetworkDynamicEntityComponent GetEntityComponent(TechType techType)
        {
            switch (techType)
            {
                case TechType.SmallStorage : return new WorldEntityModel.SmallStorage();
                case TechType.QuantumLocker: return new WorldEntityModel.QuantumLocker();
                case TechType.Beacon:        return new WorldEntityModel.Beacon();
                case TechType.SpyPenguin:    return new WorldEntityModel.SpyPenguin();
            }

            return null;
        }
    }
}
