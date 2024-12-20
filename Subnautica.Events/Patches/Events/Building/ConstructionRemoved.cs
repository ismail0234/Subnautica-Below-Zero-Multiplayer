namespace Subnautica.Events.Patches.Events.Building
{
    using System;
    using System.Collections;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.API.Extensions;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(Constructable), nameof(Constructable.ProgressDeconstruction))]
    public static class ConstructionRemoved
    {
        private static IEnumerator Postfix(IEnumerator values, Constructable __instance)
        {
            yield return values;

            if (Network.IsMultiplayerActive && __instance.constructedAmount <= 0.0f)
            {
                try
                {
                    ConstructionRemovedEventArgs args = new ConstructionRemovedEventArgs(__instance.techType, __instance.gameObject.GetIdentityId());

                    Handlers.Building.OnConstructingRemoved(args);

                    Network.BaseFacePiece.Remove(args.UniqueId);
                }
                catch (Exception e)
                {
                    Log.Error($"Building.ConstructingRemoved: {e}\n{e.StackTrace}");
                }
            }
        }
    }

    [HarmonyPatch(typeof(ConstructableBase), nameof(ConstructableBase.ProgressDeconstruction))]
    public static class ConstructionBaseRemoved
    {
        private static IEnumerator Postfix(IEnumerator values, ConstructableBase __instance)
        {
            yield return values;

            if (Network.IsMultiplayerActive && __instance.constructedAmount <= 0.0f)
            {
                try
                {
                    Int3? cell = null;
                    var baseComp = __instance.GetComponentInParent<global::Base>();
                    if (baseComp)
                    {
                        cell = baseComp.WorldToGrid(__instance.gameObject.transform.position);
                    }

                    ConstructionRemovedEventArgs args = new ConstructionRemovedEventArgs(__instance.techType, __instance.gameObject.GetIdentityId(), cell);

                    Handlers.Building.OnConstructingRemoved(args);

                    Network.BaseFacePiece.Remove(args.UniqueId);
                }
                catch (Exception e)
                {
                    Log.Error($"Building.ConstructionBaseRemoved: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}
