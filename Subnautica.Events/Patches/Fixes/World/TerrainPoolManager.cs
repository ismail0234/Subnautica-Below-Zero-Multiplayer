namespace Subnautica.Events.Patches.Fixes.World
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::TerrainPoolManager), nameof(global::TerrainPoolManager.Start))]
    public class TerrainPoolManager
    {
        private static void Postfix(global::TerrainPoolManager __instance)
        {
            if (Network.IsMultiplayerActive && __instance.poolingEnabled)
            {
                __instance.Warm(TerrainChunkPieceType.Collider, 100);
            }
        }
    }
}
