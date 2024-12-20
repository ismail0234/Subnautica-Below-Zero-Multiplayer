namespace Subnautica.Network.Models.Server
{
    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Models.Core;

    [MessagePackObject]
    public class StoryTriggerArgs : NetworkPacket
    {
        /**
         *
         * Ağ Paket Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public override ProcessType Type { get; set; } = ProcessType.StoryTrigger;

        /**
         *
         * GoalKey Değerini Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public string GoalKey { get; set; }

        /**
         *
         * StoryGoalKey Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public global::Story.GoalType GoalType { get; set; }

        /**
         *
         * CinematicType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public StoryCinematicType CinematicType { get; set; }

        /**
         *
         * IsStoryGoalMuted Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public bool IsStoryGoalMuted { get; set; }

        /**
         *
         * IsTrigger Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(9)]
        public bool IsTrigger { get; set; }

        /**
         *
         * IsPlayMuted Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(10)]
        public bool IsPlayMuted { get; set; }

        /**
         *
         * IsTrigger Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(11)]
        public bool IsClearSound { get; set; }

        /**
         *
         * TriggerTime Değerini Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(12)]
        public float TriggerTime { get; set; }

        /**
         *
         * PlayerCount Değerini Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(13)]
        public byte PlayerCount { get; set; }

        /**
         *
         * MaxPlayerCount Değerini Barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(14)]
        public byte MaxPlayerCount { get; set; }
    }
}