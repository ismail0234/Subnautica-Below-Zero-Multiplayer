namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::TechLight), nameof(global::TechLight.Start))]
    public static class TechLightInitialized
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Postfix(global::TechLight __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    TechLightInitializedEventArgs args = new TechLightInitializedEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject));

                    Handlers.Furnitures.OnTechLightInitialized(args);
                }
                catch (Exception e)
                {
                    Log.Error($"TechLightInitialized.Postfix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}