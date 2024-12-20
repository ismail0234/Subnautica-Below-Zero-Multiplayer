namespace Subnautica.Client.Synchronizations.Processors.Items
{
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Extensions;
    using Subnautica.Network.Core.Components;

    using ItemModel = Subnautica.Network.Models.Items;

    public class AirBladderProcessor : PlayerItemProcessor
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
                if (player.TechTypeInHand == TechType.AirBladder)
                {
                    this.ProcessAirBladder(player);
                }
            }
        }

        /**
         *
         * hava yastığını işler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool ProcessAirBladder(ZeroPlayer player)
        {
            if (player.HandItemComponent == null)
            {
                return false;
            }

            var tool = player.GetHandTool<global::AirBladder>(TechType.AirBladder);
            if (tool == null)
            {
                return false;
            }

            var item = player.HandItemComponent.GetComponent<ItemModel.AirBladder>();
            if (item != null)
            {
                if (item.Value != tool.animator.GetFloat(AirBladder.kAnimInflate))
                {
                    tool.animator.SetFloat(AirBladder.kAnimInflate, item.Value);
                }
            }

            return true;
        }
    }
}

