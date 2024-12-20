namespace Subnautica.Events.Patches.Fixes.Story
{
    using HarmonyLib;

    using UnityEngine;

    [HarmonyPatch(typeof(global::RadioTowerController), nameof(global::RadioTowerController.ReleaseTOM))]
    public static class RadioTower
    {
        private static void Postfix(RadioTowerController __instance)
        {
            if (__instance.insertedItem)
            {
                __instance.insertedItem.transform.position = new Vector3(-213.999f, 53.902f, -722.819f);
                __instance.insertedItem.transform.rotation = new Quaternion(-.001f, -.006f, -.002f, -1f);
            }
        }
    }
}
