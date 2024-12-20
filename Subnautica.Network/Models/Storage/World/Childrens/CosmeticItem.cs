namespace Subnautica.Network.Models.Storage.World.Childrens
{
    using MessagePack;

    using Subnautica.Network.Models.Metadata;
    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class CosmeticItem
    {
        /**
         *
         * StorageItem değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public StorageItem StorageItem { get; set; }

        /**
         *
         * BaseId değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public string BaseId { get; set; }

        /**
         *
         * Mevcut değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public ZeroVector3 Position { get; set; }

        /**
         *
         * Rotation değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public ZeroQuaternion Rotation { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public CosmeticItem()
        {

        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public CosmeticItem(StorageItem storageItem, string baseId, ZeroVector3 position, ZeroQuaternion rotation)
        {
            this.StorageItem = storageItem;
            this.BaseId      = baseId;
            this.Position    = position;
            this.Rotation    = rotation;
        }
    }
}