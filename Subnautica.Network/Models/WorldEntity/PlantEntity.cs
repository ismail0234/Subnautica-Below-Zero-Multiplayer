namespace Subnautica.Network.Models.WorldEntity
{
    using MessagePack;

    using Subnautica.API.Enums;
    using Subnautica.Network.Core.Components;

    [MessagePackObject]
    public class PlantEntity : NetworkWorldEntityComponent
    {
        /**
         *
         * ProcessType değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public override EntityProcessType ProcessType { get; set; } = EntityProcessType.Plant;

        /**
         *
         * TechType değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public TechType TechType { get; set; }

        /**
         *
         * ActiveFruitCount değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(5)]
        public byte ActiveFruitCount { get; set; } = 0;

        /**
         *
         * MaxFruit değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(6)]
        public byte MaxFruit { get; set; } = 0;

        /**
         *
         * TimeNextFruit değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(7)]
        public float TimeNextFruit { get; set; } = 0;

        /**
         *
         * SpawnInterval Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(8)]
        public float SpawnInterval { get; set; } = 0f;

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public PlantEntity()
        {

        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public PlantEntity(TechType techType, byte activeFruitCount, byte maxFruit, float timeNextFruit, float spawnInterval)
        {
            this.TechType         = techType;
            this.ActiveFruitCount = activeFruitCount;
            this.MaxFruit         = maxFruit;
            this.TimeNextFruit    = timeNextFruit;
            this.SpawnInterval    = spawnInterval;
        }

        /**
         *
         * Meyve bilgilerini senkronlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool SyncFruits(float currentTime, bool harvest = false)
        {
            if (this.SpawnInterval == -1f)
            {
                if (harvest)
                {
                    this.ActiveFruitCount--;
                    return true;
                }

                return false;
            }

            if (this.SpawnInterval == 0f)
            {
                this.SpawnInterval    = -1;
                this.ActiveFruitCount = 1;
                return false;
            }

            if (harvest)
            {
                this.ActiveFruitCount--;
            }

            while (this.ActiveFruitCount < this.MaxFruit && currentTime >= this.TimeNextFruit)
            {
                this.TimeNextFruit += this.SpawnInterval;

                this.ActiveFruitCount++;
            }

            if (this.ActiveFruitCount >= this.MaxFruit)
            {
                this.TimeNextFruit = currentTime + this.SpawnInterval;
            }

            return true;
        }
    }
}