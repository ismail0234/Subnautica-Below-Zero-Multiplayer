namespace Subnautica.API.Features
{
    using Subnautica.API.Features.Creatures.Datas;
    using Subnautica.API.Features.NetworkUtility;

    using System.Collections.Generic;

    public class CreatureData
    {
        /**
         *
         * Sınıfı barındır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static CreatureData instance;

        /**
         *
         * Sınıf örneğini barındır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static CreatureData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CreatureData();
                }

                return instance;
            }
        }

        /**
         *
         * Verileri barındır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<TechType, BaseCreatureData> Datas { get; set; } = new Dictionary<TechType, BaseCreatureData>();

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public CreatureData()
        {
            this.Register(new SkyrayData());
            this.Register(new CrashFishData());
            this.Register(new TitanHolefishData());
            this.Register(new JellyFishData());
            this.Register(new ArcticRayData());
            this.Register(new VentGardenSmallData());
            this.Register(new LilyPaddlerData());
            this.Register(new GlowWhaleData());
            this.Register(new ChelicerateData());
            this.Register(new ShadowLeviathanData());
            this.Register(new VoidLeviathanData());
            this.Register(new BruteSharkData());
            this.Register(new CryptosuchusData());
            
            this.Register(new GlowWhaleEggData());
        }

        /**
         *
         * Veri kaydı kontrolü yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsExists(TechType type)
        {
            return this.Datas.ContainsKey(type);
        }

        /**
         *
         * Veri kaydını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BaseCreatureData GetCreatureData(TechType type)
        {
            this.Datas.TryGetValue(type, out BaseCreatureData creatureData);
            return creatureData;
        }

        /**
         *
         * Veri kaydını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Register(BaseCreatureData creatureData)
        {
            if (this.Datas.ContainsKey(creatureData.CreatureType))
            {
                Log.Error($"Creature Register Error: It has already been defined - {creatureData.CreatureType}");
            }
            else
            {
                this.Datas.Add(creatureData.CreatureType, creatureData);
            }
        }

        /**
         *
         * Veri kaydını siler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void UnRegister(TechType techType)
        {
            this.Datas.Remove(techType);
        }
    }
}
