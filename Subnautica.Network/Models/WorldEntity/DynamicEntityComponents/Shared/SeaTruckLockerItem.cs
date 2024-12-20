namespace Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared
{
    using MessagePack;

    [MessagePackObject]
    public class SeaTruckLockerItem
    {
        /**
         *
         * UniqueId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public string UniqueId { get; set; }

        /**
         *
         * StorageContainer Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public Metadata.StorageContainer StorageContainer { get; set; }
    }
}
