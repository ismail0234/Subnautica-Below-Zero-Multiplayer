namespace Subnautica.Events.Patches.Fixes.World
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(WorldStreaming.WorldStreamer), nameof(WorldStreaming.WorldStreamer.ParseClipmapSettings))]
    public class WorldStreamer
    {
        private static void Postfix(ref ClipMapManager.Settings __result)
        {
            if (Network.IsMultiplayerActive)
            {
                if (__result.maxThreads < 8)
                {
                    __result.maxThreads = 8;
                }

                foreach (var item in __result.levels)
                {
                    if (item.colliders)
                    {
                        item.chunksPerSide    = 10;
                        item.chunksVertically = 10;
                    }

                    Log.Info("Level: " + item.colliders + ", chunksPerSide: " + item.chunksPerSide + ", chunksVertically: " + item.chunksVertically + ", Thread: " + __result.maxThreads);
                }
            }
        }
    }
}
