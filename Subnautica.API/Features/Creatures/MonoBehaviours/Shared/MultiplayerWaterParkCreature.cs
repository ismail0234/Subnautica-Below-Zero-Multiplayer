namespace Subnautica.API.Features.Creatures.MonoBehaviours.Shared
{
    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;

    using UnityEngine;

    public class MultiplayerWaterParkCreature : BaseMultiplayerCreature
    {
        /**
         *
         * WaterParkCreature sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::WaterParkCreature WaterParkCreature { get; set; }

        /**
         *
         * IsRegisteredWaterPark sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsRegisteredWaterPark { get; set; }

        /**
         *
         * Sınıf uyanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Awake()
        {
            this.WaterParkCreature = this.GetComponent<global::WaterParkCreature>();
        }

        /**
         *
         * Başlarken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Start()
        {
            this.HideCreature();
        }

        /**
         *
         * Aktif olurken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEnable()
        {
            this.HideCreature();
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
            if (this.IsRegisteredWaterPark == false)
            {
                this.RegisterWaterPark();
            }
            // this.MultiplayerCreature.CreatureItem.Component
        }

        /**
         *
         * Yaratık waterPark kaydını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void RegisterWaterPark()
        {
            this.IsRegisteredWaterPark = true;
        }

        /**
         *
         * Yaratığı gizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void HideCreature()
        {
            this.gameObject.transform.localScale = Vector3.zero;
        }
    }
}
