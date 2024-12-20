namespace Subnautica.Events.EventArgs
{
    using System;

    using Subnautica.API.Enums;

    public class StoryGoalTriggeringEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StoryGoalTriggeringEventArgs(string storyKey, global::Story.GoalType goalType, bool isPlayMuted, bool isStoryGoalMuted = false, StoryCinematicType cinematicType = StoryCinematicType.None, bool isAllowed = true)
        {
            this.StoryKey         = storyKey;
            this.GoalType         = goalType;
            this.IsPlayMuted      = isPlayMuted;
            this.IsStoryGoalMuted = isStoryGoalMuted;
            this.CinematicType    = cinematicType;
            this.IsAllowed        = isAllowed;
        }

        /**
         *
         * StoryKey değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string StoryKey { get; set; }

        /**
         *
         * GoalType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::Story.GoalType GoalType { get; set; }

        /**
         *
         * IsPlayMuted değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsPlayMuted { get; set; }

        /**
         *
         * IsStoryGoalMuted değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsStoryGoalMuted { get; set; }

        /**
         *
         * CinematicType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StoryCinematicType CinematicType { get; set; }

        /**
         *
         * IsAllowed değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}