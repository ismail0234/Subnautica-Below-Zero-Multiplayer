namespace Subnautica.Events.Patches.Events.Player
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.API.Extensions;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::Player), nameof(global::Player.TrySetRespawnPoint))]
    public class RespawnPointChanged
    {
        private static bool Prefix(global::Player __instance, IInteriorSpace interior)
        {
            if (Network.IsMultiplayerActive && !EventBlocker.IsEventBlocked(ProcessType.PlayerRespawnPointChanged)  && interior.GetRespawnPoint() != null && !interior.Equals(null))
            {
                if (__instance.currentRespawnInterior == null || __instance.currentRespawnInterior.Equals(null) || interior.GetGameObject() != __instance.currentRespawnInterior.GetGameObject())
                {
                    if (interior.GetGameObject().TryGetComponent<global::SeaTruckSegment>(out var seaTruckSegment))
                    {
                        var expansionManager = seaTruckSegment.GetFirstSegment()?.GetDockedMoonpoolExpansion();
                        if (expansionManager)
                        {
                            return false;
                        }
                    }

                    try
                    {
                        PlayerRespawnPointChangedEventArgs args = new PlayerRespawnPointChangedEventArgs(__instance.GetUniqueIdentifierIfExists(interior.GetGameObject()));

                        Handlers.Player.OnRespawnPointChanged(args);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"RespawnPointChanged.Prefix: {e}\n{e.StackTrace}");
                    }
                }
            }

            return true;
        }
    }
}