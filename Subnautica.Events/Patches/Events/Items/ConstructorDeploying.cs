namespace Subnautica.Events.Patches.Events.Items
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using UnityEngine;

    [HarmonyPatch(typeof(global::Constructor), nameof(global::Constructor.OnRightHandDown))]
    public class ConstructorDeploying
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::Constructor __instance)
        {
            if (Network.IsMultiplayerActive && !EventBlocker.IsEventBlocked(TechType.Constructor))
            {
                if (!global::Player.main.IsUnderwaterForSwimming() || global::Player.main.IsInSub() || global::Player.main.forceWalkMotorMode)
                {
                    ErrorMessage.AddMessage(global::Language.main.Get("DeployConstructorInWater"));
                    return false;
                }

                if (PrecursorMoonPoolTrigger.inMoonpool || PrisonManager.IsInsideAquarium(__instance.transform.position))
                {
                    ErrorMessage.AddMessage(global::Language.main.Get("DeployConstructorInPrecursor"));
                    return false;
                }

                try
                {
                    ConstructorDeployingEventArgs args = new ConstructorDeployingEventArgs(Network.Identifier.GetIdentityId(__instance.pickupable.gameObject), __instance.pickupable, MainCamera.camera.transform.forward, ZeroGame.FindDropPosition(__instance.transform.position + MainCamera.camera.transform.forward * 0.7f + Vector3.down * 0.3f));

                    Handlers.Items.OnConstructorDeploying(args);

                    return args.IsAllowed;
                }
                catch (Exception e)
                {
                    Log.Error($"ConstructorDeploying.Prefix: {e}\n{e.StackTrace}");
                }
            }

            return true;
        }
    }
}