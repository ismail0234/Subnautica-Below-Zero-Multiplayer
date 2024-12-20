namespace Subnautica.Network.Models.Metadata
{
    using MessagePack;

    using Subnautica.API.Features;
    using Subnautica.Network.Core.Components;

    [MessagePackObject]
    public class JukeboxUsed : MetadataComponent
    {
        /**
         *
         * Data Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public CustomProperty Data { get; set; }
    }
}
