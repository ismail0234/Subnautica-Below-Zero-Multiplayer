namespace Subnautica.Events.EventArgs
{
    using System;

    public class WeatherProfileChangedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public WeatherProfileChangedEventArgs(string profileId, bool isProfile, bool isAllowed = true)
        {
            this.ProfileId = profileId;
            this.IsProfile = isProfile;
            this.IsAllowed = isAllowed;
        }

        /**
         *
         * ProfileId Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string ProfileId { get; set; }

        /**
         *
         * IsProfile Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsProfile { get; set; }

        /**
         *
         * IsAllowed Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}
