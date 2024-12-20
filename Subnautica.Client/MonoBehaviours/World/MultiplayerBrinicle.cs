namespace Subnautica.Client.MonoBehaviours.World
{
    using UnityEngine;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Network.Models.Storage.World.Childrens;

    public class MultiplayerBrinicle : MonoBehaviour
    {
        /**
         *
         * Brinicle nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::Brinicle Brinicle { get; set; }

        /**
         *
         * UniqueId nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private string UniqueId { get; set; }

        /**
         *
         * ScaleAmount nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float ScaleAmount { get; set; }

        /**
         *
         * IsActive nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsActive { get; set; } = true;

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Awake()
        {
            if (this.TryGetComponent<global::Brinicle>(out var brinicle))
            {
                this.Brinicle = brinicle;
                this.UniqueId = this.gameObject.GetIdentityId();

                this.CheckBrinicleState(true);
            }
        }

        /**
         *
         * Her karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Update()
        {
            if (this.IsActive && !this.IsScaleComplete())
            {
                var brinicle = this.GetBrinicle(this.UniqueId);
                if (brinicle != null)
                {
                    var scaleAmount = brinicle.GetScaleAmount(Network.Session.GetWorldTime());
                    if (this.ScaleAmount != scaleAmount)
                    {
                        this.SetScaleAmount(scaleAmount);
                    }
                }
            }
        }

        /**
         *
         * Her sabit karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void FixedUpdate()
        {
            this.CheckBrinicleState();

            if (this.IsActive)
            {
                this.Brinicle.UpdatePlayerDamage(Time.time);
            }
        }

        /**
         *
         * Brinicle durumunu kontrol eder ve değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void CheckBrinicleState(bool force = false)
        {
            var brinicle = this.GetBrinicle(this.UniqueId);
            if (brinicle == null)
            {
                this.SetState(false);
            }
            else
            {
                var isActive = brinicle.IsActive(Network.Session.GetWorldTime());
                if (this.IsActive != isActive || force)
                {
                    this.SetState(isActive, brinicle);
                }
            }
        }

        /**
         *
         * Brinicle durumunu değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SetState(bool isActive, Brinicle brinicle = null)
        {
            this.IsActive       = isActive;
            this.Brinicle.state = isActive ? global::Brinicle.State.Enabled : global::Brinicle.State.Disabled;
            this.Brinicle.model.gameObject.SetActive(isActive);
            
            if (isActive)
            {
                this.Brinicle.fxController?.Play(0);
                this.Brinicle.liveMixin.health           = brinicle.LiveMixin.Health;
                this.Brinicle.transform.localEulerAngles = brinicle.EularAngles.ToVector3();
                this.Brinicle.model.localScale           = brinicle.FullScale.ToVector3();
                this.Brinicle.fullScale                  = brinicle.FullScale.ToVector3();
            }
            else
            {

                if (this.Brinicle.breakAfterFadeOut && this.Brinicle.fxController)
                {
                    this.Brinicle.fxController.Play(1);
                }

                this.Brinicle.fxController?.Stop(0);
                this.Brinicle.UnfreezeAll();
            }

            this.SetScaleAmount(0f);
        }

        /**
         *
         * Scale durumunu değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SetScaleAmount(float amount)
        {
            this.ScaleAmount = amount;

            this.Brinicle.model.localScale = Vector3.Lerp(this.Brinicle.zeroScale, this.Brinicle.fullScale, amount);

            if (this.ScaleAmount >= 1f)
            {
                this.ScaleAmount = 99f;
            }
        }

        /**
         *
         * Scale tamamlanma durumunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsScaleComplete()
        {
            return this.ScaleAmount >= 99f;
        }

        /**
         *
         * Brinicle döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Brinicle GetBrinicle(string uniqueId)
        {
            return Network.Session.GetBrinicle(uniqueId);
        }
    }
}
