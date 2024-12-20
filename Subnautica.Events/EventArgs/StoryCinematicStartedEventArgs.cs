namespace Subnautica.Events.EventArgs
{
    using System;

    public class StoryCinematicStartedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public StoryCinematicStartedEventArgs(string cinematicName)
        {
            this.CinematicName = cinematicName;
        }

        /**
         *
         * CinematicName değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string CinematicName { get; set; }
    }
}
