namespace Subnautica.Events.Patches.Events.World
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using UnityEngine;

    using UWE;

    [HarmonyPatch]
    public static class EntitySlotSpawning
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::EntitySlotsPlaceholder), nameof(global::EntitySlotsPlaceholder.Spawn))]
        private static bool EntitySlotsPlaceholder_Spawn(global::EntitySlotsPlaceholder __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.slotsData == null)
            {
                return false;
            }

            __instance.virtualEntityPrefab.SetActive(false);

            try
            {
                foreach (var slotData in __instance.slotsData)
                {
                    EntitySlotSpawningEventArgs args = new EntitySlotSpawningEventArgs(__instance.transform.TransformPoint(slotData.localPosition).GetHashCode());

                    Handlers.World.OnEntitySlotSpawning(args);

                    if (!args.IsAllowed)
                    {
                        continue;
                    }

                    if (args.ClassId.IsNull() || !WorldEntityDatabase.TryGetInfo(args.ClassId, out var info))
                    {
                        continue;
                    }

                    if (SpawnRestrictionEnforcer.ShouldSpawn(args.ClassId))
                    {
                        var localPosition = slotData.localPosition;
                        var localRotation = slotData.localRotation;

                        if (info.prefabZUp)
                        {
                            localRotation *= Quaternion.Euler(new Vector3(-90f, 0.0f, 0.0f));
                        }

                        var gameObject    = UWE.Utils.InstantiateDeactivated(__instance.virtualEntityPrefab, __instance.transform, localPosition, localRotation, info.localScale);
                        var virtualPrefab = gameObject.GetComponent<VirtualPrefabIdentifier>();
                        virtualPrefab.Id      = args.SlotId.ToWorldStreamerId();
                        virtualPrefab.ClassId = args.ClassId;

                        var component = gameObject.GetComponent<LargeWorldEntity>();
                        component.cellLevel = info.cellLevel;

                        var isActive = false;
                        if (LargeWorld.main)
                        {
                            isActive = LargeWorld.main.streamer.cellManager.RegisterEntity(component);
                        }

                        gameObject.SetActive(isActive);

                        if (EntitySlot.debugSlots)
                        {
                            EntitySlotSpawning.ShowDebugSlot(slotData.localPosition, slotData.localRotation, __instance.transform.parent, slotData.IsCreatureSlot(), args.ClassId);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"EntitySlotSpawning.Prefix: {e}\n{e.StackTrace}");
            }

            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::EntitySlot), nameof(global::EntitySlot.SpawnVirtualEntities))]
        private static bool EntitySlot_SpawnVirtualEntities(global::EntitySlot __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            EntitySlotSpawningEventArgs args = new EntitySlotSpawningEventArgs(__instance.transform.TransformPoint(__instance.transform.localPosition).GetHashCode());

            Handlers.World.OnEntitySlotSpawning(args);

            if (!args.IsAllowed)
            {
                if (EntitySlot.debugSlots)
                {
                    EntitySlotSpawning.ShowDebugSlot(__instance.transform.localPosition, __instance.transform.localRotation, __instance.transform.parent, __instance.IsCreatureSlot(), args.ClassId, false);
                }

                return false;
            }

            if (!WorldEntityDatabase.TryGetInfo(args.ClassId, out var info) || !SpawnRestrictionEnforcer.ShouldSpawn(args.ClassId))
            {
                return false;
            }

            var virtualEntityPrefab = VirtualEntitiesManager.GetVirtualEntityPrefab();
            virtualEntityPrefab.SetActive(value: false);

            var localPosition = __instance.transform.localPosition;
            var localRotation = __instance.transform.localRotation;

            if (info.prefabZUp)
            {
                localRotation *= Quaternion.Euler(new Vector3(-90f, 0f, 0f));
            }

            var gameObject = UnityEngine.Object.Instantiate(virtualEntityPrefab, localPosition, localRotation);
            gameObject.transform.SetParent(__instance.transform.parent, worldPositionStays: false);
            gameObject.transform.localScale = info.localScale;

            var virtualPrefab = gameObject.GetComponent<VirtualPrefabIdentifier>();
            virtualPrefab.Id      = args.SlotId.ToWorldStreamerId();
            virtualPrefab.ClassId = args.ClassId;

            var component = gameObject.GetComponent<LargeWorldEntity>();
            component.cellLevel = info.cellLevel;

            var active = false;
            if (LargeWorldStreamer.main)
            {
                active = LargeWorldStreamer.main.cellManager.RegisterEntity(component);
            }

            gameObject.SetActive(active);

            if (EntitySlot.debugSlots)
            {
                EntitySlotSpawning.ShowDebugSlot(localPosition, localRotation, __instance.transform.parent, __instance.IsCreatureSlot(), args.ClassId);
            }

            return false;
        }

        /**
         *
         * Slot hata ayıklamasını açar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ShowDebugSlot(Vector3 localPosition, Quaternion localRotation, Transform parentTransform, bool isCreatureSlot, string classId, bool isFound = true)
        {
            var primitive      = GameObject.CreatePrimitive(isCreatureSlot ? PrimitiveType.Sphere : PrimitiveType.Cube);
            primitive.SetActive(false);
            primitive.name = string.Format("{0} ghost (density {1}, id {2})", System.Guid.NewGuid().ToString(), 1, classId);
            primitive.transform.parent        = parentTransform;
            primitive.transform.localPosition = localPosition;
            primitive.transform.localRotation = localRotation;
            primitive.transform.localScale    = isCreatureSlot ? new Vector3(0.5f, 0.5f, 0.5f) : new Vector3(0.2f, 2f, 0.2f);
            primitive.transform.SetParent(null, true);

            UnityEngine.Object.Destroy(primitive.GetComponent<Collider>());
            
            primitive.GetComponent<Renderer>().sharedMaterial = EntitySlot.GetGhostMaterial(isFound);
            primitive.SetActive(true);
        }
    }
}
