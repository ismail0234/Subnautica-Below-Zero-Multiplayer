namespace Subnautica.Events.Patches.Fixes.Creatures.MonoBehaviours
{
    using System.Collections;

    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch]
    public class VoidLeviathan
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::VoidLeviathan), nameof(global::VoidLeviathan.VoidBehaviourUpdate))]        
        private static IEnumerator VoidBehaviourUpdate(IEnumerator values)
        {
            if (Network.IsMultiplayerActive)
            {
                yield break;
            }
            else
            {
                yield return values;
            }
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::VoidLeviathansSpawner), nameof(global::VoidLeviathansSpawner.UpdateSpawn))]        
        private static bool UpdateSpawn()
        {
            return !Network.IsMultiplayerActive;
        }
    }
}