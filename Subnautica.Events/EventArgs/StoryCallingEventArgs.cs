namespace Subnautica.Events.EventArgs
{
    using System;

    public class StoryCallingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StoryCallingEventArgs(string callGoalKey, string targetGoalKey, bool isAnswered, bool isAllowed = true)
        {
            this.CallGoalKey   = callGoalKey;
            this.TargetGoalKey = targetGoalKey;
            this.IsAnswered    = isAnswered;
            this.IsAllowed     = isAllowed;
        }

        /**
         *
         * CallGoalKey Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string CallGoalKey { get; private set; }

        /**
         *
         * TargetGoalKey Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string TargetGoalKey { get; private set; }

        /**
         *
         * IsAnswered Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAnswered { get; private set; }

        /**
         *
         * IsAllowed Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}