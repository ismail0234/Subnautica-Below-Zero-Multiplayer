namespace Subnautica.Client.Synchronizations.Processors.Items
{
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Extensions;
    using Subnautica.Network.Core.Components;

    using ItemModel   = Subnautica.Network.Models.Items;

    public class WelderProcessor : PlayerItemProcessor
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
                if (player.TechTypeInHand == TechType.Welder)
                {
                    this.ProcessWelder(player);
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
        private bool ProcessWelder(ZeroPlayer player)
        {
            if (player.HandItemComponent == null)
            {
                return false;
            }

            var tool = player.GetHandTool<global::Welder>(TechType.Welder);
            if (tool == null)
            {
                return false;
            }

            var item = player.HandItemComponent.GetComponent<ItemModel.Welder>();
            if (item != null)
            {
                if (tool.fxIsPlaying != item.IsActivated)
                {
                    tool.fxIsPlaying = item.IsActivated;

                    if (tool.fxIsPlaying)
                    {
                        tool.fxControl.Play(player.IsUnderwater ? 1 : 0);
                    }
                    else
                    {
                        tool.fxControl.StopAndDestroy(0.0f);
                    }
                }
            }

            return true;
        }
    }
}
