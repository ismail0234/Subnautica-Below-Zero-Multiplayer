namespace Subnautica.Events.Patches.Events.World
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::CrushDamage), nameof(global::CrushDamage.CrushDamageUpdate))]
    public static class CrushDamaging
    {
        private static bool Prefix(global::CrushDamage __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                if (!__instance.gameObject.activeInHierarchy || !__instance.enabled || !__instance.GetCanTakeCrushDamage() || __instance.GetDepth() <= __instance.crushDepth)
                {
                    return false;
                }

                try
                {
                    CrushDamagingEventArgs args = new CrushDamagingEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject, false), CraftData.GetTechType(__instance.gameObject), __instance.liveMixin.data.maxHealth, __instance.damagePerCrush);

                    Handlers.World.OnCrushDamaging(args);

                    return args.IsAllowed;
                }
                catch (Exception e)
                {
                    Log.Error($"CrushDamage.Prefix: {e}\n{e.StackTrace}");
                }
            }

            return true;
        }
    }
}