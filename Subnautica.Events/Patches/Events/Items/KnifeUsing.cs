namespace Subnautica.Events.Patches.Events.Items
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    using UnityEngine;

    [HarmonyPatch(typeof(global::Knife), nameof(global::Knife.OnToolUseAnim))]
    public class KnifeUsing
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Prefix(global::Knife __instance, GUIHand hand)
        {
            if (!Network.IsMultiplayerActive)
            {
                return;
            }

            try
            {
                Vector3 normal;
                var targetPosition   = new Vector3();
                var surfaceType      = VFXSurfaceTypes.fallback;
                var soundSurfaceType = VFXSurfaceTypes.none;
                var orientation      = MainCameraControl.main.transform.eulerAngles + new Vector3(300f, 90f, 0.0f);
                var targetObject     = KnifeUsing.GetTargetObject(__instance.attackDist, ref targetPosition, out normal);

                if (targetObject)
                {
                    var vfxSurface = targetObject.GetComponent<VFXSurface>();
                    if (vfxSurface != null)
                    {
                        surfaceType = vfxSurface.surfaceType;
                    }
                    
                    soundSurfaceType = Utils.GetObjectSurfaceType(targetObject);
                    if (soundSurfaceType == VFXSurfaceTypes.none)
                    {
                        soundSurfaceType = Utils.GetTerrainSurfaceType(targetPosition, normal, VFXSurfaceTypes.sand);
                    }
                }

                KnifeUsingEventArgs args = new KnifeUsingEventArgs(__instance.vfxEventType, targetPosition, orientation, surfaceType, soundSurfaceType, global::Player.main.IsUnderwater());
            
                Handlers.Items.OnKnifeUsing(args);
            }
            catch (Exception e)
            {
                Log.Error($"KnifeUsing.Prefix: {e}\n{e.StackTrace}");
            }
        }

        /**
         *
         * Hedef Nesneyi döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static GameObject GetTargetObject(float attackDist, ref Vector3 position, out Vector3 normal)
        {
            GameObject closestObj = null;

            UWE.Utils.TraceFPSTargetPosition(global::Player.main.gameObject, attackDist, ref closestObj, ref position, out normal);

            if (closestObj == null)
            {
                var component = global::Player.main.gameObject.GetComponent<InteractionVolumeUser>();
                if (component != null && component.GetMostRecent() != null)
                {
                    closestObj = component.GetMostRecent().gameObject;
                }
            }

            return closestObj;
        }
    }
}