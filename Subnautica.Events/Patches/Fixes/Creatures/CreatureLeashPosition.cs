namespace Subnautica.Events.Patches.Fixes.Creatures
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Emit;

    using HarmonyLib;

    [HarmonyPatch]
    public static class CreatureLeashPosition
    {
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(global::FleeOnDamage), nameof(global::FleeOnDamage.StopPerform))]
        private static IEnumerable<CodeInstruction> FleeOnDamage(IEnumerable<CodeInstruction> instructions)
        {
            return Transpile(instructions, 1);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(global::AttachToVehicle), nameof(global::AttachToVehicle.StopPerform))]
        private static IEnumerable<CodeInstruction> AttachToVehicle(IEnumerable<CodeInstruction> instructions)
        {
            return Transpile(instructions, 1);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(global::CreatureRiding), nameof(global::CreatureRiding.ManagedUpdate))]
        private static IEnumerable<CodeInstruction> CreatureRiding(IEnumerable<CodeInstruction> instructions)
        {
            return Transpile(instructions, 1);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(global::AvoidInteriors), nameof(global::AvoidInteriors.Perform))]
        private static IEnumerable<CodeInstruction> AvoidInteriors(IEnumerable<CodeInstruction> instructions)
        {
            return Transpile(instructions, 2);
        }

        public static IEnumerable<CodeInstruction> Transpile(IEnumerable<CodeInstruction> instructions, byte type)
        {
            var codes = instructions.ToList();
            var index = codes.FindIndex(q => q.opcode == OpCodes.Stfld && q.operand.ToString().Contains("leashPosition"));

            if (index > -1)
            {
                switch (type)
                {
                    case 1: codes.RemoveRange(index - 5, 6); break;
                    case 2: codes.RemoveRange(index - 3, 4); break;
                }
            }

            return codes.AsEnumerable();
        }
    }
}
