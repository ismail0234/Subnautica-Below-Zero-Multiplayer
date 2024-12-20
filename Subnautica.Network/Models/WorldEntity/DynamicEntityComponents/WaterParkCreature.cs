namespace Subnautica.Network.Models.WorldEntity.DynamicEntityComponents
{
    using MessagePack;

    using Subnautica.Network.Core.Components;

    [MessagePackObject]
    public class WaterParkCreature : NetworkDynamicEntityComponent
    {
        /**
         *
         * AddedTime Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public double AddedTime { get; set; }

        /**
         *
         * WaterParkId Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public string WaterParkId { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public WaterParkCreature()
        {

        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public WaterParkCreature(double addedTime, string waterParkId)
        {
            this.AddedTime   = addedTime;
            this.WaterParkId = waterParkId;
        }

        /**
         *
         * Yumurta doğma zamanını değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SpawnChildren(double currentTime)
        {
            this.AddedTime = currentTime;
        }

        /**
         *
         * Doğabilir mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsSpawnable(double currentTime)
        {
            return currentTime - this.AddedTime >= 1200;
        }
    }
}