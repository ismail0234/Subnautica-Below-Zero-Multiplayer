namespace Subnautica.Network.Models.Items
{
    using MessagePack;

    using Subnautica.Network.Core.Components;

    [MessagePackObject]
    public class Scanner : NetworkPlayerItemComponent
    {
        /**
         *
         * TechType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public override TechType TechType { get; set; } = TechType.Scanner;

        /**
         *
         * TargetId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public string TargetId { get; set; }
    }
}