namespace Subnautica.Events.Patches.Events.PDA
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    using UnityEngine;

    [HarmonyPatch(typeof(global::PDA), nameof(global::PDA.Close))]
    public static class Closing
    {
        public static void Prefix(global::PDA __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return;
            }

            if (!__instance.isInUse || __instance.ignorePDAInput)
            { 
                return; 
            }

            try
            {
                PDAClosingEventArgs args = new PDAClosingEventArgs(__instance.target != null ? Network.Identifier.GetIdentityId(__instance.target.gameObject, false) : null, GetTechType(__instance.target));

                Handlers.PDA.OnClosing(args);
            }
            catch (Exception e)
            {
                Log.Error($"PDA.Closing: {e}\n{e.StackTrace}");
            }
        }

        /**
         *
         * Teknoloji türünü döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static TechType GetTechType(Transform transform)
        {
            if(transform == null)
            {
                return TechType.None;
            }

            var constructable = transform.gameObject.GetComponent<Constructable>();
            if(constructable == null)
            {
                return TechType.None;
            }

            return constructable.techType;
        }
    }
}