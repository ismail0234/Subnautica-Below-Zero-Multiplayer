namespace Subnautica.Network.Models.Storage.Story.StoryGoals
{
    using global::Story;

    using MessagePack;

    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class ZeroStorySignal
    {
        /**
         *
         * UniqueId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public string UniqueId { get; set; }

        /**
         *
         * SignalType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public UnlockSignalData.SignalType SignalType { get; set; }

        /**
         *
         * TargetPosition değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public ZeroVector3 TargetPosition { get; set; }

        /**
         *
         * TargetDescription değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public string TargetDescription { get; set; }

        /**
         *
         * IsVisited değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public bool IsVisited { get; set; }

        /**
         *
         * IsRemoved değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public bool IsRemoved { get; set; }
    }
}
