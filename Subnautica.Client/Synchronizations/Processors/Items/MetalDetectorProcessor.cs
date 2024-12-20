namespace Subnautica.Client.Synchronizations.Processors.Items
{
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Extensions;
    using Subnautica.Network.Core.Components;

    using ItemModel = Subnautica.Network.Models.Items;

    public class MetalDetectorProcessor : PlayerItemProcessor
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
                if (player.TechTypeInHand == TechType.MetalDetector)
                {
                    this.ProcessMetalDetector(player);
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
        private bool ProcessMetalDetector(ZeroPlayer player)
        {
            if (player.HandItemComponent == null)
            {
                return false;
            }

            var tool = player.GetHandTool<global::MetalDetector>(TechType.MetalDetector);
            if (tool == null)
            {
                return false;
            }

            var item = player.HandItemComponent.GetComponent<ItemModel.MetalDetector>();
            if (item != null)
            {
                tool.animator.SetBool(MetalDetector.animEquipped, true);
                tool.animator.SetBool(MetalDetector.animUsingTool, item.IsUsing);
                tool.animator.SetFloat(MetalDetector.animWiggle, item.Wiggle);
                tool.UpdateScreen(item.ScreenState == MetalDetector.ScreenState.Tracking ? MetalDetector.ScreenState.Default : item.ScreenState);

                if (item.TechTypeIndex == TechType.None)
                {
                    tool.SetScreenTechType(-1);
                }
                else
                {
                    tool.screenBlip.SetAlpha(0f);
                    tool.screenTechTypeText.text = global::Language.main.Get(item.TechTypeIndex.AsString());
                    tool.screenTooltipText.text  = ResourceTrackerDatabase.GetTooltip(item.TechTypeIndex);

                    var sprite = SpriteManager.Get(item.TechTypeIndex, null);
                    if (sprite != null)
                    {
                        tool.screenTechImage.sprite  = sprite;
                        tool.screenTechImage.enabled = true;
                    }
                    else
                    {
                        tool.screenTechImage.enabled = false;
                    }
                }
            }

            return true;
        }
    }
}

