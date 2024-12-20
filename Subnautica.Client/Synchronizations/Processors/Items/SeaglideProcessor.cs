namespace Subnautica.Client.Synchronizations.Processors.Items
{
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Extensions;
    using Subnautica.Network.Core.Components;

    using ItemModel = Subnautica.Network.Models.Items;

    public class SeaglideProcessor : PlayerItemProcessor
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
                if (player.TechTypeInHand == TechType.Seaglide)
                {
                    this.ProcessSeaglide(player);
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
        private bool ProcessSeaglide(ZeroPlayer player)
        {
            if (player.HandItemComponent == null)
            {
                return false;
            }

            var tool = player.GetHandTool<global::Seaglide>(TechType.Seaglide);
            if (tool == null)
            {
                return false;
            }

            var item = player.HandItemComponent.GetComponent<ItemModel.Seaglide>();
            if (item != null)
            {
                if (item.IsActivated != tool.animator.GetBool("moving"))
                {
                    SafeAnimator.SetBool(tool.animator, "moving", item.IsActivated); 
                    tool.SetVFXActive(item.IsActivated);
                }

                if (item.IsLightsActivated != tool.toggleLights.lightsActive)
                {
                    var energy = tool.GetComponent<global::EnergyEffect>();
                    if (energy)
                    {
                        for (int index = 0; index < energy.toDisableOnPowerDown.Length; ++index)
                        {
                            energy.toDisableOnPowerDown[index].SetActive(item.IsLightsActivated);
                        }
                    }

                    ZeroGame.SetLightsActive(tool.toggleLights, item.IsLightsActivated, true);
                }
            }

            return true;
        }
    }
}

