namespace Subnautica.Events.Patches.Fixes.Interact
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch]
    public class Construction
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::BuilderTool), nameof(global::BuilderTool.OnHover), new Type[] { typeof(Constructable) })]
        private static bool BuilderTool_OnHover_Constructable(Constructable constructable)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (constructable.constructed && !constructable.deconstructionAllowed)
            {
                return false;
            }

            if (Interact.IsBlocked(Network.Identifier.GetIdentityId(constructable.gameObject), true))
            {
                Interact.ShowUseDenyMessage();
                return false;
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::BuilderTool), nameof(global::BuilderTool.OnHover), new Type[] { typeof(BaseDeconstructable) })]
        private static bool BuilderTool_OnHover_BaseDeconstructable(BaseDeconstructable deconstructable)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (Interact.IsBlocked(Network.Identifier.GetIdentityId(deconstructable.gameObject, false), true))
            {
                Interact.ShowUseDenyMessage();
                return false;
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::BuilderTool), nameof(global::BuilderTool.Construct))]
        private static bool BuilderTool_Construct(Constructable c)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (Interact.IsBlocked(Network.Identifier.GetIdentityId(c.gameObject), true))
            {
                Interact.ShowUseDenyMessage();
                return false;
            }

            return true;
        }
    }
}
