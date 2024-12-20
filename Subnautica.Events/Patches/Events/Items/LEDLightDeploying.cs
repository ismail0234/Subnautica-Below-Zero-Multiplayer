namespace Subnautica.Events.Patches.Events.Items
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::PlaceTool), nameof(global::PlaceTool.Place))]
    public class LEDLightDeploying
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::PlaceTool __instance)
        {
            if (Network.IsMultiplayerActive && __instance.validPosition && __instance.ghostModel && __instance.TryGetComponent<Pickupable>(out var pickupable) && pickupable.GetTechType() == TechType.LEDLight)
            {
                try
                {
                    LEDLightDeployingEventArgs args = new LEDLightDeployingEventArgs(pickupable.gameObject.GetIdentityId(), __instance.ghostModel.transform.position, __instance.ghostModel.transform.rotation);

                    Handlers.Items.OnLEDLightDeploying(args);

                    if (!args.IsAllowed)
                    { 
                        __instance.usedThisFrame = false;
                    }

                    return args.IsAllowed;
                }
                catch (Exception e)
                {
                    Log.Error($"LEDLightDeploying.Prefix: {e}\n{e.StackTrace}");
                }
            }

            return true;
        }
    }
}