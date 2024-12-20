namespace Subnautica.Network.Core.Components
{
    using System;

    using MessagePack;

    using ItemModel = Subnautica.Network.Models.Items;

    [Union(0, typeof(ItemModel.Scanner))]
    [Union(1, typeof(ItemModel.FlashLight))]
    [Union(2, typeof(ItemModel.Knife))]
    [Union(3, typeof(ItemModel.LaserCutter))]
    [Union(4, typeof(ItemModel.Constructor))]
    [Union(5, typeof(ItemModel.Hoverbike))]
    [Union(6, typeof(ItemModel.DeployableStorage))]
    [Union(7, typeof(ItemModel.AirBladder))]
    [Union(8, typeof(ItemModel.Seaglide))]
    [Union(9, typeof(ItemModel.LEDLight))]
    [Union(10, typeof(ItemModel.Flare))]
    [Union(11, typeof(ItemModel.Thumper))]
    [Union(12, typeof(ItemModel.Welder))]
    [Union(13, typeof(ItemModel.Beacon))]
    [Union(14, typeof(ItemModel.TeleportationTool))]
    [Union(15, typeof(ItemModel.SpyPenguin))]
    [Union(16, typeof(ItemModel.MetalDetector))]
    [Union(17, typeof(ItemModel.DroneCamera))]
    [Union(18, typeof(ItemModel.PipeSurfaceFloater))]
    [MessagePackObject]
    public abstract class NetworkPlayerItemComponent
    {
        /**
         *
         * UniqueId değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public string UniqueId { get; set; }

        /**
         *
         * TechType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public virtual TechType TechType { get; set; } = TechType.None;

        /**
         *
         * Komponenti döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public T GetComponent<T>()
        {
            if (this is T)
            {
                return (T)Convert.ChangeType(this, typeof(T));
            }

            return default(T);
        }
    }
}
