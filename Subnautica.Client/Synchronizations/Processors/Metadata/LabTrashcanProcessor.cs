namespace Subnautica.Client.Synchronizations.Processors.Metadata
{
    using Subnautica.Network.Models.Server;
    using Subnautica.Client.Abstracts.Processors;

    public class LabTrashcanProcessor : MetadataProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(string uniqueId, TechType techType, MetadataComponentArgs packet, bool isSilence)
        {
            return true;
        }
    }
}