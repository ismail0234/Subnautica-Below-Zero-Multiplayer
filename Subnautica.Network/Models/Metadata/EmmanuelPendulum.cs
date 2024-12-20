namespace Subnautica.Network.Models.Metadata
{
    using MessagePack;

    using Subnautica.Network.Core.Components;

    [MessagePackObject]
    public class EmmanuelPendulum : MetadataComponent
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
        public EmmanuelPendulum()
        {

        }

        /**
         *
         * Sınıf ayarlamarlarını yapar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public EmmanuelPendulum(bool isActive)
        {
            this.IsActive = isActive;
        }
    }
}
