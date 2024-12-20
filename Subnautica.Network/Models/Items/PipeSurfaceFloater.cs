namespace Subnautica.Network.Models.Items
{
    using MessagePack;

    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Storage.World.Childrens;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;
    using Subnautica.Network.Structures;

    [MessagePackObject]
    public class PipeSurfaceFloater : NetworkPlayerItemComponent
    {
        /**
         *
         * TechType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public override TechType TechType { get; set; } = TechType.PipeSurfaceFloater;

        /**
         *
         * ProcessType Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public byte ProcessType { get; set; }

        /**
         *
         * PipeId Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public string PipeId { get; set; }

        /**
         *
         * ParentId Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public string ParentId { get; set; }

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
         * Rotation Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public ZeroQuaternion Rotation { get; set; }

        /**
         *
         * Entity Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public WorldDynamicEntity Entity { get; set; }

        /**
         *
         * PickupItem Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public WorldPickupItem PickupItem { get; set; }

        /**
         *
         * IsSurfaceFloaterDeploy kontrolü yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsSurfaceFloaterDeploy()
        {
            return this.ProcessType == 1;
        }

        /**
         *
         * IsOxygenPipePlace kontrolü yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsOxygenPipePlace()
        {
            return this.ProcessType == 3;
        }

        /**
         *
         * IsOxygenPipePickup kontrolü yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsOxygenPipePickup()
        {
            return this.ProcessType == 4;
        }
    }
}