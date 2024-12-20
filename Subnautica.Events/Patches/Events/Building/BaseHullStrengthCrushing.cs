namespace Subnautica.Events.Patches.Events.Building
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::BaseHullStrength), nameof(global::BaseHullStrength.CrushDamageUpdate))]
    public class BaseHullStrengthCrushing
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::BaseHullStrength __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!GameModeManager.GetOption<bool>(GameOption.BaseWaterPressureDamage) || __instance.totalStrength >= 0.0f || __instance.victims.Count <= 0)
            {
                return false;
            }

            try
            {
                BaseHullStrengthCrushingEventArgs args = new BaseHullStrengthCrushingEventArgs(__instance);

                Handlers.Building.OnBaseHullStrengthCrushing(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"BaseHullStrengthCrushing.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}
