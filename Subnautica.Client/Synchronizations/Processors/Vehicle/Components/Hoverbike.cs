namespace Subnautica.Client.Synchronizations.Processors.Vehicle.Components
{
    using Subnautica.API.Extensions;
    using Subnautica.Network.Models.Server;

    public class Hoverbike
    {
        /**
         *
         * Araç bileşenini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static HoverbikeUpdateComponent GetComponent(HoverbikeUpdateComponent component, global::Hoverbike hoverbike)
        {
            component.IsBoosting = hoverbike.boostFxControl.IsPlaying(0);
            component.IsJumping  = hoverbike.jumpFxControl.IsPlaying(0);
            return component;
        }
    }
}
