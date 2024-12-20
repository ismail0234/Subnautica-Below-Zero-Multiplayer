namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::BaseNuclearReactor), nameof(global::BaseNuclearReactor.OnUse))]
    public static class NuclearReactorOpening
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::BaseNuclearReactor __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (EventBlocker.IsEventBlocked(TechType.BaseNuclearReactor))
            {
                return true;
            }

            var model = __instance.GetModel();
            if (model == null)
            {
                return false;
            }

            try
            {
                StorageOpeningEventArgs args = new StorageOpeningEventArgs(Network.Identifier.GetIdentityId(model.gameObject), TechType.BaseNuclearReactor);

                Handlers.Storage.OnOpening(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"NuclearReactorOpening.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}