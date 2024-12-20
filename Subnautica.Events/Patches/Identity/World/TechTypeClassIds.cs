namespace Subnautica.Events.Patches.Identity.World
{
    using System.Collections.Generic;
    using System.Linq;

    using HarmonyLib;

    using Subnautica.API.Features;

    using UWE;

    [HarmonyPatch]
    public static class TechTypeClassIds
    {
        /**
         *
         * Üzerine yazılacak Teknolojiler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Dictionary<string, TechType> Overrides { get; set; } = new Dictionary<string, TechType>()
        {
            { "Misc/JukeboxDisk"              , TechType.JukeboxDisksAll },
            { "_PDA"                          , TechType.PDA },
            { "WorldEntities/Alterra/PDAs/"   , TechType.PDA },
            { "LifePodDrop_Fabricator"        , TechType.Fabricator },
            { "LifepodDrop_SmallLocker"       , TechType.SmallLocker },
            { "SupplyCrate"                   , TechType.StarshipCargoCrate },
            { "DataBoxes"                     , TechType.Databox },
            { "submarine_locker"              , TechType.BaseBulkhead },
            { "_PartFabricator"               , TechType.Fabricator },
            { "Creature_Elevator_Interactive" , TechType.RocketBase },
            { "underwater_ice_brinicle_large" , TechType.Brinicle },
            { "Drillable"                     , TechType.OreVein },
        };

        /**
         *
         * Extra prefabs
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Dictionary<string, KeyValuePair<string, TechType>> ExtraPrefabs { get; set; } = new Dictionary<string, KeyValuePair<string, TechType>>()
        {
            { "a8168151-52eb-4cd0-989d-f91bb601eb4f", new KeyValuePair<string, TechType>("WorldEntities/EnvironmentResources/CrashPowder.prefab", TechType.CrashPowder) },
        };

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::UWE.WorldEntityDatabase), MethodType.Constructor)]
        private static void Postfix(global::UWE.WorldEntityDatabase __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                foreach (var prefab in PrefabDatabase.prefabFiles)
                {
                    var data = Overrides.Where(q => prefab.Value.Contains(q.Key)).FirstOrDefault(); 
                    if (data.Key != null)
                    {
                        if (__instance.infos.TryGetValue(prefab.Key, out var info) && info.techType == TechType.None)
                        {
                            info.techType = data.Value;
                        }
                    }
                }

                foreach (var info in __instance.infos)
                {
                    if (info.Value.techType != TechType.None)
                    {
                        Network.EntityDatabase.AddTechTypeInfo(info.Value.techType, info.Value);
                    }
                }
            }
        }

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::UWE.PrefabDatabase), nameof(global::UWE.PrefabDatabase.LoadPrefabDatabase))]
        private static void Postfix()
        {
            if (Network.IsMultiplayerActive)
            {
                foreach (var prefab in ExtraPrefabs)
                {
                    PrefabDatabase.prefabFiles[prefab.Key] = prefab.Value.Key;

                    CraftData.entClassTechTable[prefab.Key]   = prefab.Value.Value;
                    CraftData.techMapping[prefab.Value.Value] = prefab.Key;
                }
            }
        }
    }
}