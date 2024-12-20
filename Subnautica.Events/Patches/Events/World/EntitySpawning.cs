namespace Subnautica.Events.Patches.Events.World
{
    using System;
    using System.Collections;
    using System.IO;

    using HarmonyLib;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Client.Extensions;
    using Subnautica.Events.EventArgs;

    using UnityEngine;

    using UWE;

    [HarmonyPatch(typeof(global::ProtobufSerializer), nameof(global::ProtobufSerializer.DeserializeObjectsAsync), new Type[] { typeof(Stream), typeof(UniqueIdentifier), typeof(bool), typeof(bool), typeof(Transform), typeof(bool), typeof(int), typeof(IOut<GameObject>) })]
    public static class EntitySpawningProtobuf
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator Postfix(IEnumerator values, global::ProtobufSerializer __instance, Stream stream, UniqueIdentifier rootUid, bool forceInactiveRoot, bool forceParent, Transform parent, bool allowSpawnRestrictions, int verbose, IOut<GameObject> result)
        {
            if (!Network.IsMultiplayerActive || !__instance.IsCellModeActive())
            {
                yield return values;
            }
            else
            {
                ProtobufSerializer.LoopHeader target = ProtobufSerializer.loopHeaderPool.Get();
                target.Reset();
                __instance.Deserialize<ProtobufSerializer.LoopHeader>(stream, target, verbose > 3);

                int gameObjectCount = target.Count;
                ProtobufSerializer.loopHeaderPool.Return(target);

                var prefabInstantiateResult = new TaskResult<UniqueIdentifier>();
                var gameObjectData = ProtobufSerializer.gameObjectDataPool.Get();

                for (int i = 0; i < gameObjectCount; ++i)
                {
                    gameObjectData.Reset();
                    __instance.Deserialize<ProtobufSerializer.GameObjectData>(stream, gameObjectData, verbose > 0);

                    WorldEntityInfo info = null;
                    var isAllowed        = true;
                    var isExistEntity    = !string.IsNullOrEmpty(gameObjectData.ClassId) && WorldEntityDatabase.TryGetInfo(gameObjectData.ClassId, out info);
                    if (isExistEntity)
                    {
                        EntitySpawningEventArgs args = new EntitySpawningEventArgs(gameObjectData.Id, gameObjectData.ClassId, info.techType, EntitySpawnLevel.Protobuf, true);

                        Handlers.World.OnEntitySpawning(args);

                        isAllowed = args.IsAllowed;
                    }

                    if (!isAllowed || (allowSpawnRestrictions && !SpawnRestrictionEnforcer.ShouldSpawn(gameObjectData.ClassId)))
                    {
                        __instance.SkipFullGameObjectDeserialization(stream, verbose);
                    }
                    else
                    {
                        UniqueIdentifier uid;
                        if (i == 0 && rootUid != null)
                        {
                            uid = rootUid;
                        }
                        else if (gameObjectData.CreateEmptyObject)
                        {
                            uid = __instance.CreateEmptyGameObject("SerializerEmptyGameObject");
                        }
                        else if (gameObjectData.MergeObject)
                        {
                            uid = ProtobufSerializer.FindChildObject(gameObjectData);
                        }
                        else
                        {
                            yield return ProtobufSerializer.InstantiatePrefabAsync(gameObjectData, prefabInstantiateResult);
                            uid = prefabInstantiateResult.Get();
                        }

                        if (i == 0)
                        {
                            GameObject gameObject = uid.gameObject;
                            if (forceInactiveRoot)
                            {
                                gameObject.SetActive(false);
                            }

                            result.Set(gameObject);
                        }

                        __instance.DeserializeIntoGameObject(stream, gameObjectData, uid, forceInactiveRoot && i == 0, forceParent, parent, verbose);
                        if (__instance.ShouldNotifyAsyncListeners(uid))
                        {
                            yield return __instance.NotifyAsyncListenersAsync(uid);
                        }

                        if (isExistEntity)
                        {
                            try
                            {
                                EntitySpawnedEventArgs args = new EntitySpawnedEventArgs(gameObjectData.Id, uid.gameObject, gameObjectData.ClassId, info.techType, EntitySpawnLevel.Protobuf, false);

                                Handlers.World.OnEntitySpawned(args);
                            }
                            catch (Exception e)
                            {
                                Log.Error($"EntitySpawningProtobuf.Postfix: {e}\n{e.StackTrace}");
                            }
                        }
                    }
                }

                ProtobufSerializer.gameObjectDataPool.Return(gameObjectData);
            }
        }
    }

    [HarmonyPatch(typeof(global::PrefabPlaceholder), nameof(global::PrefabPlaceholder.Spawn))]
    public static class EntitySpawning_PrefabPlaceholder
    {
        private static bool Prefix(global::PrefabPlaceholder __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (string.IsNullOrEmpty(__instance.prefabClassId) || !SpawnRestrictionEnforcer.ShouldSpawn(__instance.prefabClassId) || !WorldEntityDatabase.TryGetInfo(__instance.prefabClassId, out var info))
            {
                return false;
            }
            
            try
            {
                EntitySpawningEventArgs beforeArgs = new EntitySpawningEventArgs(Network.GetWorldEntityId(__instance.transform.position), __instance.prefabClassId, info.techType, EntitySpawnLevel.PrefabPlaceholder, true);

                Handlers.World.OnEntitySpawning(beforeArgs);

                if (!beforeArgs.IsAllowed)
                {
                    return false;
                }

                var virtualEntityPrefab = VirtualEntitiesManager.GetVirtualEntityPrefab();
                virtualEntityPrefab.SetActive(false);

                var gameObject = UWE.Utils.InstantiateDeactivated(virtualEntityPrefab, __instance.transform.parent, __instance.transform.localPosition, __instance.transform.localRotation, __instance.transform.localScale);
                var component  = gameObject.GetComponent<VirtualPrefabIdentifier>();

                component.ClassId      = __instance.prefabClassId;
                component.highPriority = __instance.highPriority;
                component.Id           = beforeArgs.UniqueId;

                LargeWorldEntity worldEntity = gameObject.GetComponent<LargeWorldEntity>();
                worldEntity.cellLevel = info.cellLevel;

                if (LargeWorldStreamer.main != null)
                {
                    LargeWorldStreamer.main.cellManager.UnregisterEntity(worldEntity);
                }

                gameObject.SetActive(true);

                return false;
            }
            catch (Exception e)
            {
                Log.Error($"EntitySpawningPrefabPlaceholder.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }

    [HarmonyPatch(typeof(global::FixedBase), nameof(global::FixedBase.LoadBulkheadDoorsStates))]
    public static class EntitySpawning_FixedBase
    {
        private static void Postfix(global::FixedBase __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                for (int index = 0; index < __instance.bulkheadDoors.Length; ++index)
                {
                    var uniqueId = Network.GetWorldEntityId(__instance.bulkheadDoors[index].transform.position);

                    Network.Identifier.SetIdentityId(__instance.bulkheadDoors[index].gameObject, uniqueId);

                    EntitySpawnedEventArgs args = new EntitySpawnedEventArgs(uniqueId, __instance.bulkheadDoors[index].gameObject, null, TechType.BaseBulkhead, EntitySpawnLevel.FixedBase, true);

                    try
                    {
                        Handlers.World.OnEntitySpawned(args);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"EntitySpawningFixedBase.Postfix: {e}\n{e.StackTrace}");
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(global::PickPrefab), nameof(global::PickPrefab.Start))]
    public class EntitySpawning_PickPrefab
    {
        private static void Prefix(global::PickPrefab __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                if (string.IsNullOrEmpty(Network.Identifier.GetIdentityId(__instance.gameObject, false)))
                {
                    var uniqueId = Network.Identifier.GetWorldEntityId(__instance.transform.position, "Plant");

                    Network.Identifier.SetIdentityId(__instance.gameObject, uniqueId);

                    try
                    {
                        EntitySpawningEventArgs args = new EntitySpawningEventArgs(uniqueId, null, TechType.IceFruit, EntitySpawnLevel.PickPrefab, true);

                        Handlers.World.OnEntitySpawning(args);

                        if (!args.IsAllowed)
                        {
                            UnityEngine.Object.Destroy(__instance.gameObject);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error($"EntitySpawning_PickPrefab.Prefix: {e}\n{e.StackTrace}");
                    }
                }
            }
        }
    }
}