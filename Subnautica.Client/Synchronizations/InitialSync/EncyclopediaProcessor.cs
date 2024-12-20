namespace Subnautica.Client.Synchronizations.InitialSync
{
    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Client.Core;

    public class EncyclopediaProcessor
    {
        /**
         *
         * Ansiklopedi verileri yüklendikten sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEncylopediaInitialized()
        {
            if (Network.Session.Current.Encyclopedias != null)
            {
                using (EventBlocker.Create(ProcessType.EncyclopediaAdded))
                {
                    foreach (string encyclopedia in Network.Session.Current.Encyclopedias)
                    {
                        PDAEncyclopedia.Add(encyclopedia, false, false);
                    }
                }
            }
        }
    }
}
