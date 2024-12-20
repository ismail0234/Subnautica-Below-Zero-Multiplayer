namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::BulkheadDoor), nameof(global::BulkheadDoor.OnHandClick))]
    public static class BulkheadOpening
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::BulkheadDoor __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!__instance.enabled || PlayerCinematicController.cinematicModeCount > 0)
            {
                return false;
            }

            var uniqueId = BulkheadOpening.GetUniqueId(__instance);
            if (string.IsNullOrEmpty(uniqueId))
            {
                return false;
            }

            try
            {
                if (__instance.opened)
                {
                    BulkheadClosingEventArgs args = new BulkheadClosingEventArgs(uniqueId, __instance.GetSide());

                    Handlers.Furnitures.OnBulkheadClosing(args);

                    return args.IsAllowed;
                }
                else
                {
                    BulkheadOpeningEventArgs args = new BulkheadOpeningEventArgs(uniqueId, __instance.GetSide(), GetStoryCinematicType(__instance));

                    Handlers.Furnitures.OnBulkheadOpening(args);

                    return args.IsAllowed;
                }
            }
            catch (Exception e)
            {
                Log.Error($"BulkheadDoor.OnHandClick: {e}\n{e.StackTrace}");
                return true;
            }
        }

        /**
         *
         * UniqueId döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static string GetUniqueId(global::BulkheadDoor __instance)
        {
            var constructable = __instance.GetComponentInParent<BaseDeconstructable>();
            if (constructable)
            {
                return Network.Identifier.GetIdentityId(constructable.gameObject);
            }

            var door = __instance.GetComponent<global::BulkheadDoor>();
            if (door)
            {
                return Network.Identifier.GetIdentityId(door.gameObject, false);
            }

            return null;
        }

        /**
         *
         * UniqueId döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static StoryCinematicType GetStoryCinematicType(global::BulkheadDoor __instance)
        {            
            var fixedBase = __instance.GetComponentInParent<FixedBase>();
            if (fixedBase)
            {
                if (fixedBase.name.Contains("Marguerit_Base"))
                {
                    return StoryCinematicType.StoryMarg2;
                }

                if (fixedBase.name.Contains("Marguerite_GreenHouse"))
                {
                    return StoryCinematicType.StoryMarg3;
                }
            }

            return StoryCinematicType.None;
        }
    }
}
