namespace Subnautica.Client.Synchronizations.InitialSync
{
    using System.Collections;
    using System.Collections.Generic;

    using Subnautica.API.Features;
    using Subnautica.Network.Models.WorldStreamer;

    public class SpawnPointProcessor
    {
        /**
         *
         * Yumurtlamayacak nesneleri ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool OnSpawnPointsInitialized(HashSet<ZeroSpawnPointSimple> spawnPoints, bool isSpawnPointExists)
        {
            return Network.WorldStreamer.Initialize(spawnPoints, isSpawnPointExists);
        }

        /**
         *
         * Spawn point koordinatlarının kapsayıcısını ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static IEnumerator CreateSpawnPointContainer()
        {
            if (!Network.WorldStreamer.IsSpawnPointContainerInitialized())
            {
                Network.WorldStreamer.CreateSpawnPointContainer();

                var timing = new StopwatchItem(5000f);

                while (!timing.IsFinished())
                {
                    yield return UWE.CoroutineUtils.waitForNextFrame;

                    if (Network.WorldStreamer.IsSpawnPointContainerInitialized())
                    {
                        yield break;
                    }
                }
            }
        }
    }
}
