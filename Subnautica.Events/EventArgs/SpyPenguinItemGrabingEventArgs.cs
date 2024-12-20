namespace Subnautica.Events.EventArgs
{
    using System;

    public class SpyPenguinItemGrabingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public SpyPenguinItemGrabingEventArgs(string uniqueId, string animationName)
        {
            this.UniqueId      = uniqueId;
            this.AnimationName = animationName;
        }

        /**
         *
         * UniqueId Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; set; }

        /**
         *
         * AnimationName Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string AnimationName { get; set; }
    }
}