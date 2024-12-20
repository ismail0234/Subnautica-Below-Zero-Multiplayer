namespace Subnautica.API.Enums
{
    public enum CreatureSpawnLevel : byte
    {
        /**
         *
         * Varsayılan (PrefabDatabase.TryGetPrefabFilename)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        Default,

        /**
         *
         * Sahne (PrefabDatabase.GetPrefabAsync)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        Scene,

        /**
         *
         * Özel (CreatureData.OnCustomCreatureSpawn)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        Custom,

        /**
         *
         * Özel - ASYNC (CreatureData.OnCustomCreatureSpawnAsync)
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        CustomAsync
    }
}
