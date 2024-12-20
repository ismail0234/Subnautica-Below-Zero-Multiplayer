namespace Subnautica.Events.Patches.SerializeOptimizers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Client.Extensions;

    using UWE;

    [HarmonyPatch(typeof(global::ProtobufSerializer), nameof(global::ProtobufSerializer.SerializeObjectsAsync))]
    public static class ProtobufOptimizer
    {
        /**
         *
         * Whitelist isim listesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static List<string> WhitelistNames { get; set; } = new List<string>()
        {
            "NuclearReactor",
            "BioReactor",
            "FiltrationMachine",
            "MapRoomFunctionality",
            "ControlRoomModule",
            "BaseCell",
            "dockingPoint",
            "BaseUpgradeConsoleModule",
        };

        /**
         *
         * Serilize edilme durumu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool IsSerializable(UniqueIdentifier uniqueIdentifier)
        {
            if (ProtobufOptimizer.WhitelistNames.Where(q => uniqueIdentifier.name.Contains(q)).Any())
            {
                return true;
            }

            if (Network.IsExistsConstructionInServer(uniqueIdentifier.Id))
            {
                return true;
            }

            if (uniqueIdentifier.GetComponent<global::Base>())
            {
                return true;
            }

            if (uniqueIdentifier is global::StoreInformationIdentifier)
            {
                return true;
            }

            return false;
        }

        /**
         *
         * Sınıfı yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator Postfix(IEnumerator values, global::ProtobufSerializer __instance, Stream stream, IList<UniqueIdentifier> uids, bool storeParent)
        {
            if (Network.IsMultiplayerActive && __instance.IsConstructionModeActive())
            {
                using (PooledObject<ProtobufSerializer.LoopHeader> proxy = ProtobufSerializer.loopHeaderPool.GetProxy<ProtobufSerializer.LoopHeader>())
                {
                    ProtobufSerializer.LoopHeader source = proxy.Value;
                    source.Reset();
                    source.Count = uids.Count;

                    __instance.Serialize<ProtobufSerializer.LoopHeader>(stream, source);
                }

                for (int i = 0; i < uids.Count; ++i)
                {
                    UniqueIdentifier uid = uids[i];
                    if (!uid)
                    {
                        uid = ProtobufSerializer.CreateTemporaryGameObject("DESTROYED OBJECT");
                    }

                    if (ProtobufOptimizer.IsSerializable(uid))
                    {
                        __instance.SerializeGameObject(stream, uid, storeParent);
                    }

                    yield return null;
                }
            }
            else
            {
                yield return values;
            }
        }
    }
}
