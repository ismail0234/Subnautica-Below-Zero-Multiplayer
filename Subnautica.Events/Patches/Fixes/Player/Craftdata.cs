namespace Subnautica.Events.Patches.Fixes.Player
{
    using System.Collections;

    using HarmonyLib;

    using Subnautica.API.Features;

    using UnityEngine;

    [HarmonyPatch]
    public static class Craftdata
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::InventoryConsoleCommands), nameof(global::InventoryConsoleCommands.ItemCmdSpawnAsync))]
        private static IEnumerator ItemCmdSpawnAsync(IEnumerator values, int number, TechType techType)
        {
            if (!Network.IsMultiplayerActive)
            {
                yield return values;
            }
            else
            {
                var result = new TaskResult<GameObject>();
                for (int i = 0; i < number; ++i)
                {
                    yield return CraftData.InstantiateFromPrefabAsync(techType, result);

                    var target = result.Get();
                    if (target)
                    {
                        target.transform.position = MainCamera.camera.transform.position + MainCamera.camera.transform.forward * 3f;

                        CrafterLogic.NotifyCraftEnd(target, techType);
                        var component = target.GetComponent<Pickupable>();
                        if (component != null && !Inventory.main.Pickup(component))
                        {
                            ErrorMessage.AddError(global::Language.main.Get("InventoryFull"));
                            UnityEngine.Object.Destroy(target);
                        }
                    }
                }                
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::CraftData), nameof(global::CraftData.AddToInventoryAsync))]
        private static void AddToInventoryAsync(CraftData __instance, ref bool spawnIfCantAdd)
        {
            if (Network.IsMultiplayerActive)
            {
                spawnIfCantAdd = false;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Survival), nameof(global::Survival.Use))]
        private static bool Use(Survival __instance, GameObject useObj, Inventory inventory)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (useObj)
            {
                var techType = CraftData.GetTechType(useObj);
                if (techType == TechType.None)
                {
                    var component = useObj.GetComponent<global::Pickupable>();
                    if (component)
                    {
                        techType = component.GetTechType();
                    }
                }

                if (techType == TechType.WaterPurificationTablet && !inventory.HasRoomFor(techType))
                {
                    ErrorMessage.AddError(global::Language.main.Get("InventoryFull"));
                    return false;
                }
            }

            return true;
        }
    }
}
