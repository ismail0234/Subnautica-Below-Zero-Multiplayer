namespace Subnautica.API.Features.NetworkUtility
{
    using System.Collections.Generic;

    using UWE;

    public class EntityDatabase
    {
        /**
         *
         * TechTypeInfos değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private readonly Dictionary<TechType, WorldEntityInfo> TechTypeInfos = new Dictionary<TechType, WorldEntityInfo>();

        /**
         *
         * Teknoloji bilgisini önbelleğe ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void AddTechTypeInfo(TechType techType, WorldEntityInfo info)
        {
            this.TechTypeInfos[techType] = info;
        }

        /**
         *
         * Teknoloji bilgisini önbellekten döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool TryGetInfoByTechType(TechType techType, out WorldEntityInfo info)
        {
            return this.TechTypeInfos.TryGetValue(techType, out info);
        }

        /**
         *
         * Teknoloji bilgisini önbellekten döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool TryGetInfoByClassId(string classId, out WorldEntityInfo info)
        {
            return WorldEntityDatabase.TryGetInfo(classId, out info);
        }

        /**
         *
         * Verileri temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Dispose()
        {

        }
    }
}
