namespace Subnautica.Server.Processors.Items
{
    using Subnautica.Network.Models.Server;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using ItemModel = Subnautica.Network.Models.Items;

    public class MapRoomCameraProcessor : PlayerItemProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(AuthorizationProfile profile, PlayerItemActionArgs packet)
        {
            var component = packet.Item.GetComponent<ItemModel.DroneCamera>();
            if (component == null)
            {
                return false;
            }

            if (Server.Instance.Logices.Storage.TryPickupToWorld(component.PickupItem, profile.InventoryItems, out var entity))
            {
                entity.SetPositionAndRotation(component.Position, component.Rotation);
                entity.SetOwnership(profile.UniqueId);
                entity.SetDeployed(true);
                entity.SetComponent(component.Component);

                component.Entity = entity;

                profile.SendPacketToAllClient(packet);
            }

            return true;
        }
    }
}
