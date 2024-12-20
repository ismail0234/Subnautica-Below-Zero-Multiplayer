namespace Subnautica.API.Features
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using NetFwTypeLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features.Helper;
    
    public class FirewallApi
    {
        /**
         *
         * IsInitialized değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool IsInitialized { get; set; } = true;

        /**
         *
         * Son çıktıyı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string LastOutput { get; private set; } = "";

        /**
         *
         * Son hatayı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string LastError { get; private set; } = "";

        /**
         *
         * SubnauticaBelowZeroDescription değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static string SubnauticaBelowZeroDescription { get; set; } = "Subnautica BZ Multiplayer by BOT Benson";

        /**
         *
         * SubnauticaBelowZeroId değerini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static string SubnauticaBelowZeroId { get; set; } = "subnauticazero";

        /**
         *
         * Subnautica Firewall Kurulumunu yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SetupFirewallWithAdminPerms(string filePath)
        {
            FirewallApi.ExecuteCommand($@"Subnautica.Firewall.exe ""{filePath}""", true, false);
        }

        /**
         *
         * Subnautica Firewall Kurulumunu yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SetupSubnauticaFirewall(string filePath)
        {
            filePath = filePath.Replace("/", "\\");

            FirewallApi.ExecuteCommand($@"advfirewall firewall delete rule name=""{FirewallApi.SubnauticaBelowZeroId}""");
            FirewallApi.ExecuteCommand($@"advfirewall firewall add rule name=""{FirewallApi.SubnauticaBelowZeroId}"" description=""{FirewallApi.SubnauticaBelowZeroDescription}"" dir=in action=allow program=""{filePath}"" enable=yes");
        }

        /**
         *
         * Kurallar doğru yapılandırılmış mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsSubnauticaFirewallOk(string path)
        {
            path = path.Replace("/", "\\");

            var rules = FirewallApi.GetSubnauticaRules();
            if (rules.Count != 1)
            {
                return false;
            }

            return rules.Any(q => q.IsEnabled && q.IsUdp && q.IsTcp && q.IsPublicProfile && q.IsPrivateProfile && q.IsDomainProfile && q.IsAllow && q.Path == path && q.Description == FirewallApi.SubnauticaBelowZeroDescription);
        }

        /**
         *
         * Subnautica Kuralları döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static List<FirewallItemFormat> GetSubnauticaRules()
        {
            return GetRules(FirewallApi.SubnauticaBelowZeroId);
        }

        /**
         *
         * Kuralları döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static List<FirewallItemFormat> GetRules(string name)
        {
            var items = new List<FirewallItemFormat>();

            try
            {
                var tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
                var fwPolicy2     = (INetFwPolicy2) Activator.CreateInstance(tNetFwPolicy2);

                foreach (INetFwRule rule in fwPolicy2.Rules)
                {
                   if (rule.Name == name)
                    {
                        var profile = (NET_FW_PROFILE_TYPE2_) rule.Profiles;

                        items.Add(new FirewallItemFormat() {
                            Name        = rule.Name,
                            Description = rule.Description,
                            Path        = rule.ApplicationName,
                            IsEnabled   = rule.Enabled,
                            IsAllow     = rule.Action == NET_FW_ACTION_.NET_FW_ACTION_ALLOW,
                            IsTcp       = rule.Protocol == (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_ANY || rule.Protocol == (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP,
                            IsUdp       = rule.Protocol == (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_ANY || rule.Protocol == (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP,

                            IsPrivateProfile = profile.HasFlag(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL) || profile.HasFlag(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PRIVATE),
                            IsPublicProfile  = profile.HasFlag(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL) || profile.HasFlag(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PUBLIC),
                            IsDomainProfile  = profile.HasFlag(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL) || profile.HasFlag(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_DOMAIN),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Is Firewall Ok?: {ex}");
            }

            return items;
        }

        /**
         *
         * komut çalıştırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void ExecuteCommand(string command, bool useCorePath = false, bool isNetShCommand = true, bool silence = true)
        {
            try
            {
                var process = new System.Diagnostics.Process();

                if (isNetShCommand)
                {
                    process.StartInfo.FileName  = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "netsh.exe");
                    process.StartInfo.Arguments = command;
                }
                else
                {
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = string.Format("/c {0}", command);
                }

                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError  = true;
                process.StartInfo.UseShellExecute = false;

                if (silence)
                {
                    process.StartInfo.WindowStyle    = System.Diagnostics.ProcessWindowStyle.Hidden;
                    process.StartInfo.CreateNoWindow = true;
                }

                if (useCorePath)
                {
                    process.StartInfo.WorkingDirectory = Paths.GetLauncherGameCorePath();
                }

                process.Start();

                var strOutput = process.StandardOutput.ReadToEnd();
                var strError  = process.StandardError.ReadToEnd();

                process.WaitForExit(5000);

                LastOutput = strOutput?.Trim();
                LastError  = strError?.Trim();
            }
            catch (Exception ex)
            {
                LastOutput = "";
                LastError = $"System.Exception: {ex.Message}:{ex.InnerException}";
            }

            if (LastError.IsNotNull())
            {
                Log.Error($"FirewallApi Exception: {LastError}, SC: " + silence);
            }
        }
    }
}
