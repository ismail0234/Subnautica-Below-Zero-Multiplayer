namespace Subnautica.Events.Patches.Events.Items
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::SnowStalkerSpyPenguinInteractions), nameof(global::SnowStalkerSpyPenguinInteractions.OnPenguinAction))]
    public class SpyPenguinSnowStalkerInteracting
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::SnowStalkerSpyPenguinInteractions __instance, SpyPenguin penguin)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            try
            {
                SpyPenguinSnowStalkerInteractingEventArgs args = new SpyPenguinSnowStalkerInteractingEventArgs(Network.Identifier.GetIdentityId(penguin.gameObject), __instance.furSpawnChance);

                Handlers.Items.OnSpyPenguinSnowStalkerInteracting(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"SpyPenguinSnowStalkerInteracting.Prefix: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }
}