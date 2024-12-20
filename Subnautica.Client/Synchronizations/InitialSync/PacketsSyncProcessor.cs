namespace Subnautica.Client.Synchronizations.Processors.Startup
{
    using System.Collections;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Client.MonoBehaviours.General;

    using UWE;

    public class PacketsSyncProcessor
    {
        /**
         *
         * Dünya yüklendiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static IEnumerator InitialStartupWorldLoadedPackets()
        {
            yield return CoroutineUtils.waitForNextFrame;

            if (MultiplayerChannelProcessor.Processors.TryGetValue(NetworkChannel.StartupWorldLoaded, out var processor) && processor.Packets.Count > 0)
            {
                processor.SetEnabled(true);

                while (processor.Packets.Count > 0)
                {
                    yield return CoroutineUtils.waitForNextFrame;
                }

                processor.SetEnabled(false);
            }
        }
        /**
         *
         * Dünya yüklendiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static IEnumerator InitialSyncPackets()
        {
            yield return CoroutineUtils.waitForNextFrame;

            foreach (var processor in MultiplayerChannelProcessor.Processors)
            {
                switch (processor.Key)
                {
                    case NetworkChannel.PlayerMovement:
                    case NetworkChannel.VehicleMovement:
                    case NetworkChannel.EntityMovement:
                    case NetworkChannel.FishMovement:
                    case NetworkChannel.EnergyTransmission:
                    case NetworkChannel.Startup:

                        Log.Info(string.Format("{0}, {1} Packet Cleared.", processor.Value.Packets.Count, processor.Key));
                        processor.Value.ClearPackets();
                    break;
                    case NetworkChannel.Construction:
                        processor.Value.SetAsyncEnabled(true);
                    break;
                }

                if (processor.Key == NetworkChannel.Startup || processor.Key == NetworkChannel.StartupWorldLoaded)
                {
                    processor.Value.SetEnabled(false);
                }
                else
                {
                    Log.Info(string.Format("{0}, {1} Packet Consuming.", processor.Value.Packets.Count, processor.Key));
                    processor.Value.SetEnabled(true);
                }
            }
        }
    }
}