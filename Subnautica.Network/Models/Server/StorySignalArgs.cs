namespace Subnautica.Network.Models.Server
{
    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;
    using Subnautica.Network.Models.Storage.Story.StoryGoals;
    using Subnautica.Network.Models.Storage.World.Childrens;

    [MessagePackObject]
    public class StorySignalArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.StorySignal;

        /**
         *
         * SignalType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public ZeroStorySignal Signal { get; set; }

        /**
         *
         * Beacon değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public WorldDynamicEntity Beacon { get; set; }
    }
}