namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::Bench), nameof(global::Bench.OnHandClick))]
    public static class BenchSitdown
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::Bench __instance, global::GUIHand hand)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            var constructable = __instance.GetComponentInParent<Constructable>();
            if (constructable == null)
            {
                return false;
            }

            __instance.isValidHandTarget = __instance.GetCanBeUsedBy(hand.player);
            if (!__instance.isValidHandTarget)
            {
                return false;
            }

            var side = __instance.GetSide(hand.player);
            if (side == Bench.BenchSide.None)
            {
                ErrorMessage.AddWarning(global::Language.main.Get("NotEnoughSpaceToSit"));
                return false;
            }

            try
            {
                BenchSitdownEventArgs args = new BenchSitdownEventArgs(Network.Identifier.GetIdentityId(constructable.gameObject), side, constructable.techType);

                Handlers.Furnitures.OnBenchSitdown(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"Furnitures.BenchSitdown: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}