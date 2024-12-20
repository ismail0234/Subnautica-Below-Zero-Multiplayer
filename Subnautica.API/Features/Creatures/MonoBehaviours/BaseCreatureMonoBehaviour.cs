namespace Subnautica.API.Features.Creatures.MonoBehaviours
{
    using UnityEngine;

    public class BaseMultiplayerCreature : MonoBehaviour
    {
        /**
         *
         * Çok oyunculu Yaratık sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public MultiplayerCreature MultiplayerCreature { get; private set; }

        /**
         *
         * Çok oyunculu Yaratık sınıfını değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetMultiplayerCreature(MultiplayerCreature multiplayerCreature)
        {
            this.MultiplayerCreature = multiplayerCreature;
        }
    }
}
