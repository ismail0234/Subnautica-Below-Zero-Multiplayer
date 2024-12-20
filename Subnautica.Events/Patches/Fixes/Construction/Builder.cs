namespace Subnautica.Events.Patches.Fixes.Construction
{
    using System.Collections.Generic;

    using HarmonyLib;

    using Subnautica.API.Features;

    using UnityEngine;

    [HarmonyPatch]
    public class Builder
    {
        [HarmonyPatch(typeof(global::Builder), nameof(global::Builder.GetRootObjects))]
        private static bool Prefix(List<Collider> colliders, List<GameObject> results)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            for (int i = 0; i < colliders.Count; i++)
            {
                var gameObject = colliders[i].gameObject;
                if (gameObject && gameObject.TryGetComponent<BaseDeconstructable>(out var baseDeconstructable) && baseDeconstructable.recipe == TechType.BasePartition)
                {
                    continue;
                }

                var transform = gameObject.transform;
                while (transform != null)
                {
                    if (transform.GetComponent<IBaseModuleGeometry>() != null)
                    {
                        gameObject = transform.gameObject;
                        break;
                    }

                    if (transform.GetComponent<PrefabIdentifier>() != null)
                    {
                        gameObject = transform.gameObject;
                        break;
                    }

                    if (transform.GetComponent<SceneObjectIdentifier>() != null)
                    {
                        gameObject = transform.gameObject;
                        break;
                    }

                    transform = transform.parent;
                }

                if (!results.Contains(gameObject))
                {
                    results.Add(gameObject);
                }
            }

            return false;
        }
    }
}
