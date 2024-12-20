namespace Subnautica.Events.Patches.Events.World
{
    using HarmonyLib;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using System;

    [HarmonyPatch]
    public static class ElevatorCalling
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Rocket), nameof(global::Rocket.ElevatorControlButtonActivate))]
        private static bool ElevatorControlButtonActivate(global::Rocket __instance)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            if (__instance.elevatorState != Rocket.RocketElevatorStates.AtTop && __instance.elevatorState != Rocket.RocketElevatorStates.AtBottom)
            {
                __instance.liftErrorSFX.Play();
                return false;
            }

            try
            {
                ElevatorCallingEventArgs args = new ElevatorCallingEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject, false), __instance.elevatorState != Rocket.RocketElevatorStates.AtTop);

                Handlers.World.OnElevatorCalling(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"ElevatorCalling.ElevatorControlButtonActivate: {e}\n{e.StackTrace}");
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::Rocket), nameof(global::Rocket.CallElevator))]
        private static bool CallElevator(global::Rocket __instance, bool up)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            bool flag = true;
            if (up && __instance.elevatorState != Rocket.RocketElevatorStates.AtBottom)
            {
                flag = false;
            }

            if (!up && __instance.elevatorState != Rocket.RocketElevatorStates.AtTop)
            {
                flag = false;
            }

            if (!flag)
            {
                __instance.liftErrorSFX.Play();
                return false;
            }

            try
            {
                ElevatorCallingEventArgs args = new ElevatorCallingEventArgs(Network.Identifier.GetIdentityId(__instance.gameObject, false), up);

                Handlers.World.OnElevatorCalling(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"ElevatorCalling.CallElevator: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }
}