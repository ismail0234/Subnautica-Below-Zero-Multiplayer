namespace Subnautica.Events.Patches.Events.Furnitures
{
    using HarmonyLib;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    public static class ToggleOnClickShared
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool TriggerEvent(global::ToggleOnClick __instance, bool isSwitchOn)
        {
            if(!Network.IsMultiplayerActive)
            {
                return true;
            }

            var constructable = __instance.GetComponentInParent<Constructable>();
            if (constructable == null)
            {
                return true;
            }

            if (constructable.techType == TechType.Toilet)
            {
                ToiletSwitchToggleEventArgs args = new ToiletSwitchToggleEventArgs(Network.Identifier.GetIdentityId(constructable.gameObject), isSwitchOn);

                Handlers.Furnitures.OnToiletSwitchToggle(args);
                return args.IsAllowed;
            }
            else if (constructable.techType == TechType.EmmanuelPendulum)
            {
                EmmanuelPendulumSwitchToggleEventArgs args = new EmmanuelPendulumSwitchToggleEventArgs(Network.Identifier.GetIdentityId(constructable.gameObject), isSwitchOn);

                Handlers.Furnitures.OnEmmanuelPendulumSwitchToggle(args);
                return args.IsAllowed;
            }
            else if (constructable.techType == TechType.AromatherapyLamp)
            {
                AromatherapyLampSwitchToggleEventArgs args = new AromatherapyLampSwitchToggleEventArgs(Network.Identifier.GetIdentityId(constructable.gameObject), isSwitchOn);

                Handlers.Furnitures.OnAromatherapyLampSwitchToggle(args);
                return args.IsAllowed;
            }
            else if (constructable.techType == TechType.SmallStove)
            {
                SmallStoveSwitchToggleEventArgs args = new SmallStoveSwitchToggleEventArgs(Network.Identifier.GetIdentityId(constructable.gameObject), isSwitchOn);

                Handlers.Furnitures.OnSmallStoveSwitchToggle(args);
                return args.IsAllowed;
            }
            else if (constructable.techType == TechType.Sink)
            {
                SinkSwitchToggleEventArgs args = new SinkSwitchToggleEventArgs(Network.Identifier.GetIdentityId(constructable.gameObject), isSwitchOn);

                Handlers.Furnitures.OnSinkSwitchToggle(args);
                return args.IsAllowed;
            }
            else if (constructable.techType == TechType.Shower)
            {
                ShowerSwitchToggleEventArgs args = new ShowerSwitchToggleEventArgs(Network.Identifier.GetIdentityId(constructable.gameObject), isSwitchOn);

                Handlers.Furnitures.OnShowerSwitchToggle(args);
                return args.IsAllowed;
            }

            return true;
        }
    }


    [HarmonyPatch(typeof(global::ToggleOnClick), nameof(global::ToggleOnClick.SwitchOn))]
    public static class ToggleOnClickSwitchOn
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::ToggleOnClick __instance)
        {
            return ToggleOnClickShared.TriggerEvent(__instance, true);
        }
    }


    [HarmonyPatch(typeof(global::ToggleOnClick), nameof(global::ToggleOnClick.SwitchOff))]
    public static class ToggleOnClickSwitchOff
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::ToggleOnClick __instance)
        {
            return ToggleOnClickShared.TriggerEvent(__instance, false);
        }
    }
}
