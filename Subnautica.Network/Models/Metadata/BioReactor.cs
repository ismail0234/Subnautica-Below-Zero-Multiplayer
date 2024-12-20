namespace Subnautica.Network.Models.Metadata
{
    using MessagePack;

    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;

    [MessagePackObject]
    public class BioReactor : MetadataComponent
    {
        /**
         *
         * IsAdded Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public bool IsAdded { get; set; }

        /**
         *
         * WorldPickupItem Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public WorldPickupItem WorldPickupItem { get; set; }

        /**
         *
         * Item Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public StorageContainer StorageContainer { get; set; } = Metadata.StorageContainer.Create(4, 4);
    }
}
