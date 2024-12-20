namespace Subnautica.Events.Patches.Fixes.Game
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.API.Extensions;

    [HarmonyPatch]
    public class FixUniqueIdentifier
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::UniqueIdentifier), nameof(global::UniqueIdentifier.Register))]
        private static bool UniqueIdentifier_Register(global::UniqueIdentifier __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.id.IsNull())
            {
                return false;
            }

            if (global::UniqueIdentifier.identifiers.TryGetValue(__instance.id, out var uniqueIdentifier))
            {
                if (uniqueIdentifier == __instance)
                {
                    return false;
                }

                global::UniqueIdentifier.identifiers[__instance.id] = __instance;
            }
            else
            {
                global::UniqueIdentifier.identifiers.Add(__instance.id, __instance);
            }

            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::UniqueIdentifier), nameof(global::UniqueIdentifier.Unregister))]
        private static bool UniqueIdentifier_Unregister(global::UniqueIdentifier __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.id.IsNull())
            {
                return false;
            }

            if (global::UniqueIdentifier.identifiers.TryGetValue(__instance.id, out var uniqueIdentifier))
            {
                if (uniqueIdentifier == __instance)
                {
                    global::UniqueIdentifier.identifiers.Remove(__instance.id);
                }
                else if (uniqueIdentifier)
                {

                }
                else
                {
                    global::UniqueIdentifier.identifiers.Remove(__instance.id);
                }
            }

            return false;
        }
    }
}
