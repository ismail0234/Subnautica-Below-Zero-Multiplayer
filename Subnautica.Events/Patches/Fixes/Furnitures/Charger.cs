namespace Subnautica.Events.Patches.Fixes.Furnitures
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::Charger), nameof(global::Charger.Update))]
    public class Charger
    {
        private static bool Prefix(global::Charger __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            __instance.sequence.Update();
            return false;
        }
    }
}
