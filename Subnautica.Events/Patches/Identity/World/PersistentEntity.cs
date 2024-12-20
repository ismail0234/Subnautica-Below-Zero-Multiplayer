namespace Subnautica.Events.Patches.Identity.World
{
    using System;
    using System.Collections;

    using HarmonyLib;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using UnityEngine;

    using UWE;

    [HarmonyPatch(typeof(global::VirtualPrefabIdentifier), nameof(global::VirtualPrefabIdentifier.SpawnPrefab))]
    public static class PersistentEntity
    {
        private static IEnumerator Postfix(IEnumerator values, global::VirtualPrefabIdentifier __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                if (!PrefabDatabase.TryGetPrefabFilename(__instance.ClassId, out string filename))
                {
                    Debug.LogErrorFormat(__instance, "Failed to request prefab for '{0}'", __instance.ClassId);
                    UnityEngine.Object.Destroy(__instance.gameObject);
                }
                else
                {
                    DeferredSpawner.Task task = DeferredSpawner.instance.InstantiateAsync(filename, __instance, __instance.transform.parent, __instance.transform.localPosition, __instance.transform.localRotation, false, __instance.highPriority);
                    yield return task;

                    var result = task.GetResult();
                    if (result != null)
                    {
                        Network.Identifier.SetIdentityId(result, __instance.Id);

                        result.transform.localScale = __instance.transform.localScale;
                        result.SetActive(true);

                        try
                        {
                            WorldEntityDatabase.TryGetInfo(__instance.ClassId, out var info);

                            EntitySpawnedEventArgs args = new EntitySpawnedEventArgs(__instance.Id, result, __instance.ClassId, info.techType, EntitySpawnLevel.VirtualPrefab, true);

                            Handlers.World.OnEntitySpawned(args);
                        }
                        catch (Exception e)
                        {
                            Log.Error($"PersistentEntity.Postfix: {e}\n{e.StackTrace}");
                        }
                    }

                    UnityEngine.Object.Destroy(__instance.gameObject);
                    __instance.spawnCoroutine = null;
                }
            }
            else
            {
                yield return values;
            }
        }
    }
}
