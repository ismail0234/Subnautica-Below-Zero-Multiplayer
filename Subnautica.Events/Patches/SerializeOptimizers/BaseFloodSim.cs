namespace Subnautica.Events.Patches.Identity.Building
{
    using HarmonyLib;

    using Subnautica.API.Features;

    [HarmonyPatch(typeof(ProtobufSerializerPrecompiled), nameof(ProtobufSerializerPrecompiled.Serialize1725266300))]
    public class BaseFloodSim
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix()
        {
            return !Network.IsMultiplayerActive;
        }
    }
}