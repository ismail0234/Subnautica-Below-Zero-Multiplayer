namespace Subnautica.Events.EventArgs
{
    using System;

    public class PictureFrameImageSelectingEventArgs : EventArgs
    {
        /**
         *
         * Sınıf ayarlamalarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public PictureFrameImageSelectingEventArgs(string uniqueId, string imagename, byte[] imageData, bool isAllowed = true)
        {
            this.UniqueId  = uniqueId;
            this.ImageName = imagename;
            this.ImageData = imageData;
            this.IsAllowed = isAllowed;
        }

        /**
         *
         * UniqueId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; set; }

        /**
         *
         * ImageName değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string ImageName { get; set; }

        /**
         *
         * ImageData Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public byte[] ImageData { get; set; }

        /**
         *
         * Olayın çalıştırılıp/çalıştırılmayacağı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowed { get; set; }
    }
}
