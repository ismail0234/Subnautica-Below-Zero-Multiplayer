namespace Subnautica.Client.MonoBehaviours.Player
{
    using Subnautica.API.Features;
    using Subnautica.Network.Structures;

    using UnityEngine;

    public class PlayerFootstepSounds : MonoBehaviour
    {
        /**
          *
          * FootstepSounds_Player değerini barındırır.
          *
          * @author Ismail <ismaiil_0234@hotmail.com>
          *
          */
        public global::FootstepSounds FootstepSounds_Player;

        /**
          *
          * FootstepSounds_Exosuit değerini barındırır.
          *
          * @author Ismail <ismaiil_0234@hotmail.com>
          *
          */
        public global::FootstepSounds FootstepSounds_Exosuit;

        /**
          *
          * CurrentFootstepSounds değerini barındırır.
          *
          * @author Ismail <ismaiil_0234@hotmail.com>
          *
          */
        public global::FootstepSounds CurrentFootstepSounds;

        /**
         *
         * PlayerAnimation sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public PlayerAnimation PlayerAnimation;

        /**
         *
         * PlayerVehicle sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public PlayerVehicleManagement PlayerVehicle;

        /**
         *
         * PlayerAnimation sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroPlayer Player;

        /**
         *
         * CurrentVelocity değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float CurrentVelocity = 0f;

        /**
         *
         * IsUnderwater değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsUnderwater = false;

        /**
         *
         * PlayerAnimation sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float MaxFootstepRange = 25f;

        /**
         *
         * Sınıf başlatılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Awake()
        {
            this.PlayerAnimation = this.gameObject.GetComponent<PlayerAnimation>();
            this.PlayerVehicle   = this.gameObject.GetComponent<PlayerVehicleManagement>();

            this.FootstepSounds_Player  = this.gameObject.AddComponent<global::FootstepSounds>();                
            this.FootstepSounds_Exosuit = this.gameObject.AddComponent<global::FootstepSounds>();

            this.FootstepSounds_Player.CancelInvoke("TriggerSounds");
            this.FootstepSounds_Exosuit.CancelInvoke("TriggerSounds");

            this.FootstepSounds_Player.enabled  = false;
            this.FootstepSounds_Exosuit.enabled = false;

            this.FootstepSounds_Player.soundsEnabled  = false;
            this.FootstepSounds_Exosuit.soundsEnabled = false;

            this.RefreshPlayerSettings();

            this.InvokeRepeating("TriggerMultiplayerSounds", 0f, 0.05f);
        }

        /**
         *
         * Ayak seslerini tetikler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void TriggerMultiplayerSounds()
        {
            this.UpdatePlayerDatas();

            if (this.ShouldPlayStepSounds())
            {
                this.CurrentFootstepSounds = this.GetFootstepSounds();

                var magnitude = this.CurrentVelocity;
                var timeDiff  = Time.time - this.CurrentFootstepSounds.timeLastFootStep;

                if (this.CurrentFootstepSounds.clampedSpeed > 0f)
                {
                    magnitude = Mathf.Min(this.CurrentFootstepSounds.clampedSpeed, magnitude);
                }

                var requiredTime = 2.5f * this.CurrentFootstepSounds.footStepFrequencyMod / magnitude;
                if (timeDiff >= requiredTime)
                {
                    this.OnStep();

                    this.CurrentFootstepSounds.timeLastFootStep = Time.time;
                    this.CurrentFootstepSounds.triggeredLeft    = !this.CurrentFootstepSounds.triggeredLeft;
                }
            }
        }

        /**
         *
         * Ayak sesleri çalınabilir mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool ShouldPlayStepSounds()
        {
            if (this.enabled == false)
            {
                return false;
            }

            if (ZeroVector3.Distance(this.transform.position, global::Player.main.transform.position) > this.MaxFootstepRange * this.MaxFootstepRange)
            {
                return false;
            }

            if (this.Player.VehicleType == TechType.None)
            {
                if (this.Player.CurrentSurfaceType == VFXSurfaceTypes.none)
                {
                    return false;
                }

                if (!this.FootstepSounds_Player.soundsEnabled)
                {
                    return false;
                }

                if (this.Player.IsUnderwater)
                {
                    return false;
                }

                return this.CurrentVelocity > 0.2f;
            }

            if (this.Player.VehicleType == TechType.Exosuit)
            {
                this.RefreshExosuitSettings();

                if (!this.FootstepSounds_Exosuit.soundsEnabled || this.PlayerVehicle.Vehicle == null)
                {
                    return false;
                }

                var exosuit = this.PlayerVehicle.Vehicle.GetComponent<global::Exosuit>();
                if (exosuit == null || !exosuit.mainAnimator.GetBool("onGround"))
                {
                    return false;
                }

                return this.CurrentVelocity > 0.2f;
            }

            return false;
        }

        /**
         *
         * Adım atıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void OnStep()
        {
            FakeFMODByBenson.Instance.PlaySound(this.CurrentFootstepSounds.footStepSound, this.transform, this.MaxFootstepRange, this.OnStepParameters);
        }

        /**
         *
         * Footstep parametrelerini ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void OnStepParameters(FMOD.Studio.EventInstance eventInstance)
        {
            eventInstance.setParameterValueByIndex(this.CurrentFootstepSounds.speedParamIndex, this.CurrentVelocity);
            eventInstance.setParameterValueByIndex(this.CurrentFootstepSounds.surfaceParamIndex, this.Player.VehicleType == TechType.Exosuit ? (float) VFXSurfaceTypes.metal : (float) this.Player.CurrentSurfaceType);
            eventInstance.setParameterValueByIndex(this.CurrentFootstepSounds.inWaterParamIndex, this.IsUnderwater ? 1f : 0f);
            eventInstance.setParameterValue("wormlair", this.CurrentFootstepSounds.iceWormAmbience);
        }

        /**
         *
         * Player Ayarlarını yeniler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool RefreshPlayerSettings()
        {
            this.FootstepSounds_Player = this.CopyFootstepSettings(global::Player.main.footStepSounds, this.FootstepSounds_Player);
            return true;
        }

        /**
         *
         * Exosuit Ayarlarını yeniler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool RefreshExosuitSettings()
        {
            if (this.FootstepSounds_Exosuit.soundsEnabled)
            {
                return true;
            }

            var entity = Network.DynamicEntity.GetEntity(this.Player.VehicleId);
            if (entity == null)
            {
                return false;
            }

            entity.UpdateGameObject();

            if (entity.GameObject && entity.GameObject.TryGetComponent<global::FootstepSounds>(out var footstepSounds))
            {
                this.FootstepSounds_Exosuit = this.CopyFootstepSettings(footstepSounds, this.FootstepSounds_Exosuit);
                return true;
            }

            return false;
        }

        /**
         *
         * Doğru ayak ses sınıfını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private FootstepSounds GetFootstepSounds()
        {
            return this.Player.VehicleType == TechType.Exosuit ? this.FootstepSounds_Exosuit : this.FootstepSounds_Player;
        }

        /**
         *
         * Footstep ayarlarını kopyalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private FootstepSounds CopyFootstepSettings(FootstepSounds fromFootstepSounds, FootstepSounds toFootstepSounds)
        {
            toFootstepSounds.footStepSound     = fromFootstepSounds.footStepSound;
            toFootstepSounds.speedParamIndex   = fromFootstepSounds.speedParamIndex;
            toFootstepSounds.surfaceParamIndex = fromFootstepSounds.surfaceParamIndex;
            toFootstepSounds.inWaterParamIndex = fromFootstepSounds.inWaterParamIndex;

            toFootstepSounds.clampedSpeed         = fromFootstepSounds.clampedSpeed;
            toFootstepSounds.footStepFrequencyMod = fromFootstepSounds.footStepFrequencyMod;

            toFootstepSounds.soundsEnabled = true;
            return toFootstepSounds;
        }

        /**
         *
         * Oyuncu verilerini günceller.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void UpdatePlayerDatas()
        {
            this.IsUnderwater    = false;
            this.CurrentVelocity = 0f;

            if (this.Player.VehicleType == TechType.Exosuit)
            {
                var vehicle = this.PlayerVehicle.GetCurrentVehicle();
                if (vehicle != null)
                {
                    this.CurrentVelocity = vehicle.GetVelocity().magnitude;

                    var exosuit = this.PlayerVehicle.Vehicle.GetComponent<global::Exosuit>();
                    if (exosuit)
                    {
                        this.IsUnderwater = exosuit.IsUnderwater();
                    }
                }
            }
            else
            {
                this.CurrentVelocity = this.PlayerAnimation.GetVelocity().magnitude;
            }
        }
    }
}