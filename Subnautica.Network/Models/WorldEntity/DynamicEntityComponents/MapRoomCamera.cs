namespace Subnautica.Network.Models.WorldEntity.DynamicEntityComponents
{
    using MessagePack;

    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.WorldEntity.DynamicEntityComponents.Shared;

    [MessagePackObject]
    public class MapRoomCamera : NetworkDynamicEntityComponent
    {
        /**
         *
         * Battery Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public PowerCell Battery { get; set; }

        /**
         *
         * Health Değeri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public LiveMixin LiveMixin { get; set; }

        /**
         *
         * IsLightEnabled Değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public bool IsLightEnabled { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public MapRoomCamera()
        {

        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public MapRoomCamera(float charge, float capacity, bool isLightEnabled, float health, float maxHealth)
        {
            this.LiveMixin = new LiveMixin(health, maxHealth);
            this.Battery   = new PowerCell();
            this.Battery.Charge   = charge;
            this.Battery.Capacity = capacity;
            this.IsLightEnabled   = isLightEnabled;
        }
    }
}