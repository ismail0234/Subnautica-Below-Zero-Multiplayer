namespace Subnautica.Client.MonoBehaviours.World
{
    using UnityEngine;

    public class PlanterItemComponent : MonoBehaviour
    {
        /**
         *
         * Canı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float Health { get; private set; } = -1f;

        /**
         *
         * TimeNextFruit barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float TimeNextFruit { get; private set; } = 0f;

        /**
         *
         * ActiveFruitCount barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public byte ActiveFruitCount { get; private set; } = 0;

        /**
         *
         * Başlangıç zamanını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float TimeStartGrowth { get; private set; } = 0f;

        /**
         *
         * Canı değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetHealth(float health)
        {
            this.Health = health;

            var grownPlant = this.GetComponent<Plantable>().linkedGrownPlant;
            if (grownPlant)
            {
                grownPlant.GetComponent<global::LiveMixin>().health = health;
            }
        }

        /**
         *
         * Süreyi değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetTimeNextFruit(float timeNextFruit)
        {
            this.TimeNextFruit = timeNextFruit;
        }

        /**
         *
         * ActiveFruitCount değerini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetActiveFruitCount(byte activeFruitCount)
        {
            this.ActiveFruitCount = activeFruitCount;
        }

        /**
         *
         * Başlangıç zamanını değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetStartingTime(float time)
        {
            this.TimeStartGrowth = time;
        }
    }
}
