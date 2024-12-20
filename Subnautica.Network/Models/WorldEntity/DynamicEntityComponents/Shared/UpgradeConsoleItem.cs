namespace Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared
{
    using MessagePack;

    [MessagePackObject]
    public class UpgradeConsoleItem
    {
        /**
         *
         * ItemId Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public string ItemId { get; set; }

        /**
         *
         * ModuleType Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public TechType ModuleType { get; set; }
    }
}
