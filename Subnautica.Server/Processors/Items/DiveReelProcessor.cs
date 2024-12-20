namespace Subnautica.Server.Processors.Items
{
    using Subnautica.Network.Models.Server;
    using Subnautica.Server.Abstracts.Processors;
    using Subnautica.Server.Core;

    using WorldEntityModel = Subnautica.Network.Models.WorldEntity.DynamicEntityComponents;
    using ItemModel        = Subnautica.Network.Models.Items;

    public class DiveReelProcessor : PlayerItemProcessor
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
            // Bu aracı kullanan var mı?
            return true;
        }
    }
}
