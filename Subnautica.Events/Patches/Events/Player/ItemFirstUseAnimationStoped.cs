namespace Subnautica.Events.Patches.Events.Player
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::PlayerTool), nameof(global::PlayerTool.OnFirstUseAnimationStop))]
    public static class ItemFirstUseAnimationStoped
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Prefix(global::PlayerTool __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    ItemFirstUseAnimationStopedEventArgs args = new ItemFirstUseAnimationStopedEventArgs(__instance.pickupable != null ? __instance.pickupable.GetTechType() : TechType.None);

                    Handlers.Player.OnItemFirstUseAnimationStoped(args);
                }
                catch (Exception e)
                {
                    Log.Error($"ItemFirstUseAnimationStoped.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}