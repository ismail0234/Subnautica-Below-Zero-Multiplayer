namespace Subnautica.Events.Patches.Fixes.Encyclopedia
{
    using HarmonyLib;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;

    [HarmonyPatch(typeof(PDAEncyclopedia), nameof(PDAEncyclopedia.Initialize))]
    public static class Encyclopedia
    {
        /**
         *
         * Bloklanmış olayı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static EventBlocker Blocker = null;

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Prefix(PDAData pdaData)
        {
            if(Network.IsMultiplayerActive)
            {
                Blocker = EventBlocker.Create(ProcessType.EncyclopediaAdded);
            }
        }

        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Postfix(PDAData pdaData)
        {
            if (Blocker != null)
            {
                Blocker.Dispose();
            }
        }
    }
}
