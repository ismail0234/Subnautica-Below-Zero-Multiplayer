namespace Subnautica.Events.Patches.Identity.World
{
    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;

    [HarmonyPatch(typeof(global::CinematicModeTriggerBase), nameof(global::CinematicModeTriggerBase.OnEnable))]
    public class CinematicModeTriggerBase
    {
        private static void Prefix(global::CinematicModeTriggerBase __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                var uniqueId = Network.Identifier.GetIdentityId(__instance.gameObject, false);
                if (uniqueId.IsNull())
                {
                    Network.Identifier.SetIdentityId(__instance.gameObject, Network.Identifier.GetWorldEntityId(__instance.transform.position, "Cinematic"));
                }
            }
        }
    }
}
