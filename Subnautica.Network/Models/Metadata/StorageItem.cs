namespace Subnautica.Network.Models.Metadata
{
    using MessagePack;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Core.Components;

    [MessagePackObject]
    public class StorageItem : MetadataComponent
    {
        /**
         *
         * ItemId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public string ItemId { get; set; }

        /**
         *
         * Item değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public byte[] Item { get; set; }

        /**
         *
         * TechType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public TechType TechType { get; set; }

        /**
         *
         * Size değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public byte Size { get; set; }

        /**
         *
         * Teknoloji türünü değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetItem(TechType techType)
        {
            this.Item = null;
            this.Size = GetItemSize(techType);
            this.TechType = techType;
        }

        /**
         *
         * Size X değerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public byte GetSizeX()
        {
            return (byte)(this.Size % 10);
        }

        /**
         *
         * Size Y değerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public byte GetSizeY()
        {
            return (byte)(this.Size / 10);
        }

        /**
         *
         * StorageItem oluşturur. (Client Side)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static StorageItem Create(Pickupable pickupable, bool resetItem = false)
        {
            return new StorageItem()
            { 
                ItemId    = pickupable.gameObject.GetIdentityId(), 
                Item      = resetItem ? null : Serializer.SerializeGameObject(pickupable),
                TechType  = pickupable.GetTechType(),
                Size      = GetItemSize(pickupable.GetTechType()),
            };
        }

        /**
         *
         * StorageItem oluşturur. (Server Side)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static StorageItem Create(string itemId, TechType techType)
        {
            return new StorageItem()
            { 
                ItemId   = itemId, 
                TechType = techType,
                Size     = GetItemSize(techType),
            };
        }

        /**
         *
         * StorageItem oluşturur. (Server Side)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static StorageItem Create(TechType techType)
        {
            return StorageItem.Create(Network.Identifier.GenerateUniqueId(), techType);
        }

        /**
         *
         * Eşya boyutunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static byte GetItemSize(TechType techType)
        {
            var itemSize = TechData.GetItemSize(techType);

            return (byte)(itemSize.x + (itemSize.y * 10));
        }
    }
}