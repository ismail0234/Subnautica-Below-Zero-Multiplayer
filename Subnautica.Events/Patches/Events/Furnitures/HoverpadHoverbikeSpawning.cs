namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::HoverpadConstructor), nameof(global::HoverpadConstructor.TryStartConstructBike))]
    public static class HoverpadHoverbikeSpawning
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::HoverpadConstructor __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.crafterLogic.inProgress)
            {
                return false;
            }

            var constructable = __instance.GetComponentInParent<Constructable>();
            if (!constructable || !constructable.constructed || !CrafterLogic.IsCraftRecipeFulfilled(TechType.Hoverbike))
            {
                return false;
            }

            try
            {
                HoverpadHoverbikeSpawningEventArgs args = new HoverpadHoverbikeSpawningEventArgs(Network.Identifier.GetIdentityId(constructable.gameObject));

                Handlers.Furnitures.OnHoverpadHoverbikeSpawning(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"HoverpadHoverbikeSpawning.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}
