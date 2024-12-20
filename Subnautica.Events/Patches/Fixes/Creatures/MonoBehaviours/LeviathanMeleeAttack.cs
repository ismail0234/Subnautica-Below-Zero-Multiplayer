namespace Subnautica.Events.Patches.Fixes.Creatures.MonoBehaviours
{
    using Subnautica.API.Features;
    using Subnautica.API.Extensions;

    using HarmonyLib;

    using System.Collections.Generic;
    using System.Reflection.Emit;
    using System.Linq;

    [HarmonyPatch]
    public class LeviathanMeleeAttack
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::LeviathanMeleeAttack), nameof(global::LeviathanMeleeAttack.ReleaseVehicle))]
        private static void ReleaseVehicle_Postfix(global::LeviathanMeleeAttack __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                if (Network.Creatures.IsMine(__instance.gameObject))
                {
                    __instance.useRigidbody.SetNonKinematic();
                }
                else
                {
                    __instance.useRigidbody.SetKinematic();
                }
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::LeviathanMeleeAttack), nameof(global::LeviathanMeleeAttack.ReleaseVehicle))]
        private static void ReleaseVehicle_Prefix(global::LeviathanMeleeAttack __instance)
        {
            if (__instance.holdingVehicle)
            {
                if (!Network.IsMultiplayerActive || __instance.heldSeatruck?.IsPiloted() == true || global::Player.main.playerAnimator.GetBool(__instance.playerStartSeatruckAttackAnimation))
                {
                    global::Player.main.playerAnimator.SetBool(__instance.playerStartSeatruckAttackAnimation, false);
                    global::Player.main.playerAnimator.SetBool(__instance.playerEndSeatruckAttackAnimation, false);
                }
            }
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(global::LeviathanMeleeAttack), nameof(global::LeviathanMeleeAttack.ReleaseVehicle))]
        private static IEnumerable<CodeInstruction> ReleaseVehicle_Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var codes  = instructions.ToList();
            
            var index  = codes.FindIndex(q => q.opcode == OpCodes.Ldfld && q.operand.ToString().Contains("playerAnimator"));
            if (index > -1)
            {
                codes.RemoveRange(index - 1, 12);
            }

            var index2 = codes.FindLastIndex(q => q.opcode == OpCodes.Ldfld && q.operand.ToString().Contains("exosuitAttackLoopSfx"));
            if (index2 > -1)
            {
                var label = il.DefineLabel();

                codes[index2 - 1].labels.Add(label);
                codes[index - 20] = new CodeInstruction(OpCodes.Brfalse_S, label);
            }

            return codes.AsEnumerable();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::LeviathanMeleeAttack), nameof(global::LeviathanMeleeAttack.FinishSeatruckAttack))]
        private static bool FinishSeatruckAttack(global::LeviathanMeleeAttack __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            __instance.animator.SetBool("seatruck_attack_end", true);
            __instance.seatruckAttackEndSfx.Play();

            if (__instance.heldSeatruck)
            {
                __instance.heldSeatruck.animator.SetBool(__instance.seatruckEndAttackAnimation, true);

                if (__instance.heldSeatruck.IsPiloted())
                {
                    global::Player.main.playerAnimator.SetBool(__instance.playerEndSeatruckAttackAnimation, true);
                }
            }

            return false;
        }
    }
}
