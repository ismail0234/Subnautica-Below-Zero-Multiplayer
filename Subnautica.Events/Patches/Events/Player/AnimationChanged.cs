namespace Subnautica.Events.Patches.Events.Player
{
    using System;
    using System.Collections.Generic;

    using HarmonyLib;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(ArmsController), nameof(ArmsController.Update))]
    public static class AnimationChanged
    {
        /**
         *
         * Animasyon durum önbelleği.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Dictionary<string, bool> AnimationStatusCache { get; set; } = new Dictionary<string, bool>();

        /**
         *
         * Değişen animasyon listesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Dictionary<PlayerAnimationType, bool> ChangedAnimations { get; set; } = new Dictionary<PlayerAnimationType, bool>();

        /**
         *
         * SonEk Yaması
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Postfix(ArmsController __instance)
        {
            if (Network.IsMultiplayerActive)
            {
                AnimationChanged.ChangedAnimations.Clear();

                foreach (var animationName in PlayerAnimationTypeExtensions.Animations)
                {
                    var status = __instance.animator.GetBool(animationName);
                    if (AnimationChanged.IsAnimationChanged(animationName, status))
                    {
                        AnimationChanged.ChangedAnimations.Add(animationName.ToPlayerAnimationType(), status);
                    }
                }

                if (AnimationChanged.ChangedAnimations.Count > 0)
                {
                    try
                    {
                        PlayerAnimationChangedEventArgs args = new PlayerAnimationChangedEventArgs(AnimationChanged.ChangedAnimations);

                        Handlers.Player.OnAnimationChanged(args);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"AnimationChanged.Postfix: {e}\n{e.StackTrace}");
                    }
                }
            }
        }

        /**
         *
         * Animasyon durumu değişti mi? kontrolü yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool IsAnimationChanged(string animation, bool newStatus)
        {
            AnimationChanged.AnimationStatusCache.TryGetValue(animation, out bool oldStatus);
            AnimationChanged.AnimationStatusCache[animation] = newStatus;

            if (oldStatus != newStatus)
            {
                return true;
            }

            return false;
        }
    }
}
