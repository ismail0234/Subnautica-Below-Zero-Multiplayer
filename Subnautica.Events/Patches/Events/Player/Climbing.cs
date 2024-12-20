namespace Subnautica.Events.Patches.Events.Items
{
    using System;
    using System.Collections.Generic;

    using HarmonyLib;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::CinematicModeTriggerBase), nameof(global::CinematicModeTriggerBase.OnHandClick))]
    public class Climbing
    {
        /**
         *
         * Tırmanma metinleri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */        
        public static List<string> ClimbTexts = new List<string>()
        {
            "Climb",
            "ClimbUp",
            "ClimbDown",
            "ClimbLadder"
        };

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::CinematicModeTriggerBase __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                var uniqueId = Climbing.GetUniqueId(__instance);
                if (uniqueId.IsNull())
                {
                    return true;
                }

                try
                {
                    PlayerClimbingEventArgs args = new PlayerClimbingEventArgs(uniqueId, __instance.cinematicController?.director == null ? 0f : (float) __instance.cinematicController.director.duration);

                    Handlers.Player.OnClimbing(args);

                    return args.IsAllowed;
                }
                catch (Exception e)
                {
                    Log.Error($"Climbing.Prefix: {e}\n{e.StackTrace}");
                }
            }

            return true;
        }

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static string GetUniqueId(global::CinematicModeTriggerBase __instance)
        {
            var constructor = __instance.GetComponentInParent<global::Constructor>();
            if (constructor)
            {
                return Network.Identifier.GetIdentityId(constructor.gameObject, false);
            }

            if (__instance.TryGetComponent<global::CinematicModeTrigger>(out var cinematic) && ClimbTexts.Contains(cinematic.handText))
            {
                return Network.Identifier.GetIdentityId(cinematic.gameObject, false);
            }

            return null;
        }
    }
}