namespace Subnautica.Network.Models.Metadata
{
    using MessagePack;

    using Subnautica.Network.Core.Components;

    [MessagePackObject]
    public class TechLight : MetadataComponent
    {
        /**
         *
         * IsSpawning değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public bool IsPowered { get; set; }
    }
}
