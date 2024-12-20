namespace Subnautica.Network.Models.Items
{
    using MessagePack;

    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class Constructor : NetworkPlayerItemComponent
    {
        /**
         *
         * TechType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public override TechType TechType { get; set; } = TechType.Constructor;

        /**
         *
         * Forward Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public ZeroVector3 Forward { get; set; }

        /**
         *
         * Position Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public ZeroVector3 Position { get; set; }

        /**
         *
         * Entity Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public WorldDynamicEntity Entity { get; set; }

        /**
         *
         * EngageToggle Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public byte EngageToggle { get; set; }

        /**
         *
         * CraftingTechType Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public TechType CraftingTechType { get; set; }

        /**
         *
         * CraftingFinishTime Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(9)]
        public float CraftingFinishTime { get; set; }

        /**
         *
         * CraftingFinishTime Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(10)]
        public ZeroVector3 CraftingPosition { get; set; }

        /**
         *
         * CraftingFinishTime Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(11)]
        public ZeroQuaternion CraftingRotation { get; set; }
        
        /**
         *
         * EngageToggle aktif olup/olmadığını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsEngageActive()
        {
            return this.EngageToggle != 0;
        }

        /**
         *
         * Engage olup/olmadığını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsEngage()
        {
            return this.EngageToggle == 1;
        }
    }
}