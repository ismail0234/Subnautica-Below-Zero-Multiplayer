namespace Subnautica.Events.Patches.Events.Items
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::DropTool), nameof(global::DropTool.OnToolUseAnim))]
    public class DroneCameraDeploying
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::DropTool __instance, GUIHand guiHand)
        {
            if (Network.IsMultiplayerActive)
            {
                if (!global::Inventory.CanDropItemHere(__instance.pickupable, true) || guiHand.GetTool() != __instance)
                {
                    return false;
                }

                if (__instance.pickupable.GetTechType() == TechType.MapRoomCamera)
                {
                    try
                    {
                        DroneCameraDeployingEventArgs args = new DroneCameraDeployingEventArgs(__instance.pickupable.gameObject.GetIdentityId(), __instance.pickupable, ZeroGame.FindDropPosition(__instance.GetDropPosition()), MainCameraControl.main.transform.forward * __instance.pushForce);

                        Handlers.Items.OnDroneCameraDeploying(args);

                        return args.IsAllowed;
                    }
                    catch (Exception e)
                    {
                        Log.Error($"DroneCameraDeploying.Prefix: {e}\n{e.StackTrace}");
                    }
                }
            }

            return true;
        }
    }
}


