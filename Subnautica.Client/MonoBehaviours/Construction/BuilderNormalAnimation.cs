namespace Subnautica.Client.MonoBehaviours.Construction
{
    using System;

    using Subnautica.API.Features;
    using UnityEngine;

    using Constructing = Subnautica.Client.Multiplayer.Constructing;

    public class BuilderNormalAnimation : MonoBehaviour
    {
        /**
         *
         * Builder sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Constructing.Builder Builder;

        /**
         *
         * Hedef miktarı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float TargetConstructedAmount { get; set; } = 0.0f;

        /**
         *
         * Hedef için kalan zamanı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float ConstructionLeftTime { get; set; } = 0.0f;

        /**
         *
         * Builder sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsActive { get; set; } = false;

        /**
         *
         * Her karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Update()
        {
            if (this.IsActive && this.Builder != null && this.Builder.Constructable != null)
            {
                this.UpdateChangedAmount();
            }
        }

        /**
         *
         * Hedef miktarı değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetTargetConstructedAmount(float targetAmount)
        {
            this.TargetConstructedAmount = targetAmount;
            this.ConstructionLeftTime = BroadcastInterval.ConstructingAmountChanged;
            this.IsActive = true;
        }

        /**
         *
         * Tamamlanma animasyonunu günceller
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void UpdateChangedAmount()
        {
            float differentAmount = this.GetDifferentAmount();
            if (differentAmount >= 0 && this.GetConstructedAmount() + differentAmount >= this.TargetConstructedAmount) 
            {
                this.Builder.Constructable.constructedAmount = this.TargetConstructedAmount;
                this.IsActive = false;
            }
            else if (differentAmount < 0 && this.GetConstructedAmount() - differentAmount <= this.TargetConstructedAmount)
            {
                this.Builder.Constructable.constructedAmount = this.TargetConstructedAmount;
                this.IsActive = false;
            }
            else
            {
                this.Builder.Constructable.constructedAmount += differentAmount;
            }

            this.Builder.Constructable.UpdateMaterial();
        }

        /**
         *
         * Zaman arasındaki güncelleme oranını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float GetDifferentAmount()
        {
            float differentAmount  = (this.TargetConstructedAmount - this.GetConstructedAmount()) / (this.ConstructionLeftTime / Time.deltaTime);
            this.ConstructionLeftTime -= Time.deltaTime;

            return differentAmount;
        }

        /**
         *
         * Yapının inşaa edilmiş miktarını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float GetConstructedAmount()
        {
            return (float) Math.Round(this.Builder.Constructable.constructedAmount, 4);
        }
    }
}
