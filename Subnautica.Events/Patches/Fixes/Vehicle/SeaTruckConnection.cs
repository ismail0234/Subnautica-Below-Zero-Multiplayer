namespace Subnautica.Events.Patches.Fixes.Vehicle
{
    using System.Collections;
    using System.Collections.Generic;

    using HarmonyLib;

    using Subnautica.API.Features;

    using UnityEngine;

    [HarmonyPatch]
    public static class SeaTruckConnection
    {
        /**
         *
         * Zamanlayıcıları barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Dictionary<string, StopwatchItem> Timings { get; set; } = new Dictionary<string, StopwatchItem>();

        /**
         *
         * Zamanlayıcı başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SeaTruckConnection), nameof(global::SeaTruckConnection.Start))]
        private static void SeaTruckConnection_Start(global::SeaTruckConnection __instance)
        {
            if (Network.IsMultiplayerActive && !__instance.initialized)
            {
                GetTimingItem(__instance);
            }
        }

        /**
         *
         * Bağlanma kontrolü yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SeaTruckConnection), nameof(global::SeaTruckConnection.FixedUpdate))]
        private static bool SeaTruckConnection_FixedUpdate(global::SeaTruckConnection __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!__instance.connectionCandidate)
            {
                return false;
            }

            if (!__instance.CanConnect() || !__instance.connectionCandidate.CanConnect() || !global::SeaTruckConnection.ConnectionMatches(__instance.connectionCandidate, __instance) || !__instance.RotationMatches(__instance.connectionCandidate))
            {
                return false;
            }

            var timing = GetTimingItem(__instance);
            if (timing.IsFinished())
            {
                timing.Restart();

                __instance.SetConnectedTo(__instance.connectionCandidate);
            }

            return false;
        }

        /**
         *
         * Zamanlayıcı döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static StopwatchItem GetTimingItem(global::SeaTruckConnection __instance)
        {
            var uniqueId = Network.Identifier.GetIdentityId(__instance.gameObject, false);
            if (Timings.TryGetValue(uniqueId, out var timing))
            {
                return timing;
            }

            Timings[uniqueId] = new StopwatchItem(1000f);
            return Timings[uniqueId];
        }
    }

    [HarmonyPatch]
    public static class SeaTruckConnection_Segment
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::SeaTruckSegment), nameof(global::SeaTruckSegment.Update))]
        private static bool SeaTruckSegment_Update(global::SeaTruckSegment __instance)
        {
            if (!Network.IsMultiplayerActive || !__instance.updateDockedPosition || !__instance.isFrontConnected)
            {
                return true;
            }

            if ((GetTargetPosition(__instance) - __instance.transform.position).sqrMagnitude > 30f)
            {
                __instance.transform.rotation = GetTargetRotation(__instance);
                __instance.transform.position = GetTargetPosition(__instance);
                return false;
            }

            __instance.transform.rotation = Quaternion.Lerp(__instance.transform.rotation, GetTargetRotation(__instance), 7f * Time.deltaTime);
            __instance.transform.position = Vector3.MoveTowards(__instance.transform.position, GetTargetPosition(__instance), Time.deltaTime * 7f);

            if (Vector3.Dot(__instance.transform.forward, __instance.frontConnection.GetConnection().transform.forward) > 0.9f && (__instance.frontConnection.GetConnection().connectionPoint.position - __instance.frontConnection.connectionPoint.position).sqrMagnitude < 0.01f)
            {
                __instance.transform.rotation = GetTargetRotation(__instance);
                __instance.transform.position = GetTargetPosition(__instance);
                __instance.updateDockedPosition = false;

                UWE.CoroutineHost.StartCoroutine(FixSeaTruckConnection(__instance));
            }

            return false;
        }

        /**
         *
         * Bağlantıyı düzeltir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator FixSeaTruckConnection(global::SeaTruckSegment __instance)
        {
            for (int i = 0; i < 10; i++)
            {
                yield return UWE.CoroutineUtils.waitForNextFrame;

                __instance.transform.rotation = GetTargetRotation(__instance);
                __instance.transform.position = GetTargetPosition(__instance);
                __instance.updateDockedPosition = false;
            }
        }

        /**
         *
         * Hedef konumu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Vector3 GetTargetPosition(global::SeaTruckSegment __instance)
        {
            return __instance.frontConnection.GetConnection().connectionPoint.position - __instance.frontConnection.connectionPoint.position + __instance.transform.position;
        }

        /**
         *
         * Hedef açıyı döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Quaternion GetTargetRotation(global::SeaTruckSegment __instance)
        {
            return __instance.frontConnection.GetConnection().transform.rotation;
        }
    }
}