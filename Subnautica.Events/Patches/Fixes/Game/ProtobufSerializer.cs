namespace Subnautica.Events.Patches.Fixes.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Emit;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Client.Extensions;

    [HarmonyPatch]
    public static class ProtobufSerializer
    {
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(global::ProtobufSerializer), nameof(global::ProtobufSerializer.SerializeGameObject))]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return ProtobufSerializer.TranspileSerializeGameObject(instructions);
        }

        /**
         *
         * Transpiler uygular.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static IEnumerable<CodeInstruction> TranspileSerializeGameObject(IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();
            var index = codes.FindLastIndex(q => q.opcode == OpCodes.Callvirt && q.operand.ToString().Contains("set_Id"));

            if (index > -1)
            {
                codes.InsertRange(index, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ProtobufSerializer), nameof(ProtobufSerializer.GetSerializeGameObjectId), new Type[] { typeof(global::ProtobufSerializer), typeof(string) }))
                });
                
                codes.InsertRange(index - 2, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Ldarg_0)
                });

                codes.InsertRange(index + 10, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Ldarg_0)
                });

                codes.InsertRange(index + 14, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ProtobufSerializer), nameof(ProtobufSerializer.GetSerializeGameObjectParentId), new Type[] { typeof(global::ProtobufSerializer), typeof(global::UniqueIdentifier), typeof(bool), }))
                });

                codes.RemoveRange(index + 13, 1);
            }

            return codes.AsEnumerable();
        }

        /**
         *
         * UniqueId Değerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetSerializeGameObjectId(global::ProtobufSerializer serializer, string id)
        {
            if (Network.IsMultiplayerActive && serializer.IsIdIgnoreModeActive())
            {
                try
                {
                    return Serializer.GetUniqueId(id);
                }
                catch (Exception ex)
                {
                    Log.Error($"Transpiler, GetSerializeGameObject Exception: {ex}");
                }
            }

            return id;
        }

        /**
         *
         * ParentId Değerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetSerializeGameObjectParentId(global::ProtobufSerializer serializer, global::UniqueIdentifier uid, bool useParent)
        {
            if (!Network.IsMultiplayerActive || !useParent || !serializer.IsIdIgnoreModeActive())
            {
                return global::ProtobufSerializer.GetParentId(uid, useParent);
            }

            var parent = uid.transform.parent;
            if (parent == null)
            {
                return null;
            }

            var component = parent.GetComponent<global::UniqueIdentifier>();
            if (component == null)
            {
                return null;
            }

            try
            {
                return Serializer.GetUniqueId(component.Id);
            }
            catch (Exception ex)
            {
                Log.Error($"Transpiler, GetSerializeGameObjectParentId Exception: {ex}");
            }
            
            return null;
        }
    }
}