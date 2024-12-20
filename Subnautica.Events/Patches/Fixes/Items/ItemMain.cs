namespace Subnautica.Events.Patches.Fixes.Items
{
    using HarmonyLib;

    public class ItemMain
    {
        /**
         *
         * OnHolster Kontrolü yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool CheckOnHolster(global::PlayerTool __instance)
        {
            if (__instance.GetComponentInParent<global::PingInstance>())
            {
                return false;
            }

            return true;
        }
    }
}