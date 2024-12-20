namespace Subnautica.Events.Patches.Events.Items
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::OxygenPipe), nameof(global::OxygenPipe.OnRightHandDown))]
    public class OxygenPipePlacing
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::OxygenPipe __instance)
        {
            if (Network.IsMultiplayerActive && !global::Player.main.IsSpikyTrapAttached())
            {
                if (OxygenPipe.ghostModel.GetParent() == null || OxygenPipe.ghostModel.GetParent().GetRoot() == null)
                {
                    return false;
                }

                try
                {
                    OxygenPipePlacingEventArgs args = new OxygenPipePlacingEventArgs(OxygenPipe.ghostModel.GetRoot().GetGameObject().GetIdentityId(), OxygenPipe.ghostModel.GetParent().GetGameObject().GetIdentityId(), __instance.pickupable.gameObject.GetIdentityId(), __instance.pickupable, OxygenPipe.ghostModel.transform.position, __instance.transform.rotation);

                    Handlers.Items.OnOxygenPipePlacing(args);

                    return args.IsAllowed;
                }
                catch (Exception e)
                {
                    Log.Error($"OxygenPipePlacing.Prefix: {e}\n{e.StackTrace}");
                }
            }

            return true;
        }
    }
}