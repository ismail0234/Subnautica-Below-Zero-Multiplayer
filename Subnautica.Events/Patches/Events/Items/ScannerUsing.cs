namespace Subnautica.Events.Patches.Events.Items
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    using UnityEngine;

    [HarmonyPatch(typeof(global::PDAScanner), nameof(global::PDAScanner.Scan))]
    public class ScannerUsing
    {
        /**
         *
         * Geçen zamanı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static float currentTime = BroadcastInterval.ScannerUsing;

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Postfix(global::PDAScanner.Result __result)
        {
            if (Network.IsMultiplayerActive)
            {
                if (__result == PDAScanner.Result.Scan && global::PDAScanner.scanTarget.isValid)
                {
                    currentTime += Time.deltaTime;

                    if(currentTime >= BroadcastInterval.ScannerUsing)
                    {
                        currentTime = 0f;

                        try
                        {
                            ScannerUsingEventArgs args = new ScannerUsingEventArgs(Network.Identifier.GetIdentityId(global::PDAScanner.scanTarget.gameObject, false));

                            Handlers.Items.OnScannerUsing(args);
                        }
                        catch (Exception e)
                        {
                            Log.Error($"ScannerUsing.Postfix: {e}\n{e.StackTrace}");
                        }
                    }
                }
                else
                {
                    currentTime = BroadcastInterval.ScannerUsing;
                }
            }
        }
    }
}