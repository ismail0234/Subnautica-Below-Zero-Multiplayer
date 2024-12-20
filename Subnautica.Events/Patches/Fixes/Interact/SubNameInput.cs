namespace Subnautica.Events.Patches.Fixes.Interact
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.Patches.Events.Game;

    [HarmonyPatch(typeof(global::SubNameInput), nameof(global::SubNameInput.OnPointerHover))]
    public class SubNameInput
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::SubNameInput __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (!__instance.enabled || __instance.selected || __instance.target == null)
            {
                return false;
            }

            var detail = SubnameInputDetail.GetInformation(__instance.gameObject);
            if (detail.TechType == TechType.BaseControlRoom)
            {
                if (Interact.IsBlocked(TechGroup.GetBaseControlRoomCustomizerId(detail.UniqueId)))
                {
                    Interact.ShowUseDenyMessage();
                    return false;
                }
            }
            else
            {
                if (Interact.IsBlocked(detail.UniqueId))
                {
                    Interact.ShowUseDenyMessage();
                    return false;
                }
            }

            return true;
        }
    }
}
