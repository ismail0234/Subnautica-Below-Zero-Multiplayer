namespace Subnautica.Client.Synchronizations.Processors.Vehicle.Components
{
    using System.Collections.Generic;

    using Subnautica.Network.Models.Server;

    public class SpyPenguin
    {
        /**
         *
         * Araç bileşenini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static SpyPenguinUpdateComponent GetComponent(SpyPenguinUpdateComponent component, global::SpyPenguin spyPenguin, List<string> animations)
        {
            component.IsSelfieMode = spyPenguin.selfieMode;
            component.SelfieNumber = global::Player.main.playerAnimator.GetBool("selfies") ? global::Player.main.playerAnimator.GetFloat("selfie_number") : -1;
            component.Animations   = animations;
            return component;
        }
    }
}
