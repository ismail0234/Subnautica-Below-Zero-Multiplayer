namespace Subnautica.Events.EventArgs
{
    using System;

    public class BridgeFluidClickingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BridgeFluidClickingEventArgs(string uniqueId, string storyKey, bool isAllowed = true)
        {
            this.UniqueId = uniqueId;
            this.StoryKey = storyKey;
        }

        /**
         *
         * UniqueId Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; private set; }

        /**
         *
         * StoryKey Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string StoryKey { get; private set; }

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