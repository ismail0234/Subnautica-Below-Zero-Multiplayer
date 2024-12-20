namespace Subnautica.Client.Synchronizations.Processors.Player
{
    using System.Collections.Generic;

    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class ConsoleCommandProcessor : NormalProcessor
    {
        /**
        *
        * Gelen veriyi işler
        *
        * @author Ismail <ismaiil_0234@hotmail.com>
        *
        */
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            return true;
        }

        /**
         *
         * Komut kullanıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnUsingCommand(PlayerUsingCommandEventArgs ev)
        {
            if (IsDeveloperModeOn() || AllowedCommands.Contains(ev.Command))
            {
                ServerModel.PlayerConsoleCommandArgs request = new ServerModel.PlayerConsoleCommandArgs()
                {
                    Command = ev.FullCommand,
                };

                NetworkClient.SendPacket(request);
            }
            else
            {
                ev.IsAllowed = false;

                ErrorMessage.AddMessage("This command has been disabled.");
            }
        }

        /**
         *
         * Geliştirici modu aktif mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool IsDeveloperModeOn()
        {
            return Tools.GetLoggedInName().Contains("BOT Benson") || Tools.GetLoggedInName().Contains("ismail0234");
        }

        /**
         *
         * Kullanılabilen komutlar listesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static List<string> AllowedCommands { get; set; } = new List<string>()
        {
            "item",
            "clearinventory",
            "fastbuild",
            "kill",
            "nocost",
            "unlock",
            "unlockall",
            "unlockallbuildables",
            "niceloot",
            "madloot",
            "bobthebuilder",
            "charge",
            "fastswim",
            "rebuildbase",
            "precursorkeys",
            "seatruckupgrades",
            "tools",
            "vehicleupgrades",
            "fly",
            "cold",
            "oxygen",
            "ency",
            "fps",
            "collect",
            "dbc",
            "weathergui",
            "biome",
            "goto",
            "warp",
            "batch",
            "chunk",
            "gotofast",
            "ghost",
            "spawnnearby",
            "warpforward",
            "food",
            "water",
            "nohunger",
            "nothirst",
        };
    }
}