namespace Subnautica.Server.Processors.Items
{
    using Subnautica.Network.Models.Server;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using ItemModel = Subnautica.Network.Models.Items;

    internal class TeleportationToolProcessor : PlayerItemProcessor
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
            var component = packet.Item.GetComponent<ItemModel.TeleportationTool>();

            if (string.IsNullOrEmpty(component.TeleporterId))
            {
                profile.SendPacketToOtherClients(packet);
            }
            else
            {
                var entity = Server.Instance.Storages.World.GetDynamicEntity(component.TeleporterId);
                if (entity == null)
                {
                    return false;
                }

                profile.SendPacketToOtherClients(packet);
            }

            return true;
        }
    }
}
