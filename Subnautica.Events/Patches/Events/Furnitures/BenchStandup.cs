namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::Bench), nameof(global::Bench.ExitSittingMode))]
    public static class BenchStandup
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Prefix(global::Bench __instance, global::Player player, bool skipCinematics = false)
        {
            if (Network.IsMultiplayerActive)
            {
                var constructable = __instance.GetComponentInParent<Constructable>();
                if (constructable == null)
                {
                    return;
                }

                try
                {
                    Bench.BenchSide side = Bench.BenchSide.Front;
                    if(__instance.animator.transform.localEulerAngles == __instance.backAnimRotation)
                    {
                        side = Bench.BenchSide.Back;
                    }

                    BenchStandupEventArgs args = new BenchStandupEventArgs(Network.Identifier.GetIdentityId(constructable.gameObject), side, constructable.techType);

                    Handlers.Furnitures.OnBenchStandup(args);
                }
                catch (Exception e)
                {
                    Log.Error($"Furnitures.BenchStandup: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}