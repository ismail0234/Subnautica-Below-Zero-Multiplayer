namespace Subnautica.Events.Patches.Events.Furnitures
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch(typeof(global::Planter), nameof(global::Planter.AddItem), new Type[] { typeof(Plantable), typeof(int) })]
    public static class PlanterItemAdded
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Postfix(global::Planter __instance, Plantable plantable, int slotID)
        {
            if (!Network.IsMultiplayerActive)
            {
                return;
            }

            if (__instance.constructable == null || EventBlocker.IsEventBlocked(__instance.constructable.techType))
            {
                return;
            }
            
            try
            {
                PlanterItemAddedEventArgs args = new PlanterItemAddedEventArgs(__instance.constructable.gameObject.GetIdentityId(), plantable.gameObject.GetIdentityId(), plantable, slotID);

                Handlers.Furnitures.OnPlanterItemAdded(args);
            }
            catch (Exception e)
            {
                Log.Error($"PlanterItemAdded.Prefix: {e}\n{e.StackTrace}");
            }
        }
    }
}