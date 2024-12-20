namespace Subnautica.Network.Models.Metadata
{
    using MessagePack;

    using Subnautica.Network.Core.Components;

    [MessagePackObject]
    public class Crafter : MetadataComponent
    {
        /**
         *
         * IsOpened değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public bool IsOpened { get; set; }

        /**
         *
         * IsPickup değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public bool IsPickup { get; set; }

        /**
         *
         * CraftingTechType değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public TechType CraftingTechType { get; set; }

        /**
         *
         * CraftingDuration değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public float CraftingDuration { get; set; }

        /**
         *
         * CraftingEndTime değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public float CraftingStartTime { get; set; }

        /**
         *
         * CrafterClone değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public Crafter CrafterClone { get; set; }

        /**
         *
         * Zanaatkarı açar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Open()
        {
            this.IsOpened = true;
            return true;
        }

        /**
         *
         * Zanaatkarı kapatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Close()
        {
            this.IsOpened = false;
            return true;
        }

        /**
         *
         * Zanaatkarlığı başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool Craft(TechType techType, float startTime, float duration)
        {
            if (this.CraftingTechType != TechType.None)
            {
                return false;
            }

            this.CraftingTechType  = techType;
            this.CraftingStartTime = startTime;
            this.CraftingDuration  = duration;
            return true;
        }

        /**
         *
         * Nesneyi alır. başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool TryPickup()
        {
            if (this.CraftingTechType == TechType.None)
            {
                return false;
            }

            this.CraftingTechType  = TechType.None;
            this.CraftingStartTime = 0f;
            this.CraftingDuration  = 0f;
            return true;
        }
    }
}
