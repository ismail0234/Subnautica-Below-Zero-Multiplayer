namespace Subnautica.Network.Models.Metadata
{
    using MessagePack;

    using Subnautica.Network.Core.Components;

    [MessagePackObject]
    public class Jukebox : MetadataComponent
    {
        /**
         *
         * CurrentPlayingTrack Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public string CurrentPlayingTrack { get; set; }

        /**
         *
         * IsPaused değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public bool IsPaused { get; set; }

        /**
         *
         * IsStoped değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public bool IsStoped { get; set; }

        /**
         *
         * IsNext değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public bool IsNext { get; set; }

        /**
         *
         * IsPrevious değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public bool IsPrevious { get; set; }

        /**
         *
         * RepeatMode değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public global::Jukebox.Repeat RepeatMode { get; set; }

        /**
         *
         * Shuffle değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public bool IsShuffled { get; set; }

        /**
         *
         * Position değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public float Position { get; set; }

        /**
         *
         * Length değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public uint Length { get; set; }

        /**
         *
         * Volume değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(9)]
        public float Volume { get; set; }
    }
}
