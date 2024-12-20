namespace Subnautica.Events.Patches.Events.Creatures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using UnityEngine;

    [HarmonyPatch]
    public class Freezing
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::CreatureFrozenMixin), nameof(global::CreatureFrozenMixin.Freeze))]
        private static bool CreatureFrozenMixin_Freeze(global::CreatureFrozenMixin __instance, float endTime)
        {
            if (!Network.IsMultiplayerActive || EventBlocker.IsEventBlocked(ProcessType.CreatureFreeze))
            {
                return true;
            }

            if (!__instance.enabled)
            {
                return false;
            }

            try
            {
                CreatureFreezingEventArgs args = new CreatureFreezingEventArgs(__instance.gameObject.GetIdentityId(), endTime - Time.time);

                Handlers.Creatures.OnFreezing(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"Freezed.Prefix: {e}\n{e.StackTrace}");
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
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Brinicle), nameof(global::Brinicle.FreezeInternal))]
        private static bool CreatureFrozenMixin_Freeze(global::Brinicle __instance, GameObject go)
        {
            if (!Network.IsMultiplayerActive || !go.GetTechType().IsSynchronizedCreature())
            {
                return true;
            }

            if (__instance.state != Brinicle.State.Grow && __instance.state != Brinicle.State.Enabled)
            {
                return false;
            }

            if (go.TryGetComponent<global::FrozenMixin>(out var component) && !component.IsFrozenInsideIce())
            {
                try
                {
                    CreatureFreezingEventArgs args = new CreatureFreezingEventArgs(go.GetIdentityId(), float.PositiveInfinity, __instance.gameObject.GetIdentityId());

                    Handlers.Creatures.OnFreezing(args);

                    return args.IsAllowed;
                }
                catch (Exception e)
                {
                    Log.Error($"Freezed.Prefix: {e}\n{e.StackTrace}");
                }
            }

            return false;
        }
    }
}
