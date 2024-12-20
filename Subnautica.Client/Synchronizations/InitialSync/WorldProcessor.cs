namespace Subnautica.Client.Synchronizations.InitialSync
{
    using System;
    using System.Collections;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Client.Synchronizations.Processors.Startup;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using UnityEngine;

    using UWE;

    using ClientModel = Subnautica.Network.Models.Client;
    using ServerModel = Subnautica.Network.Models.Server;

    public class WorldProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ClientModel.WorldLoadedArgs>();
            if (packet.IsSpawnPointRequest)
            {
                CoroutineHost.StartCoroutine(LoadedWorldSpawnPointAsync(packet));
            }
            else
            {
                CoroutineHost.StartCoroutine(LoadedWorldAsync(packet));
            }
            
            return true;
        }

        /**
         *
         * Dünya yüklendiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnWorldLoaded(WorldLoadedEventArgs ev)
        {
            ev.WaitingMethods.Add(WaitingScreen.AddWaitScreen(ProcessType.WorldLoaded, LoadedWorldAction, Main.ReturnToMainMenu));
        }

        /**
         *
         * Dünya yüklenirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnWorldLoading(WorldLoadingEventArgs ev)
        {
            ev.WaitingMethods.Add(WaitingScreen.AddWaitScreen(ProcessType.WorldLoaded, LoadingWorldGlobalRootAction, Main.ReturnToMainMenu));
            ev.WaitingMethods.Add(WaitingScreen.AddWaitScreen(ProcessType.WorldLoaded, LoadingWorldSpawnPointAction, Main.ReturnToMainMenu));
        }

        /**
         *
         * Kuyruktaki işlemi başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void LoadedWorldAction()
        {
            ServerModel.WorldLoadedArgs request = new ServerModel.WorldLoadedArgs()
            {
                Images = ScreenshotProcessor.GetLocalScreenshotFileNames(),
            };

            NetworkClient.SendPacket(request);
        }

        /**
         *
         * Kuyruktaki işlemi başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void LoadingWorldGlobalRootAction()
        {
            CoroutineHost.StartCoroutine(LoadingWorldAsync());
        }

        /**
         *
         * Kuyruktaki işlemi başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void LoadingWorldSpawnPointAction()
        {
            ServerModel.WorldLoadedArgs request = new ServerModel.WorldLoadedArgs()
            {
                IsSpawnPointRequest = true,
                SpawnPointCount     = Network.WorldStreamer.GetClientSpawnPointCount(),
            };

            NetworkClient.SendPacket(request);
        }
        

        /**
         *
         * Dünya yüklendiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator LoadedWorldSpawnPointAsync(ClientModel.WorldLoadedArgs packet)
        {
            if (SpawnPointProcessor.OnSpawnPointsInitialized(packet.SpawnPoints, packet.IsSpawnPointExists))
            {
                yield return CoroutineUtils.waitForNextFrame;

                WaitingScreen.RemoveScreen(ProcessType.WorldLoaded);
            }
            else
            {
                Main.ReturnToMainMenu();
            }
        }

        /**
         *
         * Dünya yüklendiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator LoadedWorldAsync(ClientModel.WorldLoadedArgs packet)
        {
            try
            {
                ScreenshotProcessor.OnScreenshotInitialized();
                ScreenshotProcessor.OnPictureFrameInitialized(packet.Images, packet.ExistImages);
                PlayerProcessor.OnPlayerInitialized();
                PlayerProcessor.OnPlayerFirstEquipmentInitialized();
                InventoryProcessor.OnInventoryQuickSlotsInitialized();
                PlayerProcessor.OnOtherPlayersInitialized(packet.Players);
                PlayerProcessor.OnPlayerSubRootInitialized();
                PlayerProcessor.OnPlayerRespawnPointInitialized();
                NotificationProcessor.OnNotificationInitialized();
            }
            catch (Exception ex)
            {
                Log.Error($"LoadedWorldAsync Exception: {ex}");
            }

            yield return PacketsSyncProcessor.InitialStartupWorldLoadedPackets();
            yield return new WaitForSecondsRealtime(0.5f);
            yield return PacketsSyncProcessor.InitialSyncPackets();
            yield return new WaitForSecondsRealtime(0.5f);

            QualitySetting.DisableFastMode();

            Discord.UpdateRichPresence(null, ZeroLanguage.GetServerPlayerCount());

            WaitingScreen.RemoveScreen(ProcessType.WorldLoaded);
        }

        /**
         *
         * Dünyayı yükler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator LoadingWorldAsync()
        {
            QualitySetting.EnableFastMode();

            Log.Info("LoadingWorld R --> " + VRGameOptions.GetVrAnimationMode());
            yield return SpawnPointProcessor.CreateSpawnPointContainer();

            try
            {
                using (EventBlocker.Create(ProcessType.NotificationAdded))
                {
                    WorldProcessor.SetDayNightCycle(Network.Session.Current.ServerTime);
                    EncyclopediaProcessor.OnEncylopediaInitialized();
                    TechnologyProcessor.OnKnownTechInitialized();
                    TechnologyProcessor.OnPDAScannerInitialized();
                    ItemPinManagerProcessor.OnItemPinInitialized();
                    JukeboxDiskProcessor.OnJukeboxInitialized();
                    EntityProcessor.OnEntityRestrictedInitialized();
                    StoryProcessor.OnGoalsCompleteInitialized();
                    TeleporterProcessor.OnTeleporterInitialized();
                }
            }
            catch (Exception e)
            {
                Log.Error($"LoadingWorldAsync Exception: {e}");
            }

            using (EventBlocker.Create(ProcessType.NotificationAdded))
            {
                yield return InventoryProcessor.OnEquipmentInitialized();
                yield return CustomDoorways.OnDoorwaysInitialized();
            }

            using (EventBlocker.Create(ProcessType.ConstructingAmountChanged))
            using (EventBlocker.Create(ProcessType.ConstructionSynced))
            using (EventBlocker.Create(ProcessType.MetadataRequest))
            using (EventBlocker.Create(ProcessType.NotificationAdded))
            {
                yield return LifepodProcessor.OnLifePodInitialized();

                yield return ConstructionProcessor.InitialConstructions();
                yield return ConstructionProcessor.InitialInCompleteConstructions();
                yield return ConstructionProcessor.InitialBasePieceIds();
                yield return ConstructionProcessor.InitialMetadatas();
                yield return ConstructionProcessor.InitialHealths();

                yield return BaseProcessor.InitialBases();

                Log.Info("Section 1 -> Total Entity Count: " + Entity.QueueTotalCount());

                while (Entity.IsRunning)
                {
                    yield return CoroutineUtils.waitForNextFrame;
                }

                yield return new WaitForSecondsRealtime(0.5f);
            }

            using (EventBlocker.Create(ProcessType.PlayerItemAction))
            using (EventBlocker.Create(ProcessType.InventoryItem))
            {
                EntityProcessor.OnWorldItemsSpawn();
                InventoryProcessor.OnInventoryInitialized();

                Log.Info("Section 2 -> Total Entity Count: " + Entity.QueueTotalCount());

                while (Entity.IsRunning)
                {
                    yield return CoroutineUtils.waitForNextFrame;
                }

                yield return new WaitForSecondsRealtime(0.5f);
            }

            SeaTruckProcessor.OnConnectionIntialized();
            SeaTruckProcessor.OnMoonpoolExpansionTailsInitialized();
            PlayerProcessor.OnPlayerPositionInitialized();

            Log.Info("LoadingWorldAsync -> IsFirstLogin: " + Network.Session.Current.IsFirstLogin);

            WaitingScreen.RemoveScreen(ProcessType.WorldLoaded);
        }

        /**
         *
         * Dünya zamanını ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool SetDayNightCycle(double serverTime)
        {
            var elapsedTime = serverTime - Network.Session.GetWorldTime();
            if (elapsedTime < 0.0)
            {
                elapsedTime *= -1;
            }

            if (elapsedTime > 0.2)
            {
                if (BelowZeroEndGame.isActive)
                {
                    Network.Session.SetEndGameWorldTime(serverTime);
                }
                else
                {
                    DayNightCycle.main.timePassedAsDouble = serverTime;
                    PDALog.SetTime(DayNightCycle.main.timePassedAsFloat);
                }

                return true;
            }

            return false;
        }
    }
}
