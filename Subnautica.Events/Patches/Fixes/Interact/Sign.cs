namespace Subnautica.Events.Patches.Fixes.Interact
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::Sign), nameof(global::Sign.OnHandHover))]
    public class Sign
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::Sign __instance)
        {
            if(!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!__instance.enabled)
            {
                return false;
            }

            var constructable = __instance.gameObject.GetComponentInParent<Constructable>();
            if (constructable == null)
            {
                return false;
            }

            if (Interact.IsBlocked(Network.Identifier.GetIdentityId(constructable.gameObject)))
            {
                Interact.ShowUseDenyMessage();
                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(global::ColoredLabel), nameof(global::ColoredLabel.OnHandHover))]
    public class ColoredLabel
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::ColoredLabel __instance)
        {
            if(!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!__instance.enabled)
            {
                return false;
            }

            var uniqueId = string.Empty;

            var constructable = __instance.GetComponentInParent<Constructable>();
            if (constructable)
            {
                uniqueId = Network.Identifier.GetIdentityId(constructable.gameObject);
            }
            else
            {
                if (__instance.GetComponentInParent<global::SeaTruckSegment>())
                {
                    uniqueId = Network.Identifier.GetIdentityId(__instance.signInput.gameObject, false).Replace(ZeroGame.GetSeaTruckColoredLabelUniqueId(null, true), "");
                }
                else
                {
                    var lwe = __instance.GetComponentInParent<LargeWorldEntity>();
                    if (lwe)
                    {
                        uniqueId = Network.Identifier.GetIdentityId(lwe.gameObject, false);
                    }
                }
            }

            if (string.IsNullOrEmpty(uniqueId))
            {
                return false;
            }

            if (Interact.IsBlocked(uniqueId))
            {
                Interact.ShowUseDenyMessage();
                return false;
            }

            return true;
        }
    }
}
