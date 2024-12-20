namespace Subnautica.Client.Synchronizations.Processors.Items
{
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Extensions;
    using Subnautica.Network.Core.Components;

    using ItemModel   = Subnautica.Network.Models.Items;

    public class FlashLightProcessor : PlayerItemProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkPlayerItemComponent packet, byte playerId)
        {
            return true;
        }

        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnFixedUpdate()
        {
            foreach (var player in ZeroPlayer.GetPlayers())
            {
                if (player.TechTypeInHand == TechType.Flashlight)
                {
                    this.ProcessFlashLight(player);
                }
            }
        }

        /**
         *
         * Fener ışığını açar/kapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool ProcessFlashLight(ZeroPlayer player)
        {
            if (player.HandItemComponent == null)
            {
                return false;
            }

            var tool = player.GetHandTool<global::FlashLight>(TechType.Flashlight);
            if (tool == null)
            {
                return false;
            }

            var item = player.HandItemComponent.GetComponent<ItemModel.FlashLight>();
            if (item != null)
            {
                ZeroGame.SetLightsActive(tool.toggleLights, item.IsActivated, true);
            }

            return true;
        }
    }
}
