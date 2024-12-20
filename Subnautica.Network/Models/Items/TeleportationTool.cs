namespace Subnautica.Network.Models.Items
{
    using MessagePack;

    using Subnautica.Network.Core.Components;

    [MessagePackObject]
    public class TeleportationTool : NetworkPlayerItemComponent
    {
        /**
         *
         * TechType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public override TechType TechType { get; set; } = TechType.TeleportationTool;

        /**
         *
         * TeleporterId Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public string TeleporterId { get; set; }
    }
}