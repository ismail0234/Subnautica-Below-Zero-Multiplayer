namespace Subnautica.Events.Patches.Fixes.Construction
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.API.Extensions;

    using System;
    using System.Collections.Generic;

    using UnityEngine;

    [HarmonyPatch]
    public class ExtraCollision
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Builder), nameof(global::Builder.GetOverlappedColliders), new Type[] { typeof(Vector3), typeof(Quaternion), typeof(Vector3), typeof(int), typeof(QueryTriggerInteraction), typeof(List<Collider>) })]
        private static bool GetOverlappedColliders(Vector3 position, Quaternion rotation, Vector3 extents, int layerMask, QueryTriggerInteraction trigger, List<Collider> results)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            results.Clear();

            int num = UWE.Utils.OverlapBoxIntoSharedBuffer(position, extents, rotation, layerMask, trigger);
            for (int index = 0; index < num; ++index)
            {
                var collider   = UWE.Utils.sharedColliderBuffer[index];
                var gameObject = collider.gameObject;

                if (!collider.isTrigger || gameObject.layer == LayerID.Useable || gameObject.layer == LayerID.Player || gameObject.CompareTag(global::Builder.denyBuildingTag) || CraftData.GetTechType(gameObject).IsCreature())
                {
                    results.Add(collider);
                }
            }

            if (results.Count <= 0)
            {
                int num2 = UWE.Utils.OverlapBoxIntoSharedBuffer(position, extents, rotation, -1, QueryTriggerInteraction.Collide);
                for (int index = 0; index < num2; ++index)
                {
                    var collider = UWE.Utils.sharedColliderBuffer[index];
                    if (collider.gameObject.layer == LayerID.Player)
                    {
                        results.Add(collider);
                    }
                }
            }

            return false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::Builder), nameof(global::Builder.CanDestroyObject))]
        private static void CanDestroyObject(ref bool __result, GameObject go)
        {
            if (Network.IsMultiplayerActive && __result)
            {
                __result = !CraftData.GetTechType(go).IsSynchronizedCreature();
            }
        }
    
        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::Builder), nameof(global::Builder.FindObstacle))]
        private static void FindObstacle(ref GameObject __result, GameObject go)
        {
            if (Network.IsMultiplayerActive && __result == null)
            {
                Transform transform = go.transform;

                for (; transform != null; transform = transform.parent)
                {
                    if (transform.gameObject.layer == LayerID.Player || CraftData.GetTechType(transform.gameObject).IsCreature())
                    {
                        __result = transform.gameObject;
                        break;
                    }
                }
            }
        }
    }
}