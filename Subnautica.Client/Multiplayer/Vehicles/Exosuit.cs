namespace Subnautica.Client.Multiplayer.Vehicles
{
    using FMOD.Studio;
    using FMODUnity;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Server;

    using UnityEngine;

    public class Exosuit : VehicleController
    {
        /**
         *
         * Exosuit aracını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::Exosuit ExoSuit { get; set; }

        /**
         *
         * Araç bileşenini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private ExosuitUpdateComponent VehicleComponent { get; set; } = new ExosuitUpdateComponent();

        /**
         *
         * Exosuit aracını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Vector3 SmoothedVelocity { get; set; } = Vector3.zero;

        /**
         *
         * Her karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnUpdate()
        {
            if (this.ExoSuit.docked == false)
            {
                base.OnUpdate();
            }

            if (this.ExoSuit)
            {
                this.UpdateMovementAnimation();
                this.UpdateMotorAnimation();
                this.UpdateExosuitArms();
            }
        }

        /**
         *
         * Bileşen verisi alındığında yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnComponentDataReceived(VehicleUpdateComponent component)
        {
            this.VehicleComponent = component.GetComponent<ExosuitUpdateComponent>();
        }

        /**
         *
         * Oyuncu araca bindiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnEnterVehicle()
        {
            base.OnEnterVehicle();

            if (this.Management.Vehicle)
            {
                this.ExoSuit = this.Management.Vehicle.GetComponent<global::Exosuit>();
                this.ExoSuit.collisionModel.SetActive(true);
                this.ExoSuit.useRigidbody.SetKinematic();
                this.ExoSuit.mainAnimator.Rebind();

                this.SetPlayerParent(this.ExoSuit.playerPosition.transform);

                this.ExoSuit.mainAnimator.SetBool("player_in", true);
                this.Management.Player.Animator.SetBool("in_exosuit", true);

                this.ResetAnimations();
           }
        }

        /**
         *
         * Oyuncu araçtan indiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnExitVehicle()
        {
            if (this.ExoSuit)
            {
                this.ResetAnimations();

                this.Management.Player.Animator.SetBool("in_exosuit", false);
                this.ExoSuit.mainAnimator.SetBool("player_in", false);
            }

            this.ExoSuit = null;
            base.OnExitVehicle();
        }

        /**
         *
         * Yürüme animasyonu uygular
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void UpdateMovementAnimation()
        {
            var relativeVelocity = this.ExoSuit.useRigidbody.transform.InverseTransformDirection(this.VehicleVelocity - this.ExoSuit.useRigidbody.GetPointVelocity(this.ExoSuit.useRigidbody.position));

            this.SmoothedVelocity = Vector3.Slerp(this.SmoothedVelocity, relativeVelocity, 4f * Time.deltaTime);

            this.ExoSuit.mainAnimator.SetBool("sit", false);
            this.ExoSuit.mainAnimator.SetBool("onGround", this.VehicleComponent.IsOnGround);
            this.ExoSuit.mainAnimator.SetFloat("move_speed"  , this.SmoothedVelocity.magnitude);
            this.ExoSuit.mainAnimator.SetFloat("move_speed_x", this.SmoothedVelocity.x);
            this.ExoSuit.mainAnimator.SetFloat("move_speed_y", this.SmoothedVelocity.y);
            this.ExoSuit.mainAnimator.SetFloat("move_speed_z", this.SmoothedVelocity.z);
        }

        /**
         *
         * Motor su kabarcık animasyonunu çalıştırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void UpdateMotorAnimation()
        {
            if (!this.ExoSuit.fxcontrol.IsPlaying(0))
            {
                this.ExoSuit.fxcontrol.Play(0);
            }
        }

        /**
         *
         * Yürüme animasyonu uygular
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void UpdateExosuitArms()
        {
            var armLerpFormula = this.GetArmLerpFormula(this.VehicleComponent.AngleX, this.VehicleComponent.CameraPosition.ToVector3());
            if (this.ExoSuit.aimTargetLeft)
            {
                this.ExoSuit.aimTargetLeft.transform.position = Vector3.Lerp(this.ExoSuit.aimTargetLeft.transform.position, armLerpFormula, Time.deltaTime * 15f);
            }

            if (this.ExoSuit.aimTargetRight)
            {
                this.ExoSuit.aimTargetRight.transform.position = Vector3.Lerp(this.ExoSuit.aimTargetRight.transform.position, armLerpFormula, Time.deltaTime * 15f);
            }

            this.ArmProcess(this.ExoSuit.leftArm, this.ExoSuit.leftArmType, this.VehicleComponent.LeftArm);
            this.ArmProcess(this.ExoSuit.rightArm, this.ExoSuit.rightArmType, this.VehicleComponent.RightArm);
        }


        /**
         *
         * Kolların işlemlerini uygular.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void ArmProcess(IExosuitArm exosuitArm, TechType techType, ExosuitArmComponent armComponent, bool isReset = false)
        {
            if (techType == TechType.ExosuitDrillArmModule)
            {
                this.ProcessDrillArmModule(armComponent?.GetComponent<ExosuitDrillArmComponent>(), exosuitArm as global::ExosuitDrillArm, isReset);
            }
            else if (techType == TechType.ExosuitClawArmModule)
            {
                this.ProcessClawArmModule(armComponent?.GetComponent<ExosuitClawArmComponent>(), exosuitArm as global::ExosuitClawArm, isReset);
            }
            else if (techType == TechType.ExosuitGrapplingArmModule)
            {
                this.ProcessGrapplingArmModule(armComponent?.GetComponent<ExosuitGrapplingArmComponent>(), exosuitArm as global::ExosuitGrapplingArm, isReset);
            }
            else if (techType == TechType.ExosuitTorpedoArmModule)
            {

            }
        }

        /**
         *
         * Drill Arm işlemlerini uygular.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool ProcessDrillArmModule(ExosuitDrillArmComponent component, global::ExosuitDrillArm drillArm, bool isReset)
        {
            if (isReset)
            {
                drillArm.animator.SetBool("use_tool", false);
                drillArm.StopEffects();
                return true;
            }

            if (component == null || drillArm == null)
            {
                return false;
            }

            if (component.IsDrilling)
            {
                if (!drillArm.animator.GetBool("use_tool"))
                {
                    drillArm.animator.SetBool("use_tool", true);
                }

                if (component.IsFxPlaying)
                {
                    if (!drillArm.fxControl.IsPlaying(0, true))
                    {
                        drillArm.fxControl.Play(0);
                    }
                }
                else
                {
                    if (drillArm.fxControl.IsPlaying(0, true))
                    {
                        drillArm.fxControl.Stop(0);
                    }
                }
            }
            else
            {
                drillArm.animator.SetBool("use_tool", false);
                drillArm.fxControl.Stop(0);
            }

            return true;
        }

        /**
         *
         * Claw Arm işlemlerini uygular.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool ProcessClawArmModule(ExosuitClawArmComponent component, global::ExosuitClawArm clawArm, bool isReset)
        {
            if (isReset)
            {
                clawArm.animator.SetBool("use_tool", false);
                clawArm.animator.SetBool("bash", false);
                return true;
            }

            if (component == null || clawArm == null)
            {
                return false;
            }

            if (component.IsUsing && !isReset)
            {
                if (component.IsPickup && !clawArm.animator.GetBool("use_tool"))
                {
                    clawArm.animator.SetBool("use_tool", true);
                }
                else if (component.IsBash && !clawArm.animator.GetBool("bash"))
                {
                    clawArm.animator.SetBool("bash", true);
                }
            }
            else
            {
                clawArm.animator.SetBool("use_tool", false);
                clawArm.animator.SetBool("bash", false);
            }

            return true;
        }

        /**
         *
         * Grappling Arm işlemlerini uygular.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool ProcessGrapplingArmModule(ExosuitGrapplingArmComponent component, global::ExosuitGrapplingArm grapplingArm, bool isReset)
        {
            if (isReset)
            {
                grapplingArm?.animator.SetBool("use_tool", false);
                grapplingArm?.ResetHook();
                return true;
            }

            if (component == null || grapplingArm == null)
            {
                return false;
            }

            if (component.IsFlying || component.IsAttached || component.IsUsing)
            {
                grapplingArm.animator.SetBool("use_tool", true);
            }
            else
            {
                grapplingArm.animator.SetBool("use_tool", false);
            }

            if (component.IsFlying || component.IsAttached || !component.IsStopped)
            {
                grapplingArm.hook.transform.parent   = null;
                grapplingArm.hook.transform.position = Vector3.Lerp(grapplingArm.hook.transform.position, component.HookPosition.ToVector3(), 0.1f);
                grapplingArm.hook.transform.rotation = Quaternion.Lerp(grapplingArm.hook.transform.rotation, component.HookRotation.ToQuaternion(), 0.1f);
                grapplingArm.hook.rb.isKinematic     = true;

                if (component.IsFlying)
                {
                    grapplingArm.hook.fxControl.Play(0);
                    grapplingArm.rope.LaunchHook(0f);
                }
            }
            else
            {
                if (grapplingArm.hook.transform.parent == null)
                {
                    grapplingArm.ResetHook();
                }
            }

            return true;
        }

        /**
         *
         * Animasyonları sıfırlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void ResetAnimations()
        {
            this.SmoothedVelocity = Vector3.zero;

            this.ExoSuit.mainAnimator.SetFloat("move_speed", 0f);
            this.ExoSuit.mainAnimator.SetFloat("move_speed_x", 0f);
            this.ExoSuit.mainAnimator.SetFloat("move_speed_y", 0f);
            this.ExoSuit.mainAnimator.SetFloat("move_speed_z", 0f);

            this.ExoSuit.mainAnimator.SetBool("sit", false);
            this.ExoSuit.mainAnimator.SetBool("onGround", false);
            this.ExoSuit.mainAnimator.SetBool("powersliding", false);
            this.ExoSuit.mainAnimator.SetFloat("thrustIntensity", 0f);

            this.ExoSuit.fxcontrol.Stop(0);

            this.ArmProcess(this.ExoSuit.leftArm, this.ExoSuit.leftArmType, this.VehicleComponent.LeftArm, true);
            this.ArmProcess(this.ExoSuit.rightArm, this.ExoSuit.rightArmType, this.VehicleComponent.RightArm, true);
      }

        /**
         *
         * Kol lerp formülünü döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Vector3 GetArmLerpFormula(float angleX, Vector3 cameraPosition)
        {
            var aimDirection = Quaternion.Euler(angleX, this.ExoSuit.transform.eulerAngles.y, this.ExoSuit.transform.eulerAngles.z);
            return cameraPosition + aimDirection * Vector3.forward * 100f;
        }
    }
}
