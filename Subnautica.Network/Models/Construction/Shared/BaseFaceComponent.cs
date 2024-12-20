namespace Subnautica.Network.Models.Construction.Shared
{
    using MessagePack;

    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class BaseFaceComponent
    {
        /**
         *
         * Cell değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public ZeroInt3 Cell { get; set; }

        /**
         *
         * Direction değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public Base.Direction Direction { get; set; }
    }
}
