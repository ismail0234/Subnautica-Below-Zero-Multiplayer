namespace Subnautica.Events.Patches.Events.Items
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using UnityEngine;

    [HarmonyPatch(typeof(global::ConstructorInput), nameof(global::ConstructorInput.Craft))]
    public class ConstructorCrafting
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::ConstructorInput __instance, TechType techType, float duration)
        {
            if (Network.IsMultiplayerActive && !EventBlocker.IsEventBlocked(TechType.Constructor))
            {
                var zero = Vector3.zero;
                var identity = Quaternion.identity;
                __instance.GetCraftTransform(techType, ref zero, ref identity);

                if (!__instance.CheckSpace(techType, zero))
                {
                    __instance.invalidNotification.Play();
                    return false;
                }

                if (!CrafterLogic.IsCraftRecipeFulfilled(techType))
                {
                    return false;
                }

                try
                {
                    ConstructorCraftingEventArgs args = new ConstructorCraftingEventArgs(Network.Identifier.GetIdentityId(__instance.constructor.gameObject), techType, zero, identity);

                    Handlers.Items.OnConstructorCrafting(args);

                    return args.IsAllowed;
                }
                catch (Exception e)
                {
                    Log.Error($"ConstructorCrafting.Prefix: {e}\n{e.StackTrace}");
                }
            }

            return true;
        }
    }
}