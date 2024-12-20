namespace Subnautica.Network.Models.Storage.Story.StoryGoals
{
    using MessagePack;

    [MessagePackObject]
    public class ZeroStoryGoal
    {
        /**
         *
         * Hedef Anahtarı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public string Key { get; set; }

        /**
         *
         * GoalType Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public global::Story.GoalType GoalType { get; set; }

        /**
         *
         * IsPlayMuted Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public bool IsPlayMuted { get; set; }

        /**
         *
         * Time Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public float FinishedTime { get; set; }
    }
}
