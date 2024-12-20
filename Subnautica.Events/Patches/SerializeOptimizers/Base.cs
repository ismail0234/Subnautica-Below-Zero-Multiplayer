namespace Subnautica.Events.Patches.SerializeOptimizers
{
    using HarmonyLib;

    using ProtoBuf;
    
    using Subnautica.Network.Models.Construction.Shared;
    using Subnautica.API.Features;
    using Subnautica.Network.Core;

    [HarmonyPatch]
    public class Base
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::ProtobufSerializerPrecompiled), nameof(global::ProtobufSerializerPrecompiled.Serialize1966747463))]
        private static bool Serialize(global::ProtobufSerializerPrecompiled __instance, global::Base obj, int objTypeId, ProtoWriter writer)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            ProtoWriter.WriteFieldHeader(10, WireType.String, writer);
            ProtoWriter.WriteBytes(BaseComponent.Serialize(obj), writer);
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::ProtobufSerializerPrecompiled), nameof(global::ProtobufSerializerPrecompiled.Deserialize1966747463))]
        private static bool Deserialize(global::ProtobufSerializerPrecompiled __instance, global::Base obj, ProtoReader reader)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            for (int num = reader.ReadFieldHeader(); num > 0; num = reader.ReadFieldHeader())
            {
                switch (num)
                {
                    case 1:
                        SubItemToken token2 = ProtoReader.StartSubItem(reader);
                        obj.baseShape = __instance.Deserialize997267884(obj.baseShape, reader);
                        ProtoReader.EndSubItem(token2, reader);
                        return true;
                    case 10:

                        
                        var component = NetworkTools.Deserialize<BaseComponent>(ProtoReader.AppendBytes(null, reader));
                        if (component != null)
                        {
                            component.ImportToBase(obj);
                            component.ShowDetails();
                        }

                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }

            return false;
        }
    }
}
