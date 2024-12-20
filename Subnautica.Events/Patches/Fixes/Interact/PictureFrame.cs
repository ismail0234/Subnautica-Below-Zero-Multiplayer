namespace Subnautica.Events.Patches.Fixes.Interact
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::PictureFrame), nameof(global::PictureFrame.OnHandHover))]
    public class PictureFrame
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::PictureFrame __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!__instance.enabled)
            {
                return false;
            }

            var uniqueId = PictureFrame.GetUniqueId(__instance);
            if (uniqueId.IsNull())
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

        /**
         *
         * Benzersiz ID numarasını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetUniqueId(global::PictureFrame __instance)
        {
            var constructable = __instance.gameObject.GetComponentInParent<global::Constructable>();
            if (constructable)
            {
                return constructable.gameObject.GetIdentityId();
            }

            var seaTruckSegment = __instance.gameObject.GetComponentInParent<global::SeaTruckSegment>();
            if (seaTruckSegment)
            {
                return seaTruckSegment.gameObject.GetIdentityId();
            }

            return null;
        }
    }
}