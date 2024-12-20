namespace Subnautica.Events.Patches.Fixes.Game
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(CellManager), nameof(CellManager.RegisterEntity), new Type[] { typeof(LargeWorldEntity) })]
    public static class RegisterEntity
    {
        private static void Prefix(CellManager __instance, LargeWorldEntity lwe)
        {
            if (Network.IsMultiplayerActive && lwe.cellLevel != LargeWorldEntity.CellLevel.Global)
            {
                if (lwe.TryGetComponent<global::Constructable>(out var constructable))
                {
                    lwe.cellLevel = LargeWorldEntity.CellLevel.Global;
                }
            }
        }
    }
}
