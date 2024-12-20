namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::BaseBioReactor), nameof(global::BaseBioReactor.OnUse))]
    public static class BioReactorOpening
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::BaseBioReactor __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (EventBlocker.IsEventBlocked(TechType.BaseBioReactor))
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
                StorageOpeningEventArgs args = new StorageOpeningEventArgs(Network.Identifier.GetIdentityId(model.gameObject), TechType.BaseBioReactor);

                Handlers.Storage.OnOpening(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"BioReactorOpening.Prefix: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
}