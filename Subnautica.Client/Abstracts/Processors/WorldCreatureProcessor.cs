namespace Subnautica.Client.Abstracts.Processors
{
    using Subnautica.API.Features;
    using Subnautica.Network.Core.Components;

    public abstract class WorldCreatureProcessor : BaseProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public abstract bool OnDataReceived(NetworkCreatureComponent packet, byte requesterId, double processTime, TechType creatureType, ushort creatureId);

        /**
         *
         * İşlemi yönlendirip çalıştırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool ExecuteProcessor(NetworkCreatureComponent packet, byte requesterId, double processTime, TechType creatureType, ushort creatureId)
        {
            if (ProcessorShared.WorldCreatureProcessors.TryGetValue(creatureType, out var processor))
            {
                processor.OnDataReceived(packet, requesterId, processTime, creatureType, creatureId);
                return true;
            }
            else
            {
                Log.Error(string.Format("WorldCreatureProcessor Not Found: {0}, UniqueId: {1}", creatureType, creatureId));
                return false;
            }
        }
    }
}