namespace Subnautica.Network.Models.Items
{
    using MessagePack;

    using Subnautica.Network.Core.Components;

    [MessagePackObject]
    public class Seaglide : NetworkPlayerItemComponent
    {
        /**
         *
         * IsActivated değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public bool IsActivated { get; set; }

        /**
         *
         * IsLightsActivated değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public bool IsLightsActivated { get; set; }
    }
}