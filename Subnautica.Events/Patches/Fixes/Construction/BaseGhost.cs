namespace Subnautica.Events.Patches.Fixes.Construction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Emit;

    using HarmonyLib;
    
    [HarmonyPatch]
    public static class BaseGhostLastRotation
    {
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(global::BaseAddCellGhost), nameof(global::BaseAddCellGhost.GetCellType))]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return BaseGhostTranspiler.TranspileLastRotation(instructions);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(global::BaseAddMapRoomGhost), nameof(global::BaseAddMapRoomGhost.GetCellType))]
        private static IEnumerable<CodeInstruction> Transpiler2(IEnumerable<CodeInstruction> instructions)
        {
            return BaseGhostTranspiler.TranspileLastRotation(instructions);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(global::BaseAddWallFoundationGhost), nameof(global::BaseAddWallFoundationGhost.GetCellType))]
        private static IEnumerable<CodeInstruction> Transpiler3(IEnumerable<CodeInstruction> instructions)
        {
            return BaseGhostTranspiler.TranspileLastRotation(instructions);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(global::BaseAddCorridorGhost), nameof(global::BaseAddCorridorGhost.CheckCorridorConnection))]
        private static IEnumerable<CodeInstruction> Transpiler4(IEnumerable<CodeInstruction> instructions)
        {
            return BaseGhostTranspiler.TranspileLastRotation(instructions);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(global::BaseAddCorridorGhost), nameof(global::BaseAddCorridorGhost.CalculateCorridorType))]
        private static IEnumerable<CodeInstruction> Transpiler5(IEnumerable<CodeInstruction> instructions)
        {
            return BaseGhostTranspiler.TranspileLastRotation(instructions);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(global::BaseAddModuleGhost), nameof(global::BaseAddModuleGhost.UpdatePlacement))]
        private static IEnumerable<CodeInstruction> Transpiler6(IEnumerable<CodeInstruction> instructions)
        {
            return BaseGhostTranspiler.TranspileLastRotation(instructions);
        }
    }

    [HarmonyPatch]
    public static class BaseGhostAwake
    {
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(global::BaseAddCellGhost), nameof(global::BaseAddCellGhost.Awake))]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return BaseGhostTranspiler.TranspileClampRotation(instructions);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(global::BaseAddCorridorGhost), nameof(global::BaseAddCorridorGhost.Awake))]
        private static IEnumerable<CodeInstruction> Transpiler2(IEnumerable<CodeInstruction> instructions)
        {
            return BaseGhostTranspiler.TranspileClampRotation(instructions);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(global::BaseAddMapRoomGhost), nameof(global::BaseAddMapRoomGhost.Awake))]
        private static IEnumerable<CodeInstruction> Transpiler3(IEnumerable<CodeInstruction> instructions)
        {
            return BaseGhostTranspiler.TranspileClampRotation(instructions);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(global::BaseAddModuleGhost), nameof(global::BaseAddModuleGhost.Awake))]
        private static IEnumerable<CodeInstruction> Transpiler4(IEnumerable<CodeInstruction> instructions)
        {
            return BaseGhostTranspiler.TranspileClampRotation(instructions);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(global::BaseAddWallFoundationGhost), nameof(global::BaseAddWallFoundationGhost.Awake))]
        private static IEnumerable<CodeInstruction> Transpiler5(IEnumerable<CodeInstruction> instructions)
        {
            return BaseGhostTranspiler.TranspileClampRotation(instructions);
        }
    }

    [HarmonyPatch]
    public static class BaseGhostUpdateRotation
    {
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(global::BaseAddCellGhost), nameof(global::BaseAddCellGhost.UpdateRotation))]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return BaseGhostTranspiler.TranspileUpdateRotation(instructions);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(global::BaseAddCorridorGhost), nameof(global::BaseAddCorridorGhost.UpdateRotation))]
        private static IEnumerable<CodeInstruction> Transpiler2(IEnumerable<CodeInstruction> instructions)
        {
            return BaseGhostTranspiler.TranspileUpdateRotation(instructions);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(global::BaseAddMapRoomGhost), nameof(global::BaseAddMapRoomGhost.UpdateRotation))]
        private static IEnumerable<CodeInstruction> Transpiler3(IEnumerable<CodeInstruction> instructions)
        {
            return BaseGhostTranspiler.TranspileUpdateRotation(instructions);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(global::BaseAddWallFoundationGhost), nameof(global::BaseAddWallFoundationGhost.UpdateRotation))]
        private static IEnumerable<CodeInstruction> Transpiler4(IEnumerable<CodeInstruction> instructions)
        {
            return BaseGhostTranspiler.TranspileUpdateRotation(instructions);
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(global::BaseAddModuleGhost), nameof(global::BaseAddModuleGhost.UpdatePlacement))]
        private static IEnumerable<CodeInstruction> Transpiler5(IEnumerable<CodeInstruction> instructions)
        {
            return BaseGhostTranspiler.TranspileUpdateRotation(instructions, true);
        }
    }

    public class BaseGhostTranspiler
    {
        /**
         *
         * LastRotation değerini yamalar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static IEnumerable<CodeInstruction> TranspileLastRotation(IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();
            var index = codes.FindIndex(q => q.opcode == OpCodes.Call && q.operand.ToString().Contains("lastRotation"));

            if (index > -1)
            {
                codes.RemoveRange(index, 1);
                codes.InsertRange(index, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Subnautica.API.MonoBehaviours.BaseGhostRotationComponent), nameof(Subnautica.API.MonoBehaviours.BaseGhostRotationComponent.GetLastRotation), new Type[] { typeof(BaseGhost) }))
                });
            }

            return codes.AsEnumerable();
        }

        /**
         *
         * ClampRotation değerini yamalar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static IEnumerable<CodeInstruction> TranspileClampRotation(IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();
            var index = codes.FindIndex(q => q.opcode == OpCodes.Call && q.operand.ToString().Contains("ClampRotation")); 

            if (index > -1)
            {
                codes.InsertRange(0, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Ldarg_0),
                });

                index++;

                codes.RemoveRange(index, 1);
                codes.InsertRange(index, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Subnautica.API.MonoBehaviours.BaseGhostRotationComponent), nameof(Subnautica.API.MonoBehaviours.BaseGhostRotationComponent.ClampRotation), new Type[] { typeof(BaseGhost), typeof(int) }))
                });
            }

            return codes.AsEnumerable();
        }

        /**
         *
         * UpdateRotation değerini yamalar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static IEnumerable<CodeInstruction> TranspileUpdateRotation(IEnumerable<CodeInstruction> instructions, bool isPlacement = false)
        {
            var codes = instructions.ToList();
            var index  = codes.FindIndex(q => q.opcode == OpCodes.Call && q.operand.ToString().Contains("UpdateRotation"));

            if (index < 0)
            {
                return codes.AsEnumerable();
            }

            if (isPlacement)
            {
                var index2 = codes.FindIndex(q => q.opcode == OpCodes.Ldsfld && q.operand.ToString().Contains("HorizontalDirections"));

                if (index > -1)
                {
                    codes.InsertRange(index2, new CodeInstruction[] {
                        new CodeInstruction(OpCodes.Ldarg_0),
                    });

                    codes.RemoveRange(index + 1, 1);
                    codes.InsertRange(index + 1, new CodeInstruction[] {
                        new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Subnautica.API.MonoBehaviours.BaseGhostRotationComponent), nameof(Subnautica.API.MonoBehaviours.BaseGhostRotationComponent.UpdateRotation), new Type[] { typeof(BaseGhost), typeof(int) }))
                    });
                }

                return codes.AsEnumerable();
            }

            var opCode = codes[index - 1].opcode;
            if (opCode == OpCodes.Callvirt)
            {
                codes.RemoveRange(index, 1);
                codes.InsertRange(index - 2, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Ldarg_0),
                });

                codes.InsertRange(index + 1, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Subnautica.API.MonoBehaviours.BaseGhostRotationComponent), nameof(Subnautica.API.MonoBehaviours.BaseGhostRotationComponent.UpdateRotation), new Type[] { typeof(BaseGhost), typeof(int) }))
                });
            }
            else if (opCode == OpCodes.Conv_I4)
            {
                codes.InsertRange(index - 3, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Ldarg_0),
                });

                codes.RemoveRange(index, 2);
                codes.InsertRange(index, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Subnautica.API.MonoBehaviours.BaseGhostRotationComponent), nameof(Subnautica.API.MonoBehaviours.BaseGhostRotationComponent.UpdateRotation), new Type[] { typeof(BaseGhost), typeof(int) }))
                });
            }
            else 
            {
                codes.RemoveRange(index - 1, 2);
                codes.InsertRange(index - 1, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldc_I4_4),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Subnautica.API.MonoBehaviours.BaseGhostRotationComponent), nameof(Subnautica.API.MonoBehaviours.BaseGhostRotationComponent.UpdateRotation), new Type[] { typeof(BaseGhost), typeof(int) }))
                });
            }

            return codes.AsEnumerable();
        }
    }
}