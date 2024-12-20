namespace Subnautica.Network.Models.Metadata
{
    using MessagePack;

    using Subnautica.Network.Core.Components;

    [MessagePackObject]
    public class PictureFrame : MetadataComponent
    {
        /**
         *
         * ImageName barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public string ImageName { get; set; }

        /**
         *
         * ImageData değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public byte[] ImageData { get; set; }

        /**
         *
         * ImageData değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public bool IsOpening { get; set; }

        /**
         *
         * Sınıf ayarlamarlarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public PictureFrame()
        {

        }

        /**
         *
         * Sınıf ayarlamarlarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public PictureFrame(string imageName, byte[] imageData, bool isOpening)
        {
            this.ImageName = imageName;
            this.ImageData = imageData;
            this.IsOpening = isOpening;
        }
    }
}
