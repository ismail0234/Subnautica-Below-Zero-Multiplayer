namespace Subnautica.Events.Patches.Events.Items
{
    using System;

    using HarmonyLib;
    using Subnautica.API.Features;

    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::SpyPenguin), nameof(global::SpyPenguin.TryUse))]
    public static class SpyPenguinItemGrabing
    {

        private static void Prefix(SpyPenguin __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                var animationName = GetAnimationName(__instance);
                if (string.IsNullOrEmpty(animationName))
                {
                    return;
                }

                try
                {
                    SpyPenguinItemGrabingEventArgs args = new SpyPenguinItemGrabingEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject), animationName);

                    Handlers.Items.OnSpyPenguinItemGrabing(args);
                }
                catch (Exception e)
                {
                    Log.Error($"SpyPenguinItemGrabing.Prefix: {e}\n{e.StackTrace}");
                }
            }

        }

        /**
         *
         * Animasyon adını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static string GetAnimationName(SpyPenguin penguin)
        {
            if (penguin.activeTarget == null && !SpyPenguin.IsViewingPlayer(penguin.cameraTransform.position, MainCamera.camera.transform.forward, 10f))
            {
                return "arm_grab_fail";
            }

            if (penguin.activeTarget != null)
            {
                switch (penguin.activeTarget.OnPenguinClick(penguin))
                {
                    case SpyPenguinTargetActionType.Grab:

                        if (!penguin.pickupReady)
                        {
                            break;
                        }

                        return "arm_grab";
                    case SpyPenguinTargetActionType.Punch:  return "arm_punch";
                }
            }

            return null;
        }
    }
}
