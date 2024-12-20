namespace Subnautica.Events.Patches.Fixes.Items
{
    using HarmonyLib;

    using Subnautica.API.Features;

    using System.Collections;

    using UnityEngine;

    [HarmonyPatch]
    public static class SpyPenguin
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SpyPenguin), nameof(global::SpyPenguin.UpdateDeployed))]
        private static bool UpdateDeployed(global::SpyPenguin __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            var entity = Network.DynamicEntity.GetEntity(Network.Identifier.GetIdentityId(__instance.gameObject, false));
            if (entity.IsMine(ZeroPlayer.CurrentPlayer.UniqueId))
            {
                return true;
            }

            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SpyPenguin), nameof(global::SpyPenguin.InitSelfDestructSequence))]
        private static bool InitSelfDestructSequence(global::SpyPenguin __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            return GameModeManager.GetOption<float>(GameOption.TechDamageTakenModifier) != 0f;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::SpyPenguin), nameof(global::SpyPenguin.CO_SelfDestructSequence))]
        private static IEnumerator CO_SelfDestructSequence(IEnumerator values, global::SpyPenguin __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                yield return new WaitForSeconds(1.8f);

                __instance.liveMixin.TakeDamage(1000f, __instance.transform.position, DamageType.Starve);
            }
            else
            {
                yield return values;
            }
        }
    }
}
