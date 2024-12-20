namespace Subnautica.NetBird
{
    using System;
    using System.Threading;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;

    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Log.Error($"[Error] [Subnautica.Netbird] Message: Arguments are not equal ({args.Length}: {string.Join(" ", args)})");
                Console.WriteLine($"[Error] [Subnautica.Netbird] Message: Arguments are not equal ({args.Length}: {string.Join(" ", args)})");
                Thread.Sleep(1000);
                return;
            }

            var appDataPath = args[0].Trim().Replace("/", "\\");
            if (appDataPath.IsNull())
            {
                Log.Error($"[Error] [Subnautica.Netbird] Message: The path cannot be empty ({appDataPath})");
                Console.WriteLine($"[Error] [Subnautica.Netbird] Message: The path cannot be empty ({appDataPath})");
                Thread.Sleep(1000);
                return;
            }

            Paths.SetCustomAppDataPath(appDataPath);

            NetBirdApi.Instance.RemoveAndUpdateInstall();
            NetBirdApi.Instance.StartInstall();

            Thread.Sleep(500);
        }
    }
}
