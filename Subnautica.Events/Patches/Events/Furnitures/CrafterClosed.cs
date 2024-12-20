namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;
    using Subnautica.Events.Patches.Fixes.Interact;
    
    [HarmonyPatch(typeof(global::uGUI_CraftingMenu), nameof(global::uGUI_CraftingMenu.OnDeselect))]
    public static class CrafterClosed
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Type ConstructorInputType = typeof(ConstructorInput);

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::uGUI_CraftingMenu __instance)
        {   
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            var uniqueId = "";
            var techType = TechType.None;
            var type     = __instance._client.GetType();
            if (type == ConstructorInputType)
            {
                var constructorInput = __instance._client as global::ConstructorInput;
                if (constructorInput)
                {
                    uniqueId = Network.Identifier.GetIdentityId(constructorInput.constructor.gameObject);
                    techType = TechType.Constructor;
                }
            }
            else
            {
                var ghostCrafter = __instance._client as global::GhostCrafter;
                if (ghostCrafter)
                {
                    uniqueId = GhostCrafter.GetUniqueId(ghostCrafter.gameObject);
                    techType = GhostCrafter.GetTechType(ghostCrafter.gameObject);
                }
            }

            if (uniqueId.IsNull())
            {
                return true;
            }

            try
            {
                CrafterClosedEventArgs args = new CrafterClosedEventArgs(uniqueId, techType);

                Handlers.Furnitures.OnCrafterClosed(args);
            }
            catch (Exception e)
            {
                Log.Error($"Furnitures.CrafterClosed: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }
}
