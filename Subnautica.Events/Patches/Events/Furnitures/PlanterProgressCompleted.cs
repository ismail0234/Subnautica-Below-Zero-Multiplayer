namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;
    using System.Collections;
    using System.ComponentModel;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.EventSystems;

    using UWE;

    [HarmonyPatch(typeof(global::GrowingPlant), nameof(global::GrowingPlant.SpawnGrownModelAsync))]
    public static class PlanterProgressCompleted
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator Postfix(IEnumerator values, global::GrowingPlant __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                GrowingPlant behaviour = __instance;

                BehaviourUpdateUtils.DeregisterFromUpdate(behaviour);

                CoroutineTask<GameObject> task = AddressablesUtility.InstantiateAsync((behaviour.grownModelPrefab).RuntimeKey as string, position: behaviour.growingTransform.position, rotation: behaviour.growingTransform.rotation, awake: false);
                yield return task;

                GameObject result = task.GetResult();
                behaviour.growingTransform.gameObject.SetActive(false);
                behaviour.SetScale(result.transform, 1f);

                try
                {
                    PlanterProgressCompletedEventArgs args = new PlanterProgressCompletedEventArgs(behaviour.seed, result);

                    Handlers.Furnitures.OnPlanterProgressCompleted(args);
                }
                catch (Exception e)
                {
                    Log.Error($"PlanterProgressCompleted.Postfix: {e}\n{e.StackTrace}");
                }

                if (result.GetComponent<Pickupable>() != null)
                {
                    var seedUniqueId = Network.Identifier.GetIdentityId(behaviour.seed.pickupable.gameObject, false);

                    Plantable component = result.GetComponent<Plantable>();
                    if (component != null && behaviour.seed.ReplaceSeedByPlant(component))
                    {
                        Network.Identifier.SetIdentityId(component.gameObject, seedUniqueId);

                        yield break;
                    }
                }

                result.SetActive(true);

                GrownPlant grownPlant = result.AddComponent<GrownPlant>();
                grownPlant.seed = behaviour.seed;
                grownPlant.SendMessage("OnGrown", SendMessageOptions.DontRequireReceiver);

                if (behaviour.seed != null)
                {
                    result.transform.parent = behaviour.seed.currentPlanter.grownPlantsRoot;
                    behaviour.seed.currentPlanter.SetupRenderers(result, true);
                    behaviour.seed.currentPlanter.SetupLighting(result);
                }

                behaviour.enabled = false;
            }
            else
            {
                yield return values;
            }
        }
    }
}