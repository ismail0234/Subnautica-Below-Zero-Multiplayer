namespace Subnautica.Client.Synchronizations.InitialSync
{
    using System.Linq;
    using Oculus.Platform;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;

    public class TechnologyProcessor
    {
        /**
         *
         * Teknoloji verileri yüklendikten sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnKnownTechInitialized()
        {
            if (Network.Session.Current.Technologies != null)
            {
                using (EventBlocker.Create(ProcessType.TechnologyAdded))
                using (EventBlocker.Create(ProcessType.TechAnalyzeAdded))
                {
                    KnownTech.AddRange(Network.Session.Current.Technologies.Where(q => q.Value.TotalFragment == q.Value.Unlocked).Select(q => q.Key).ToList(), false);
                }
            }

            if (Network.Session.Current.AnalizedTechnologies != null)
            {
                using (EventBlocker.Create(ProcessType.TechAnalyzeAdded))
                {
                    foreach (var techType in Network.Session.Current.AnalizedTechnologies)
                    {
                        KnownTech.Analyze(techType, false, false);
                    }
                }
            }
        }

        /**
         *
         * PDA ile taranan verileri yüklendikten sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPDAScannerInitialized()
        {
            if (Network.Session.Current.Technologies != null)
            {
                using (EventBlocker.Create(ProcessType.TechnologyFragmentAdded))
                using (EventBlocker.Create(ProcessType.TechnologyAdded))
                using (EventBlocker.Create(ProcessType.ScannerCompleted))
                {
                    foreach (var technology in Network.Session.Current.Technologies)
                    {
                        if (Network.Session.Current.ScannedTechnologies != null)
                        {
                            Network.Session.Current.ScannedTechnologies.Remove(technology.Value.TechType);
                        }

                        if (technology.Value.Unlocked >= technology.Value.TotalFragment)
                        {
                            PDAScanner.complete.Add(technology.Value.TechType);
                        }
                        else
                        {
                            PDAScanner.Add(technology.Value.TechType, technology.Value.Unlocked);

                            foreach (var item in technology.Value.Fragments)
                            {
                                PDAScanner.fragments.Add(item, 1f);
                            }
                        }
                    }
                }
            }

            if (Network.Session.Current.ScannedTechnologies != null)
            {
                foreach (TechType techType in Network.Session.Current.ScannedTechnologies)
                {
                    if (!PDAScanner.complete.Contains(techType))
                    {
                        PDAScanner.complete.Add(techType);
                    }
                }
            }
        }
    }
}
