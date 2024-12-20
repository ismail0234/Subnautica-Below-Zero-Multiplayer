namespace Subnautica.Network.Models.Metadata
{
    using MessagePack;

    using Subnautica.Network.Core.Components;

    [MessagePackObject]
    public class AromatherapyLamp : MetadataComponent
    {
        /**
         *
         * IsActive değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public bool IsActive { get; set; }

        /**
         *
         * Sınıf ayarlamarlarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public AromatherapyLamp()
        {

        }

        /**
         *
         * Sınıf ayarlamarlarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public AromatherapyLamp(bool isActive)
        {
            this.IsActive = isActive;
        }
    }
}
