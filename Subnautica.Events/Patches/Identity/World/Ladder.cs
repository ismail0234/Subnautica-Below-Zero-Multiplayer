namespace Subnautica.Events.Patches.Identity.World
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.Patches.Events.Items;

    using UnityEngine;

    [HarmonyPatch(typeof(global::CinematicModeTriggerBase), nameof(global::CinematicModeTriggerBase.OnEnable))]
    public class Ladder
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Prefix(global::CinematicModeTriggerBase __instance)
        {
            if (Network.IsMultiplayerActive && __instance.TryGetComponent<global::CinematicModeTrigger>(out var cinematic) && Climbing.ClimbTexts.Contains(cinematic.handText))
            {
                Network.Identifier.SetIdentityId(__instance.gameObject, GetLadderUniqueId(cinematic));
            }
        }

        /**
         *
         * Merdiveni döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetLadderUniqueId(global::CinematicModeTrigger cinematic)
        {
            GameObject ladder = null;

            foreach (var item in cinematic.GetComponentsInParent<Component>())
            {
                if (item.name == "Ladder" || item.name == "ladder1_cin")
                {
                    ladder = item.gameObject;
                    break;
                }
            }

            if (ladder == null)
            {
                return Network.Identifier.GetWorldEntityId(cinematic.transform.position, cinematic.handText);
            }

            return Network.Identifier.GetWorldEntityId(ladder.transform.position, cinematic.name);
        }
    }
}
