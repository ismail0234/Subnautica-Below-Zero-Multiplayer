namespace Subnautica.Events.Patches.Fixes.Items
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Emit;

    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch]
    public static class Flare
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Flare), nameof(global::Flare.Update))]
        private static bool Flare_Update(global::Flare __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                if (__instance.usingPlayer)
                {
                    return true;
                }

                if (__instance.flareActivateTime == -99f)
                {
                    return false;
                }
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Flare), nameof(global::Flare.GetWindBurnDownScalar))]
        private static bool Flare_GetWindBurnDownScalar(global::Flare __instance, ref float __result)
        {
            if (Network.IsMultiplayerActive && !__instance.usingPlayer)
            {
                __result = 0f;
                return false;
            }

            return true;
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(global::Flare), nameof(global::Flare.Update))]
        public static IEnumerable<CodeInstruction> Flare_Update_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();
            var index = codes.FindLastIndex(q => q.opcode == OpCodes.Ldfld && q.operand.ToString().Contains("energyLeft"));

            if (index > -1)
            {
                codes.RemoveRange(index + 1, 1);
                codes.InsertRange(index + 1, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Ldc_R4, -55555f),
                });
            }

            return codes.AsEnumerable();
        }
    }
}
