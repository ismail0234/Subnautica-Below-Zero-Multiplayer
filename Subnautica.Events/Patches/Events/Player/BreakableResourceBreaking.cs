namespace Subnautica.Events.Patches.Events.Player
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::BreakableResource), nameof(global::BreakableResource.BreakIntoResources))]
    public class BreakableResourceBreaking
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::BreakableResource __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.broken)
            {
                return false;
            }

            var resourceType = ChooseRandomResource(__instance);
            if (resourceType == TechType.None)
            {
                return false;
            }

            __instance.hitsToBreak = 2;

            try
            {
                BreakableResourceBreakingEventArgs args = new BreakableResourceBreakingEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject), resourceType, __instance.transform.position + __instance.transform.up * __instance.verticalSpawnOffset);

                Handlers.Player.OnBreakableResourceBreaking(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"BreakableResourceBreaking.Prefix: {e}\n{e.StackTrace}");
            }

            return true;
        }

        /**
         *
         * Rastgele teknoloji döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static TechType ChooseRandomResource(global::BreakableResource __instance)
        {
            for (int index = 0; index < __instance.prefabList.Count; ++index)
            {
                var prefab = __instance.prefabList[index];

                if (global::Player.main.gameObject.GetComponent<PlayerEntropy>().CheckChance(prefab.prefabTechType, prefab.chance))
                {
                    return prefab.prefabTechType;
                }
            }

            return TechType.Titanium;
        }
    }
}