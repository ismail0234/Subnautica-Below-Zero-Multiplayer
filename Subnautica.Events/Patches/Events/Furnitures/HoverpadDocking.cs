namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;
    using System.Collections.Generic;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using UnityEngine;

    [HarmonyPatch]
    public static class HoverpadDocking
    {
        /**
         *
         * StopwatchItem nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static readonly Dictionary<string, StopwatchItem> StopwatchItems = new Dictionary<string, StopwatchItem>();

        /**
         *
         * Yamalar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Hoverpad), nameof(global::Hoverpad.OnEnable))]
        private static void Hoverpad_OnEnable(global::Hoverpad __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                var uniqueId = GetUniqueId(__instance.gameObject);
                if (uniqueId.IsNotNull())
                {
                    StopwatchItems[uniqueId] = new StopwatchItem(BroadcastInterval.VehicleDocking);
                }
            }
        }

        /**
         *
         * Yamalar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Hoverpad), nameof(global::Hoverpad.OnDisable))]
        private static void Hoverpad_OnDisable(global::Hoverpad __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                var uniqueId = GetUniqueId(__instance.gameObject);
                if (uniqueId.IsNotNull())
                {
                    StopwatchItems.Remove(uniqueId);
                }
            }
        }

        /**
         *
         * Yamalar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Hoverpad), nameof(global::Hoverpad.TryDockBike))]
        private static bool Hoverpad_TryDockBike(global::Hoverpad __instance, Hoverbike hc)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.isBikeDocked || __instance.isConstructing || !hc || hc.rb.velocity.magnitude >= __instance.maxDockSpeed)
            {
                return false;
            }

            var uniqueId = GetUniqueId(__instance.gameObject);
            if (uniqueId.IsNull())
            {
                return false;
            }

            if (!StopwatchItems.TryGetValue(uniqueId, out var stopwatchItem))
            {
                return false;
            }

            if (!stopwatchItem.IsFinished())
            {
                return false;
            }

            stopwatchItem.Restart();

            try
            {
                HoverpadDockingEventArgs args = new HoverpadDockingEventArgs(uniqueId, Network.Identifier.GetIdentityId(hc.gameObject));

                Handlers.Furnitures.OnHoverpadDocking(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"HoverpadDocking.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }

        /**
         *
         * UniqueId değerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static string GetUniqueId(GameObject gameObject)
        {
            return Network.Identifier.GetIdentityId(gameObject);
        }
    }
}