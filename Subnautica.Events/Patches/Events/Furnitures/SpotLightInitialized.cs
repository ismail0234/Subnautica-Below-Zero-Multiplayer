namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::BaseSpotLight), nameof(global::BaseSpotLight.Start))]
    public static class SpotLightInitialized
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Postfix(global::BaseSpotLight __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    SpotLightInitializedEventArgs args = new SpotLightInitializedEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject));

                    Handlers.Furnitures.OnSpotLightInitialized(args);
                }
                catch (Exception e)
                {
                    Log.Error($"SpotLightInitialized.Postfix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}