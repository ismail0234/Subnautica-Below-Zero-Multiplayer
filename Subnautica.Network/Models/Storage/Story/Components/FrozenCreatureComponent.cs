namespace Subnautica.Network.Models.Storage.Story.Components
{
    using MessagePack;

    [MessagePackObject]
    public class FrozenCreatureComponent
    {
        /**
         *
         * IsSampleAdded Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public bool IsSampleAdded { get; set; }

        /**
         *
         * IsInjected Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public bool IsInjected { get; set; }

        /**
         *
         * InjectTime Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public float InjectTime { get; set; }

        /**
         *
         * Köprüyü uzatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool AddSample()
        {
            if (this.IsSampleAdded)
            {
                return false;
            }
            
            this.IsSampleAdded = true;
            return true;
        }

        /**
         *
         * Köprüyü uzatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Inject(float serverTime)
        {
            if (this.IsInjected)
            {
                return false;
            }

            this.IsInjected = true;
            this.InjectTime = serverTime;
            return true;
        }
    }
}