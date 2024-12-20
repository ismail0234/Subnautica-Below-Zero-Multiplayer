namespace Subnautica.Client.Synchronizations.Processors.Vehicle.Components
{
    using Subnautica.API.Extensions;

    using Subnautica.Network.Models.Server;

    using UnityEngine;

    public class Exosuit
    {
        /**
         *
         * Araç bileşenini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static ExosuitUpdateComponent GetComponent(ExosuitUpdateComponent component, global::Exosuit exosuit)
        {
            component.IsOnGround     = exosuit.mainAnimator.GetBool("onGround");
            component.CameraPosition = MainCamera.camera.transform.position.ToZeroVector3();
            component.AngleX         = MainCamera.camera.transform.eulerAngles.x;
            component.LeftArm        = GetArmComponent(exosuit.leftArm, exosuit.leftArmType, true);
            component.RightArm       = GetArmComponent(exosuit.rightArm, exosuit.rightArmType, false);
            component.IsPlayingJumpSound  = exosuit.jumpJetsSound.IsPlaying();
            component.IsPlayingBoostSound = exosuit.boostSound.IsPlaying();
            return component;
        }

        /**
         *
         * Araç kol bileşenini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static ExosuitArmComponent GetArmComponent(IExosuitArm exosuitArm, TechType armType, bool isLeftArm)
        {
            if (exosuitArm == null)
            {
                return null;
            }

            if (armType == TechType.ExosuitGrapplingArmModule)
            {
                var module = exosuitArm as global::ExosuitGrapplingArm;
                if (module && module.hook)
                {
                    return new ExosuitGrapplingArmComponent()
                    {
                        HookPosition = module.hook.transform.position.ToZeroVector3(),
                        HookRotation = module.hook.transform.rotation.ToZeroQuaternion(),
                        IsAttached   = module.hook.attached,
                        IsFlying     = module.hook.flying,
                        IsUsing      = module.animator.GetBool("use_tool"),
                        IsStopped    = module.hook.transform.localPosition == Vector3.zero,
                    };
                }
            }
            else if (armType == TechType.ExosuitClawArmModule)
            {
                var module = exosuitArm as global::ExosuitClawArm;
                if (module && module.exosuit)
                {
                    var cooldown = module.exosuit.GetQuickSlotCooldown(module.exosuit.GetSlotIndex(isLeftArm ? "ExosuitArmLeft" : "ExosuitArmRight"));

                    return new ExosuitClawArmComponent()
                    {
                        IsBash   = module.cooldownTime == module.cooldownPunch,
                        IsPickup = module.cooldownTime == module.cooldownPickup,
                        IsUsing  = cooldown > 0.0f && cooldown <= 0.5f,
                    };
                }
            }
            else if (armType == TechType.ExosuitDrillArmModule)
            {
                var module = exosuitArm as global::ExosuitDrillArm;
                if (module)
                {
                    return new ExosuitDrillArmComponent()
                    {
                        IsDrilling  = module.drilling,
                        IsFxPlaying = module.fxControl.IsPlaying(0, true),
                    };
                }
            }

            return null;
        }
    }
}
