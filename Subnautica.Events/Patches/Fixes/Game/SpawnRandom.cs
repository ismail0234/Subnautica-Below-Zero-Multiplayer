namespace Subnautica.Events.Patches.Fixes.Game
{
    using System.Collections;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.API.MonoBehaviours;

    [HarmonyPatch(typeof(global::SpawnRandom), nameof(global::SpawnRandom.Start))]
    public static class SpawnRandom
    {
        private static IEnumerator Postfix(IEnumerator values, global::SpawnRandom __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                var uniqueId = Network.Identifier.GetIdentityId(__instance.gameObject, false);
                var task = AddressablesUtility.InstantiateAsync(__instance.assetReferences[0].RuntimeKey as string, __instance.transform.parent, __instance.transform.localPosition, __instance.transform.localRotation, true);

                yield return task;

                var result = task.GetResult();
                if (result)
                {
                    Network.Identifier.SetIdentityId(result, uniqueId);
                }

                if (__instance.TryGetComponent<SpawnPointComponent>(out var spawnPoint))
                {
                    result.EnsureComponent<SpawnPointComponent>().SetSpawnPoint(spawnPoint.SpawnPoint);
                }

                UnityEngine.Object.Destroy(__instance.gameObject);
            }
            else
            {
                yield return values;
            }
        }
    }
}
