namespace Subnautica.Client.MonoBehaviours.Entity
{
    using Subnautica.API.Features;
    using Subnautica.Client.MonoBehaviours.Entity.Components;

    using UnityEngine;

    public class MultiplayerEntityTracker : MonoBehaviour
    {
        /**
         *
         * Enterpolasyon sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public EntityInterpolate Interpolate { get; set; } = new EntityInterpolate();

        /**
         *
         * Konum sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public EntityPosition Position { get; set; } = new EntityPosition();

        /**
         *
         * Görünürlük sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public EntityVisibility Visibility { get; set; } = new EntityVisibility();

        /**
         *
         * Her karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Update()
        {
            if (World.IsLoaded)
            {
                this.Visibility.Update();
                this.Position.Update();
                this.Interpolate.Update();
            }
        }
    }
}