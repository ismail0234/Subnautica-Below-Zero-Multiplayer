namespace Subnautica.Events.Patches.Fixes.World
{
    using System.Collections;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Structures;

    [HarmonyPatch]
    public class DepthAlarms
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::DepthAlarms), nameof(global::DepthAlarms.Start))]
        private static IEnumerator OnPlayerCinematicModeEndx(IEnumerator values, global::DepthAlarms __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                yield return null;

                __instance.conditionRules.AddCondition(() => __instance.crushDamage.GetCanTakeCrushDamage() && __instance.crushDamage.GetDepth() > __instance.crushDamage.crushDepth).WhenBecomesTrue(delegate
                {
                    if (global::Player.main.currentSub)
                    {
                        return;
                    }

                    var uniqueId = Network.Identifier.GetIdentityId(__instance.gameObject, false);
                    if (uniqueId.IsNull())
                    {
                        return;
                    }

                    if (global::Player.main.currentInterior is SeaTruckSegment)
                    {
                        var interiorId = (global::Player.main.currentInterior as global::SeaTruckSegment).GetFirstSegment().gameObject.GetIdentityId();
                        if (interiorId == uniqueId)
                        {
                            __instance.crushDepthNotification.Play(__instance.crushDamage.crushDepth);
                        }
                    }
                    else
                    {
                        if (ZeroVector3.Distance(global::Player.main.transform.position, __instance.transform.position) < 400f)
                        {
                            __instance.crushDepthNotification.Play(__instance.crushDamage.crushDepth);
                        }
                    }
                });
            }
            else
            {
                yield return values;
            }
        }
    }
}