namespace Subnautica.Firewall
{
    using System.IO;
    using System.Threading;

    using Subnautica.API.Features;

    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Log.Error($"Subnautica.Firewall - ARGS Error({args.Length}): {string.Join(" ", args)}");
                Thread.Sleep(500);
                return;
            }

            var filePath = args[0];
            if (!File.Exists(filePath))
            {
                Log.Error($"Subnautica.Firewall - Path Not Found: {filePath}");
                Thread.Sleep(500);
                return;
            }

            FirewallApi.SetupSubnauticaFirewall(filePath);

            Thread.Sleep(500);
        }
    }
}
