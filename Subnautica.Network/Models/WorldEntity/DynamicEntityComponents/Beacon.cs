namespace Subnautica.Network.Models.WorldEntity.DynamicEntityComponents
{
    using MessagePack;

    using Subnautica.Network.Core.Components;

    [MessagePackObject]
    public class Beacon : NetworkDynamicEntityComponent
    {
        /**
         *
         * IsDeployedOnLand Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public bool IsDeployedOnLand { get; set; }

        /**
         *
         * Text Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public string Text { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Beacon()
        {

        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Beacon(bool isDeployedOnLand, string text)
        {
            this.IsDeployedOnLand = isDeployedOnLand;
            this.Text             = text;
        }

        /**
         *
         * Metni değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetText(string text)
        {
            this.Text = text;
        }
    }
}