namespace Subnautica.Events.Patches.Events.Player
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(PDAScanner), nameof(PDAScanner.Scan))]
    public static class EntityScannerCompleted
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static string UniqueId { get; set; } = null;

        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static TechType TechType { get; set; } = TechType.None;

        /**
         *
         * Önek Yaması
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Prefix()
        {
            if (Network.IsMultiplayerActive)
            {
                UniqueId = PDAScanner.scanTarget.uid;
                TechType = PDAScanner.scanTarget.techType;
            }
        }

        /**
         *
         * Sonek Yaması
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Postfix(ref PDAScanner.Result __result)
        {
            if (Network.IsMultiplayerActive)
            {
                if (__result == PDAScanner.Result.Done || __result == PDAScanner.Result.Researched)
                {
                    try
                    {
                        EntityScannerCompletedEventArgs args = new EntityScannerCompletedEventArgs(UniqueId, TechType);

                        Handlers.Player.OnEntityScannerCompleted(args);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"EntityScannerCompleted.Postfix: {e}\n{e.StackTrace}");
                    }
                }
            }
        }
    }
}