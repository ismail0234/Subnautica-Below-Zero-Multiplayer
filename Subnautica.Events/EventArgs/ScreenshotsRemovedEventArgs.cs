namespace Subnautica.Events.EventArgs
{
    using System;

    public class ScreenshotsRemovedEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ScreenshotsRemovedEventArgs(string imageName)
        {
            this.ImageName = imageName;
        }

        /**
         *
         * Kimlik
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string ImageName { get; private set; }
    }
}