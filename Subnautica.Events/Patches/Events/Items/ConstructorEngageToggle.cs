namespace Subnautica.Events.Patches.Events.Items
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch]
    public class ConstructorEngageToggle
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::ConstructorInput), nameof(global::ConstructorInput.OnHandClick))]
        private static bool OnHandClick(global::ConstructorInput __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                if (__instance.logic == null || __instance.logic.inProgress || !__instance.GetPlayerAllowedToUse() || !CraftTree.HasKnown(CraftTree.Type.Constructor))
                {
                    return false;
                }

                try
                {
                    ConstructorEngageToggleEventArgs args = new ConstructorEngageToggleEventArgs(Network.Identifier.GetIdentityId(__instance.GetComponentInParent<global::Constructor>().gameObject), true);

                    Handlers.Items.OnConstructorEngageToggle(args);

                    return args.IsAllowed;
                }
                catch (Exception e)
                {
                    Log.Error($"ConstructorEngageToggle.Prefix: {e}\n{e.StackTrace}");
                }
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::ConstructorCinematicController), nameof(global::ConstructorCinematicController.DisengageConstructor), new Type[] { typeof(global::Player), typeof(bool) })]
        private static void DisengageConstructor(global::ConstructorCinematicController __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                try
                {
                    ConstructorEngageToggleEventArgs args = new ConstructorEngageToggleEventArgs(Network.Identifier.GetIdentityId(__instance.constructorInput.GetComponentInParent<global::Constructor>().gameObject), false);

                    Handlers.Items.OnConstructorEngageToggle(args);
                }
                catch (Exception e)
                {
                    Log.Error($"ConstructorEngageToggle.Prefix: {e}\n{e.StackTrace}");
                }
            }
        }
    }
}