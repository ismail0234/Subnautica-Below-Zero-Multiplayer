namespace Subnautica.Events.Patches.Events.Items
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::DeployableStorage), nameof(global::DeployableStorage.Throw))]
    public class DeployableStorageDeploying
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::DeployableStorage __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            __instance._isInUse = false;
            __instance.SetContainerActiveState(false);

            try
            {
                DeployableStorageDeployingEventArgs args = new DeployableStorageDeployingEventArgs(Network.Identifier.GetIdentityId(__instance.pickupable.gameObject), __instance.pickupable, ZeroGame.FindDropPosition(__instance.transform.position), global::MainCameraControl.main.transform.forward * 2f);

                Handlers.Items.OnDeployableStorageDeploying(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"DeployableStorage.Prefix: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }
}