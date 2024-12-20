namespace Subnautica.Network.Models.Storage.Story.Components
{
    using MessagePack;

    [MessagePackObject]
    public class ShieldBaseComponent
    {
        /**
         *
         * IsEntered Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public bool IsFirstEntered { get; set; }

        /**
         *
         * Kalkan üssüne ilk giriş durumunu ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Enter()
        {
            if (this.IsFirstEntered)
            {
                return false;
            }

            this.IsFirstEntered = true;
            return true;
        }
    }
}