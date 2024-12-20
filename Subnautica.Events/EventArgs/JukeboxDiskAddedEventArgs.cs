namespace Subnautica.Events.EventArgs
{
    using System;

    public class JukeboxDiskAddedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public JukeboxDiskAddedEventArgs(string trackFile, bool notify)
        {
            TrackFile = trackFile;
            Notify = notify;
        }

        /**
         *
         * TrackFile Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string TrackFile { get; private set; }

        /**
         *
         * Notify Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Notify { get; private set; }
    }
}
