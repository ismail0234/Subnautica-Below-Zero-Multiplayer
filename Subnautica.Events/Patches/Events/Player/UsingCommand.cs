namespace Subnautica.Events.Patches.Events.Player
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::DevConsole), nameof(global::DevConsole.Submit))]
    public static class UsingCommand
    {
        private static bool Prefix(global::DevConsole __instance, string value)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            var strArray = value.Trim().Split(new char[2] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray.Length == 0)
            {
                return false;
            }

            var command = strArray[0];
            if (command.IsNull())
            {
                return false;
            }

            if (!DevConsole.commands.TryGetValue(command, out var _))
            {
                return false;
            }

            try
            {
                PlayerUsingCommandEventArgs args = new PlayerUsingCommandEventArgs(command, value);

                Handlers.Player.OnPlayerUsingCommand(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"UsingCommand.Prefix: {e}\n{e.StackTrace}");
            }

            return true;
        }
    }
}
