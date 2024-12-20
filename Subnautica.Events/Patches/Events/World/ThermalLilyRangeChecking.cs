namespace Subnautica.Events.Patches.Events.Storage
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;
    using UnityEngine;

    [HarmonyPatch(typeof(global::ThermalLily), nameof(global::ThermalLily.IsPlayerClose))]
    public static class ThermalLilyRangeChecking
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::ThermalLily __instance, ref bool __result)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            try
            {
                ThermalLilyRangeCheckingEventArgs args = new ThermalLilyRangeCheckingEventArgs(__instance.transform.position, __instance.playerRange);

                Handlers.World.OnThermalLilyRangeChecking(args);

                __result = args.IsPlayerInRange;

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"ThermalLilyRangeChecking.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }


    [HarmonyPatch(typeof(global::ThermalLily), nameof(global::ThermalLily.UpdateAnimationAngles))]
    public static class ThermalLilyAnimationAnglesChecking
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::ThermalLily __instance, float deltaTime)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.state == ThermalLily.State.Sleep || __instance.state == ThermalLily.State.TransitionToSleep || __instance.state != ThermalLily.State.FollowPlayer)
            {
                return true;
            }

            try
            {
                ThermalLilyAnimationAnglesCheckingEventArgs args = new ThermalLilyAnimationAnglesCheckingEventArgs(__instance.transform.position, __instance.playerRange);

                Handlers.World.OnThermalLilyAnimationAnglesChecking(args);

                if (args.IsAllowed)
                {
                    return true;
                }

                Vector3 zero = Vector3.zero;
                Vector3 vector3 = __instance.defaultHeadDirection.InverseTransformDirection((args.PlayerPosition - __instance.headTransform.position).normalized);
                if (vector3.y < 0.0)
                {
                    vector3.y = 0.0f;
                    vector3 = vector3.normalized;
                }

                float num1 = Mathf.Asin(vector3.x);
                float num2 = (float)(-(double)Mathf.Asin(vector3.z) - 0.785398185253143);

                __instance.animator.SetFloat(ThermalLily.animHorizontalAngle, num1 / 1.570796f, 1f, deltaTime);
                __instance.animator.SetFloat(ThermalLily.animVerticalAngle, num2 / 1.570796f, 1f, deltaTime);

                bool flag = (double) Vector3.Angle(__instance.headLight.transform.forward, __instance.lastForward) > 0.600000023841858;
                if (flag && !__instance.sfx_Move.playing)
                {
                    __instance.sfx_Move.Play();
                }
                else if (!flag && __instance.sfx_Move.playing)
                {
                    __instance.sfx_Move.Stop();
                }
                    
                __instance.lastForward = __instance.headLight.transform.forward;

                return false;
            }
            catch (Exception e)
            {
                Log.Error($"ThermalLilyAnimationAnglesChecking.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}