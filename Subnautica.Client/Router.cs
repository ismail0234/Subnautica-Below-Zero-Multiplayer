namespace Subnautica.Client
{
    using Subnautica.Client.Modules;
    using Subnautica.Events.EventArgs;

    using Initial         = Subnautica.Client.Synchronizations.InitialSync;
    using Encyclopedia    = Subnautica.Client.Synchronizations.Processors.Encyclopedia;
    using Inventory       = Subnautica.Client.Synchronizations.Processors.Inventory;
    using Player          = Subnautica.Client.Synchronizations.Processors.Player;
    using Technology      = Subnautica.Client.Synchronizations.Processors.Technology;
    using Building        = Subnautica.Client.Synchronizations.Processors.Building;
    using PDA             = Subnautica.Client.Synchronizations.Processors.PDA;
    using World           = Subnautica.Client.Synchronizations.Processors.World;
    using Metadata        = Subnautica.Client.Synchronizations.Processors.Metadata;
    using General         = Subnautica.Client.Synchronizations.Processors.General;
    using Items           = Subnautica.Client.Synchronizations.Processors.Items;
    using Vehicle         = Subnautica.Client.Synchronizations.Processors.Vehicle;
    using Creatures       = Subnautica.Client.Synchronizations.Processors.Creatures;
    using WorldEntities   = Subnautica.Client.Synchronizations.Processors.WorldEntities;
    using Story           = Subnautica.Client.Synchronizations.Processors.Story;
    using DynamicEntities = Subnautica.Client.Synchronizations.Processors.WorldEntities.DynamicEntities;

    public class Router
    {
        /**
         *
         * Eklenti aktifleştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPluginEnabled()
        {
            InviteCodeModule.OnPluginEnabled();
            MainProcess.OnPluginEnabled();
            DiscordRichPresence.OnPluginEnabled();
        }

        /**
         *
         * Oyun içi menü açılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnInGameMenuOpened(InGameMenuOpenedEventArgs ev)
        {
            InviteCodeModule.OnInGameMenuOpened(ev);
            ClientServerConnection.OnInGameMenuOpened(ev);
        }

        /**
         *
         * Oyun içi menü kapandıktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnInGameMenuClosed(InGameMenuClosedEventArgs ev)
        {
            ClientServerConnection.OnInGameMenuClosed(ev);
        }

        /**
         *
         * Arka planda çalışma ayarı değişirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSettingsRunInBackgroundChanging(SettingsRunInBackgroundChangingEventArgs ev)
        {
            ClientServerConnection.OnSettingsRunInBackgroundChanging(ev);
        }

        /**
         *
         * Sahne yüklendiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSceneLoaded(SceneLoadedEventArgs ev)
        {
            InviteCodeModule.OnSceneLoaded(ev);
            DiscordRichPresence.OnSceneLoaded(ev);
            MainProcess.OnSceneLoaded(ev);
            MultiplayerMainMenu.OnSceneLoaded(ev);
        }

        /**
         *
         * Ana menü kayıtlı oyunları sil iptal onay butonu tetiklenmesi.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnMenuSaveCancelDeleteButtonClicking(MenuSaveCancelDeleteButtonClickingEventArgs ev)
        {
            MultiplayerMainMenu.OnMenuSaveCancelDeleteButtonClicking(ev);
        }

        /**
         *
         * Ana menü kayıtlı oyunu başlat tetiklemesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnMenuSaveLoadButtonClicking(MenuSaveLoadButtonClickingEventArgs ev)
        {
            MultiplayerMainMenu.OnMenuSaveLoadButtonClicking(ev);
        }

        /**
         *
         * Ana menü kayıtlı oyunları sil butonu tetiklenmesi.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnMenuSaveDeleteButtonClicking(MenuSaveDeleteButtonClickingEventArgs ev)
        {
            MultiplayerMainMenu.OnMenuSaveDeleteButtonClicking(ev);
        }

        /**
         *
         * Ana menü kayıtlı oyun buton bilgileri tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnMenuSaveUpdateLoadedButtonState(MenuSaveUpdateLoadedButtonStateEventArgs ev)
        {
            MultiplayerMainMenu.OnMenuSaveUpdateLoadedButtonState(ev);
        }

        /**
         *
         * Ansiklopedi taraması yapıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEncyclopediaAdded(EncyclopediaAddedEventArgs ev)
        {
            Encyclopedia.AddedProcessor.OnEncyclopediaAdded(ev);
        }

        /**
         *
         * Bir eşya veya bina eşya taslağı oluşturulduğunda saniyede ortalama 60 kez tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnConstructingGhostMoved(ConstructionGhostMovedEventArgs ev)
        {
            Building.GhostMovedProcessor.OnConstructingGhostMoved(ev);
        }

        /**
         *
         * Hayalet yapı kurulmaya çalışıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnConstructingGhostTryPlacing(ConstructionGhostTryPlacingEventArgs ev)
        {
            Building.GhostTryPlacingProcessor.OnConstructingGhostTryPlacing(ev);
        }

        /**
         *
         * Oyuncu verileri tetiklendikten sonra çalışır
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPlayerUpdated(PlayerUpdatedEventArgs ev)
        {
            Player.UpdatedProcessor.OnPlayerUpdated(ev);
        }

        /**
         *
         * Teknoloji taraması tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnTechnologyAdded(TechnologyAddedEventArgs ev)
        {
            Technology.AddedProcessor.OnTechnologyAdded(ev);
        }

        /**
         *
         * Teknoloji parçası taraması tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnTechnologyFragmentAdded(TechnologyFragmentAddedEventArgs ev)
        {
            Technology.FragmentAddedProcessor.OnTechnologyFragmentAdded(ev);
        }

        /**
         *
         * Ayarlardaki pda oyun duraklatma seçeneği değiştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSettingsPdaGamePauseChanging(SettingsPdaGamePauseChangingEventArgs ev)
        {
            ClientServerConnection.OnSettingsPdaGamePauseChanging(ev);
        }

        /**
         *
         * Yapı inşaa değeri değiştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnConstructingAmountChanged(ConstructionAmountChangedEventArgs ev)
        {
            Building.AmountChangedProcessor.OnConstructingAmountChanged(ev);
        }

        /**
         *
         * Yapı inşaası tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnConstructingCompleted(ConstructionCompletedEventArgs ev)
        {
            Building.CompletedProcessor.OnConstructingCompleted(ev);
            Building.ConstructionSyncedProcessor.OnConstructingCompleted(ev);
        }

        /**
         *
         * Yapı yıkıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnConstructingRemoved(ConstructionRemovedEventArgs ev)
        {
            Building.RemovedProcessor.OnConstructingRemoved(ev);
            Building.ConstructionSyncedProcessor.OnConstructingRemoved(ev);
        }

        /**
         *
         * Oyuncu'nun envanterine bir eşya geldiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnInventoryItemAdded(InventoryItemAddedEventArgs ev)
        {
            Inventory.ItemProcessor.OnInventoryItemAdded(ev);
        }

        /**
         *
         * Oyuncu'nun envanterinden bir eşya kaldırıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnInventoryItemRemoved(InventoryItemRemovedEventArgs ev)
        {
            Inventory.ItemProcessor.OnInventoryItemRemoved(ev);
        }

        /**
         *
         * Oyuncu istatistikleri alındığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPlayerStatsUpdated(PlayerStatsUpdatedEventArgs ev)
        {
            Player.StatsProcessor.OnPlayerStatsUpdated(ev);
        }

        /**
         *
         * Oyuncu elindeki nesnenin enerjisi değiştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnToolBatteryEnergyChanged(ToolBatteryEnergyChangedEventArgs ev)
        {
            Player.ToolEnergyProcessor.OnToolBatteryEnergyChanged(ev);
        }

        /**
         *
         * Komut kullanıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnUsingCommand(PlayerUsingCommandEventArgs ev)
        {
            Player.ConsoleCommandProcessor.OnUsingCommand(ev);
        }

        /**
         *
         * Oyuncunun yeniden doğma noktası değişince tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPlayerRespawnPointChanged(PlayerRespawnPointChangedEventArgs ev)
        {
            Player.RespawnPointProcessor.OnPlayerRespawnPointChanged(ev);
        }

        /**
         *
         * Oyuncu istatistikleri alındığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPingVisibilityChanged(PlayerPingVisibilityChangedEventArgs ev)
        {
            PDA.NotificationProcessor.OnPingVisibilityChanged(ev);
        }

        /**
         *
         * Oyuncu istatistikleri alındığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPingColorChanged(PlayerPingColorChangedEventArgs ev)
        {
            PDA.NotificationProcessor.OnPingColorChanged(ev);
        }

        /**
         *
         * Oyuncu ana menüye gittiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnQuittingToMainMenu(QuittingToMainMenuEventArgs ev)
        {
            InviteCodeModule.OnQuittingToMainMenu(ev);
            MainProcess.OnQuittingToMainMenu(ev);
        }

        /**
         *
         * Oyuncu oyundan çıkarken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnQuitting()
        {

        }

        /**
         *
         * Oyuncu bir eşyayı kuşandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEquipmentEquiped()
        {
            Inventory.EquipmentProcessor.OnProcessEquipment();
        }

        /**
         *
         * Oyuncu bir eşyayı üzerinden çıkardığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEquipmentUnequiped()
        {
            Inventory.EquipmentProcessor.OnProcessEquipment();
        }

        /**
         *
         * Oyuncu bir eşyayı slotlara atadığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnQuickSlotBinded()
        {
            Inventory.QuickSlotProcessor.OnProcessQuickSlot();
        }

        /**
         *
         * Oyuncu bir eşyayı slotlardan kaldırdığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnQuickSlotUnbinded()
        {
            Inventory.QuickSlotProcessor.OnProcessQuickSlot();
        }

        /**
         *
         * Oyuncu bir eşyayı slotlardan kaldırdığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnQuickSlotActiveChanged(QuickSlotActiveChangedEventArgs ev)
        {
            Inventory.QuickSlotProcessor.OnProcessQuickSlot();
        }

        /**
         *
         * Tarama tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnScannerCompleted(ScannerCompletedEventArgs ev)
        {
            Technology.ScannerCompletedProcessor.OnScannerCompleted(ev);
        }

        /**
         *
         * Yeni pin eklendiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnItemPinAdded()
        {
            Inventory.ItemPinProcessor.OnProcessPin();
        }

        /**
         *
         * Pin kaldırıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnItemPinRemoved()
        {
            Inventory.ItemPinProcessor.OnProcessPin();
        }

        /**
         *
         * Pin taşındığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnItemPinMoved()
        {
            Inventory.ItemPinProcessor.OnProcessPin();
        }

        /**
         *
         * PDA log kaydı eklenince tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPDALogAdded(PDALogAddedEventArgs ev)
        {
            PDA.LogAddedProcessor.OnPDALogAdded(ev);
        }
        
        /**
         *
         * PDA'dan bildirim kaldırılınca/eklenince tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnNotificationToggle(NotificationToggleEventArgs ev)
        {
            PDA.NotificationProcessor.OnNotificationToggle(ev);
        }

        /**
         *
         * Teknoloji analiz edildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnTechAnalyzeAdded(TechAnalyzeAddedEventArgs ev)
        {
            PDA.TechAnalyzeAddedProcessor.OnTechAnalyzeAdded(ev);
        }

        /**
         *
         * Yapı inşaası ilk kaldırma işlemi olduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnDeconstructionBegin(DeconstructionBeginEventArgs ev)
        {
            Building.DeconstructionBeginProcessor.OnDeconstructionBegin(ev);
        }

        /**
         *
         * Mobilya inşaası ilk kaldırma işlemi olduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnFurnitureDeconstructionBegin(FurnitureDeconstructionBeginEventArgs ev)
        {
            Building.FurnitureDeconstructionBeginProcessor.OnFurnitureDeconstructionBegin(ev);
        }

        /**
         *
         * Tuvalet kapağı açılıp/kapandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnToiletSwitchToggle(ToiletSwitchToggleEventArgs ev)
        {
            Metadata.ToiletProcessor.OnToiletSwitchToggle(ev);
        }

        /**
         *
         * Oyuncak aktif/pasif olduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEmmanuelPendulumSwitchToggle(EmmanuelPendulumSwitchToggleEventArgs ev)
        {
            Metadata.EmmanuelPendulumProcessor.OnEmmanuelPendulumSwitchToggle(ev);
        }

        /**
         *
         * Terapi nesnesi aktif/pasif olduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnAromatherapyLampSwitchToggle(AromatherapyLampSwitchToggleEventArgs ev)
        {
            Metadata.AromatherapyProcessor.OnAromatherapyLampSwitchToggle(ev);
        }

        /**
         *
         * Ocak nesnesi aktif/pasif olduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSmallStoveSwitchToggle(SmallStoveSwitchToggleEventArgs ev)
        {
            Metadata.SmallStoveProcessor.OnSmallStoveSwitchToggle(ev);
        }

        /**
         *
         * Lavabo nesnesi aktif/pasif olduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSinkSwitchToggle(SinkSwitchToggleEventArgs ev)
        {
            Metadata.SinkProcessor.OnSinkSwitchToggle(ev);
        }

        /**
         *
         * Banyo nesnesi aktif/pasif olduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnShowerSwitchToggle(ShowerSwitchToggleEventArgs ev)
        {
            Metadata.ShowerProcessor.OnShowerSwitchToggle(ev);
        }

        /**
         *
         * Kardan adam yok edilirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSnowmanDestroying(SnowmanDestroyingEventArgs ev)
        {
            Metadata.SnowmanProcessor.OnSnowmanDestroying(ev);
            World.StaticEntityProcessor.OnSnowmanDestroying(ev);
        }

        /**
         *
         * Tabela da veri değişimi olduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSignDataChanged(SignDataChangedEventArgs ev)
        {
            Metadata.SignProcessor.OnSignDataChanged(ev);
            Items.DeployableStorageProcessor.OnSignDataChanged(ev);
            Vehicle.SeaTruckStorageModuleProcessor.OnSignDataChanged(ev);
            Vehicle.SeaTruckFabricatorModuleProcessor.OnSignDataChanged(ev);
        }

        /**
         *
         * Resim çervesine resim eklenirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPictureFrameImageSelecting(PictureFrameImageSelectingEventArgs ev)
        {
            Metadata.PictureFrameProcessor.OnPictureFrameImageSelecting(ev);
        }

        /**
         *
         * Yatağı kullanabilirlik durumunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBedIsCanSleepChecking(BedIsCanSleepCheckingEventArgs ev)
        {
            Metadata.BedProcessor.OnBedIsCanSleepChecking(ev);
        }

        /**
         *
         * Kullanıcı yatağa tıkladığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBedEnterInUseMode(BedEnterInUseModeEventArgs ev)
        {
            Metadata.BedProcessor.OnBedEnterInUseMode(ev);
            Vehicle.SeaTruckSleeperModuleProcessor.OnBedEnterInUseMode(ev);
        }

        /**
         *
         * Kullanıcı yatak'dan kalktığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBedExitInUseMode(BedExitInUseModeEventArgs ev)
        {
            Metadata.BedProcessor.OnBedExitInUseMode(ev);
            Vehicle.SeaTruckSleeperModuleProcessor.OnBedExitInUseMode(ev);
        }

        /**
         *
         * Şarkı kutusunda veri değişimi olduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnJukeboxUsed(JukeboxUsedEventArgs ev)
        {
            Metadata.JukeboxProcessor.OnJukeboxUsed(ev);
            Vehicle.SeaTruckSleeperModuleProcessor.OnJukeboxUsed(ev);
        }

        /**
         *
         * Şarkı diski açıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnJukeboxDiskAdded(JukeboxDiskAddedEventArgs ev)
        {
            PDA.JukeboxDiskAddedProcessor.OnJukeboxDiskAdded(ev);
        }

        /**
         *
         * Fabricator nesnesinden bir eşya alındığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCrafterItemPickup(CrafterItemPickupEventArgs ev)
        {
            Metadata.CrafterProcessor.OnCrafterItemPickup(ev);
        }

        /**
         *
         * Fabricator nesnesi kapandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCrafterClosed(CrafterClosedEventArgs ev)
        {
            Metadata.CrafterProcessor.OnCrafterClosed(ev);
        }

        /**
         *
         * Fabricator nesnesinde üretim başladığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCrafterBegin(CrafterBeginEventArgs ev)
        {
            Metadata.CrafterProcessor.OnCrafterBegin(ev);
        }

        /**
         *
         * Fabricator nesnesinde üretim sona erdiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCrafterEnded(CrafterEndedEventArgs ev)
        {
            Metadata.CrafterProcessor.OnCrafterEnded(ev);
        }

        /**
         *
         * Oturma animasyonu başladığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBenchSitdown(BenchSitdownEventArgs ev)
        {
            Metadata.BenchProcessor.OnBenchSitdown(ev);
        }

        /**
         *
         * Kalkma animasyonu başladığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBenchStandup(BenchStandupEventArgs ev)
        {
            Metadata.BenchProcessor.OnBenchStandup(ev);
        }

        /**
         *
         * Spotlight oluştuktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSpotLightInitialized(SpotLightInitializedEventArgs ev)
        {
            Metadata.SpotLightProcessor.OnSpotLightInitialized(ev);
        }

        /**
         *
         * Techlight oluştuktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnTechLightInitialized(TechLightInitializedEventArgs ev)
        {
            Metadata.TechlightProcessor.OnTechLightInitialized(ev);
        }

        /**
         *
         * Map Room tarama başlatılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBaseMapRoomScanStarting(BaseMapRoomScanStartingEventArgs ev)
        {
            Metadata.BaseMapRoomProcessor.OnBaseMapRoomScanStarting(ev);
        }

        /**
         *
         * Map Room tarama iptal edilirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBaseMapRoomScanStopping(BaseMapRoomScanStoppingEventArgs ev)
        {
            Metadata.BaseMapRoomProcessor.OnBaseMapRoomScanStopping(ev);
        }

        /**
         *
         * Kamera değiştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBaseMapRoomCameraChanging(MapRoomCameraChangingEventArgs ev)
        {
            Metadata.BaseMapRoomProcessor.OnBaseMapRoomCameraChanging(ev);
        }

        /**
         *
         * Yeni kaynak keşfedildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBaseMapRoomResourceDiscovering(BaseMapRoomResourceDiscoveringEventArgs ev)
        {
            General.ResourceDiscoverProcessor.OnBaseMapRoomResourceDiscovering(ev);
        }

        /**
         *
         * Harita odası başlatıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBaseMapRoomInitialized(BaseMapRoomInitializedEventArgs ev)
        {
            General.ResourceDiscoverProcessor.OnBaseMapRoomInitialized(ev);
        }

        /**
         *
         * Expansion -> Seatruck ayrılma tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBaseMoonpoolExpansionUndockingTimelineCompleting(BaseMoonpoolExpansionUndockingTimelineCompletingEventArgs ev)
        {
            Metadata.MoonpoolProcessor.OnBaseMoonpoolExpansionUndockingTimelineCompleting(ev);
        }

        /**
         *
         * Expansion -> Seatruck yanaşma tamamlanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBaseMoonpoolExpansionDockingTimelineCompleting(BaseMoonpoolExpansionDockingTimelineCompletingEventArgs ev)
        {
            Metadata.MoonpoolProcessor.OnBaseMoonpoolExpansionDockingTimelineCompleting(ev);
        }

        /**
         *
         * Expansion -> Seatruck kuyruk kenetlenme işlemi tamamlanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBaseMoonpoolExpansionDockTail(BaseMoonpoolExpansionDockTailEventArgs ev)
        {
            Metadata.MoonpoolProcessor.OnBaseMoonpoolExpansionDockTail(ev);
        }

        /**
         *
         * Expansion -> Seatruck kuyruk ayrılma işlemi tamamlanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBaseMoonpoolExpansionUndockTail(BaseMoonpoolExpansionUndockTailEventArgs ev)
        {
            Metadata.MoonpoolProcessor.OnBaseMoonpoolExpansionUndockTail(ev);
        }

        /**
         *
         * Oyuncu üse girdiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPlayerBaseEntered(PlayerBaseEnteredEventArgs ev)
        {
            Player.SubrootToggleProcessor.OnPlayerBaseEntered(ev);
        }

        /**
         *
         * Oyuncu üs'den ayrıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPlayerBaseExited(PlayerBaseExitedEventArgs ev)
        {
            Player.SubrootToggleProcessor.OnPlayerBaseExited(ev);
        }

        /**
         *
         * Resim çerçevesi tıklanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPictureFrameOpening(PictureFrameOpeningEventArgs ev)
        {
            Metadata.PictureFrameProcessor.OnPictureFrameOpening(ev);
        }

        /**
         *
         * Fabricator nesnesi açıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCrafterOpening(CrafterOpeningEventArgs ev)
        {
            Metadata.CrafterProcessor.OnCrafterOpening(ev);
        }

        /**
         *
         * Şarj cihazına tıklanınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnChargerOpening(ChargerOpeningEventArgs ev)
        {
            Metadata.ChargerProcessor.OnChargerOpening(ev);
        }

        /**
         *
         * Depolama Açılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnStorageOpening(StorageOpeningEventArgs ev)
        {
            General.StorageOpenProcessor.OnStorageOpening(ev);
        }

        /**
         *
         * Depolamaya eşya eklenirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnStorageItemAdding(StorageItemAddingEventArgs ev)
        {
            General.LifepodProcessor.OnStorageItemAdding(ev);
            Metadata.StorageProcessor.OnStorageItemAdding(ev);
            Metadata.AquariumProcessor.OnStorageItemAdding(ev);
            Metadata.BioReactorProcessor.OnStorageItemAdding(ev);
            Metadata.FridgeProcessor.OnStorageItemAdding(ev);
            Metadata.BaseMapRoomProcessor.OnStorageItemAdding(ev);
            Items.DeployableStorageProcessor.OnStorageItemAdding(ev);
            Items.SpyPenguinProcessor.OnStorageItemAdding(ev);
            Vehicle.SeaTruckFabricatorModuleProcessor.OnStorageItemAdding(ev);
            Vehicle.SeaTruckStorageModuleProcessor.OnStorageItemAdding(ev);
            Vehicle.SeaTruckAquariumModuleProcessor.OnStorageItemAdding(ev);
            Vehicle.ExosuitStorageProcessor.OnStorageItemAdding(ev);

            // Old Storage System
            Metadata.FiltrationMachineProcessor.OnStorageItemAdding(ev);
            Metadata.CoffeeVendingMachineProcessor.OnStorageItemAdding(ev);
        }

        /**
         *
         * Depolama'dan eşya kaldırıldırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnStorageItemRemoving(StorageItemRemovingEventArgs ev)
        {
            General.LifepodProcessor.OnStorageItemRemoving(ev);
            Metadata.StorageProcessor.OnStorageItemRemoving(ev);
            Metadata.AquariumProcessor.OnStorageItemRemoving(ev);
            Metadata.FridgeProcessor.OnStorageItemRemoving(ev);
            Metadata.BaseMapRoomProcessor.OnStorageItemRemoving(ev);
            Items.DeployableStorageProcessor.OnStorageItemRemoving(ev);
            Items.SpyPenguinProcessor.OnStorageItemRemoving(ev);
            Vehicle.SeaTruckFabricatorModuleProcessor.OnStorageItemRemoving(ev);
            Vehicle.SeaTruckStorageModuleProcessor.OnStorageItemRemoving(ev);
            Vehicle.SeaTruckAquariumModuleProcessor.OnStorageItemRemoving(ev);
            Vehicle.ExosuitStorageProcessor.OnStorageItemRemoving(ev);

            // Old Storage System
            Metadata.FiltrationMachineProcessor.OnStorageItemRemoving(ev);
            Metadata.CoffeeVendingMachineProcessor.OnStorageItemRemoving(ev);
        }

        /**
         *
         * Nükleer Depolamaya eşya eklendiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnNuclearReactorItemAdded(NuclearReactorItemAddedEventArgs ev)
        {
            Metadata.NuclearReactorProcessor.OnNuclearReactorItemAdded(ev);
        }

        /**
         *
         * Nükleer Depolamadan eşya kaldırıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnNuclearReactorItemRemoved(NuclearReactorItemRemovedEventArgs ev)
        {
            Metadata.NuclearReactorProcessor.OnNuclearReactorItemRemoved(ev);
        }

        /**
         *
         * Tabela seçildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSignSelect(SignSelectEventArgs ev)
        {
            Metadata.SignProcessor.OnSignSelect(ev);
            Items.DeployableStorageProcessor.OnSignSelect(ev);
            Vehicle.SeaTruckStorageModuleProcessor.OnSignSelect(ev);
            Vehicle.SeaTruckFabricatorModuleProcessor.OnSignSelect(ev);
        }

        /**
         *
         * Tabela seçimi kaldırıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSignDeselect(SignDeselectEventArgs ev)
        {
            General.InteractProcessor.OnSignDeselect(ev);
        }

        /**
         *
         * PDA kapatıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnClosing(PDAClosingEventArgs ev)
        {
            Metadata.ChargerProcessor.OnClosing(ev);
            General.InteractProcessor.OnClosing(ev);
        }

        /**
         *
         * Şarj cihazına pil eklendiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnChargerItemAdded(ChargerItemAddedEventArgs ev)
        {
            Metadata.ChargerProcessor.OnChargerItemAdded(ev);
        }

        /**
         *
         * Şarj cihazın'dan pil kaldırılınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnChargerItemRemoved(ChargerItemRemovedEventArgs ev)
        {
            Metadata.ChargerProcessor.OnChargerItemRemoved(ev);
        }

        /**
         *
         * Hoverbike inşaa edilirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnHoverpadHoverbikeSpawning(HoverpadHoverbikeSpawningEventArgs ev)
        {
            Metadata.HoverpadProcessor.OnHoverpadHoverbikeSpawning(ev);
        }

        /**
         *
         * Bir eşya geri dönüştürüldüğünde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnRecyclotronRecycle(RecyclotronRecycleEventArgs ev)
        {
            Metadata.RecyclotronProcessor.OnRecyclotronRecycle(ev);
        }

        /**
         *
         * Saksıya bitki eklenince tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPlanterItemAdded(PlanterItemAddedEventArgs ev)
        {
            Metadata.PlanterProcessor.OnPlanterItemAdded(ev);
        }

        /**
         *
         * Saksı büyüdüğünde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPlanterProgressCompleted(PlanterProgressCompletedEventArgs ev)
        {
            Metadata.PlanterProcessor.OnPlanterProgressCompleted(ev);
        }

        /**
         *
         * Saksı'daki toplanabilir bitki büyüdüğünde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPlanterGrowned(PlanterGrownedEventArgs ev)
        {
            Metadata.PlanterProcessor.OnPlanterGrowned(ev);
        }

        /**
         *
         * Bölme kapısı açılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBulkheadOpening(BulkheadOpeningEventArgs ev)
        {
            Metadata.BulkheadProcessor.OnBulkheadOpening(ev);
            WorldEntities.BulkheadDoorProcessor.OnBulkheadOpening(ev);
        }

        /**
         *
         * Bölme kapısı kapanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBulkheadClosing(BulkheadClosingEventArgs ev)
        {
            Metadata.BulkheadProcessor.OnBulkheadClosing(ev);
            WorldEntities.BulkheadDoorProcessor.OnBulkheadClosing(ev);
        }

        /**
         *
         * Termal zambak alanı kontrol edilirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnThermalLilyRangeChecking(ThermalLilyRangeCheckingEventArgs ev)
        {
            WorldEntities.ThermalLilyProcessor.OnThermalLilyRangeChecking(ev);
        }

        /**
         *
         * Termal zambak açısı kontrol edilirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnThermalLilyAnimationAnglesChecking(ThermalLilyAnimationAnglesCheckingEventArgs ev)
        {
            WorldEntities.ThermalLilyProcessor.OnThermalLilyAnimationAnglesChecking(ev);
        }

        /**
         *
         * Oksijen bitkisine tıklandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnOxygenPlantClicking(OxygenPlantClickingEventArgs ev)
        {
            WorldEntities.OxygenPlantProcessor.OnOxygenPlantClicking(ev);
        }

        /**
         *
         * Işınlayıcı terminali aktif edilirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnTeleporterTerminalActivating(TeleporterTerminalActivatingEventArgs ev)
        {
            World.PrecursorTeleporterProcessor.OnTeleporterTerminalActivating(ev);
        }

        /**
         *
         * Işınlayıcı başlatılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnTeleporterInitialized(TeleporterInitializedEventArgs ev)
        {
            World.PrecursorTeleporterProcessor.OnTeleporterInitialized(ev);
        }

        /**
         *
         * Oyuncu ışınlanma başladıktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPrecursorTeleporterUsed()
        {
            World.PrecursorTeleporterProcessor.OnPrecursorTeleporterUsed();
        }

        /**
         *
         * Oyuncu ışınlanma tamamlandıktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPrecursorTeleportationCompleted()
        {
            World.PrecursorTeleporterProcessor.OnPrecursorTeleportationCompleted();
        }

        /**
         *
         * Asansör başlatıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnElevatorInitialized(ElevatorInitializedEventArgs ev)
        {
            MultiplayerElevator.OnElevatorInitialized(ev);
        }

        /**
         *
         * Nesne spawn olurken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEntitySpawning(EntitySpawningEventArgs ev)
        {
            World.EntitySpawnProcessor.OnEntitySpawning(ev);
            World.EntitySlotSpawnProcessor.OnEntitySpawning(ev);
            General.LifepodProcessor.OnEntitySpawning(ev);
        }

        /**
         *
         * Nesne Slotu doğarken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEntitySlotSpawning(EntitySlotSpawningEventArgs ev)
        {
            World.EntitySlotSpawnProcessor.OnEntitySlotSpawning(ev);
        }

        /**
         *
         * Nesne spawn olduktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEntitySpawned(EntitySpawnedEventArgs ev)
        {
            World.EntitySpawnProcessor.OnEntitySpawned(ev);
            World.EntitySlotSpawnProcessor.OnEntitySpawned(ev);
            World.BrinicleProcessor.OnEntitySpawned(ev);
            Metadata.CrafterProcessor.OnEntitySpawned(ev);
        }

        /**
         *
         * Bıçak kullanıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnKnifeUsing(KnifeUsingEventArgs ev)
        {
            Items.KnifeProcessor.OnKnifeUsing(ev);
        }

        /**
         *
         * Dünya yüklenirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnWorldLoading(WorldLoadingEventArgs ev)
        {
            Initial.WorldProcessor.OnWorldLoading(ev);
        }

        /**
         *
         * Cell yüklendikten sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCellLoading(CellLoadingEventArgs ev)
        {
            World.CellProcessor.OnCellLoading(ev);
        }

        /**
         *
         * Cell kaldırılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCellUnLoading(CellUnLoadingEventArgs ev)
        {
            World.CellProcessor.OnCellUnLoading(ev);
        }

        /**
         *
         * Dünya yüklendiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnWorldLoaded(WorldLoadedEventArgs ev)
        {
            Initial.WorldProcessor.OnWorldLoaded(ev);
            PingLatency.OnWorldLoaded();
        }

        /**
         *
         * Oyuncu nesne taraması tamamlandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEntityScannerCompleted(EntityScannerCompletedEventArgs ev)
        {
            World.EntityScannerProcessor.OnEntityScannerCompleted(ev);
        }

        /**
         *
         * Alterra pda nesnesi aldıktan sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnAlterraPdaPickedUp(AlterraPdaPickedUpEventArgs ev)
        {
            World.StaticEntityProcessor.OnAlterraPdaPickedUp(ev);
        }

        /**
         *
         * Müzik nesnesi alındığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnJukeboxDiskPickedUp(JukeboxDiskPickedUpEventArgs ev)
        {
            World.StaticEntityProcessor.OnJukeboxDiskPickedUp(ev);
        }

        /**
         *
         * Oyuncu yerden eşya aldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPlayerItemPickedUp(PlayerItemPickedUpEventArgs ev)
        {
            World.StaticEntityProcessor.OnPlayerItemPickedUp(ev);
            World.EntitySlotSpawnProcessor.OnPlayerItemPickedUp(ev);
            World.CosmeticItemProcessor.OnPlayerItemPickedUp(ev);
            Player.ItemPickupProcessor.OnPlayerItemPickedUp(ev);
            Items.PipeSurfaceFloaterProcessor.OnPlayerItemPickedUp(ev);
            Metadata.BaseMapRoomProcessor.OnPlayerItemPickedUp(ev);
        }

        /**
         *
         * Oyuncu bir nesneyi tararken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnScannerUsing(ScannerUsingEventArgs ev)
        {
            Items.ScannerProcessor.OnScannerUsing(ev);
        }

        /**
         *
         * Drone camera denize bırakılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnDroneCameraDeploying(DroneCameraDeployingEventArgs ev)
        {
            Items.MapRoomCameraProcessor.OnDroneCameraDeploying(ev);
        }

        /**
         *
         * Boru yüzey yüzdürücü bırakılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPipeSurfaceFloaterDeploying(PipeSurfaceFloaterDeployingEventArgs ev)
        {
            Items.PipeSurfaceFloaterProcessor.OnPipeSurfaceFloaterDeploying(ev);
        }

        /**
         *
         * Boru bırakılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnOxygenPipePlacing(OxygenPipePlacingEventArgs ev)
        {
            Items.PipeSurfaceFloaterProcessor.OnOxygenPipePlacing(ev);
        }

        /**
         *
         * Bir araç veya yapı tamir edilirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnWelding(WeldingEventArgs ev)
        {
            World.WelderProcessor.OnWelding(ev);
        }

        /**
         *
         * Tedarik sandığı açıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSupplyCrateOpened(SupplyCrateOpenedEventArgs ev)
        {
            WorldEntities.SupplyCrateProcessor.OnSupplyCrateOpened(ev);
        }

        /**
         *
         * Veri kutusundan tasarım alındığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnDataboxItemPickedUp(DataboxItemPickedUpEventArgs ev)
        {
            WorldEntities.DataboxProcessor.OnDataboxItemPickedUp(ev);
        }

        /**
         *
         * Bir nesne hasar aldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnTakeDamaging(TakeDamagingEventArgs ev)
        {
            WorldEntities.DestroyableEntityProcessor.OnTakeDamaging(ev);
            WorldEntities.DestroyableDynamicEntityProcessor.OnTakeDamaging(ev);
            Metadata.PlanterProcessor.OnTakeDamaging(ev);
            Building.BaseHullStrengthProcessor.OnTakeDamaging(ev);
            Building.HealthProcessor.OnTakeDamaging(ev);
            Creatures.HealthProcessor.OnTakeDamaging(ev);
            Vehicle.HealthProcessor.OnTakeDamaging(ev);
            World.BrinicleProcessor.OnTakeDamaging(ev);
        }

        /**
         *
         * Bitki hasat değildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnFruitHarvesting(FruitHarvestingEventArgs ev)
        {
            Metadata.PlanterProcessor.OnFruitHarvesting(ev);
            WorldEntities.FruitHarvestProcessor.OnFruitHarvesting(ev);
        }

        /**
         *
         * Bitki hasat değildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnGrownPlantHarvesting(GrownPlantHarvestingEventArgs ev)
        {
            Metadata.PlanterProcessor.OnGrownPlantHarvesting(ev);
        }

        /**
         *
         * Oyuncu Animasyonu değiştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPlayerAnimationChanged(PlayerAnimationChangedEventArgs ev)
        {
            Player.AnimationChangedProcessor.OnPlayerAnimationChanged(ev);
        }

        /**
         *
         * Oyuncu bir nesneyi bırakırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPlayerItemDroping(PlayerItemDropingEventArgs ev)
        {
            Player.ItemDropProcessor.OnPlayerItemDroping(ev);
            Metadata.BaseWaterParkProcessor.OnPlayerItemDroping(ev);
        }

        /**
         *
         * Oyuncu ekranı tamamen karardığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSleepScreenStartingCompleted()
        {
            Metadata.BedProcessor.OnSleepScreenStartingCompleted();
        }

        /**
         *
         * Oyuncu ekranı aydınlanma başlatıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSleepScreenStopingStarted()
        {
            Metadata.BedProcessor.OnSleepScreenStopingStarted();
        }

        /**
         *
         * Intro kontrolü yapılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnIntroChecking(IntroCheckingEventArgs ev)
        {
            General.IntroProcessor.OnIntroChecking(ev);
        }

        /**
         *
         * Lifepod bölgesi seçilirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnLifepodZoneSelecting(LifepodZoneSelectingEventArgs ev)
        {
            General.LifepodProcessor.OnLifepodZoneSelecting(ev);
        }

        /**
         *
         * Lifepod spawnlanma için kontrol eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnLifepodZoneCheck(LifepodZoneCheckEventArgs ev)
        {
            General.LifepodProcessor.OnLifepodZoneCheck(ev);
        }

        /**
         *
         * Lifepod enterpolasyon işleminde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnLifepodInterpolation(LifepodInterpolationEventArgs ev)
        {
            General.LifepodProcessor.OnLifepodInterpolation(ev);
        }

        /**
         *
         * Oyuncu bir araca binmeye yada inmeye çalıştığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnUseableDiveHatchClicking(UseableDiveHatchClickingEventArgs ev)
        {
            Player.UseableDiveHatchProcessor.OnUseableDiveHatchClicking(ev);
        }

        /**
         *
         * Oyuncu bir araca bindiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEnteredInterior(PlayerEnteredInteriorEventArgs ev)
        {
            Player.InteriorToggleProcessor.OnEnteredInterior(ev);
        }

        /**
         *
         * Oyuncu bir araçtan indiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnExitedInterior(PlayerExitedInteriorEventArgs ev)
        {
            Player.InteriorToggleProcessor.OnExitedInterior(ev);
        }

        /**
         *
         * Üs dayanıklılığı düştüğünde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBaseHullStrengthCrushing(BaseHullStrengthCrushingEventArgs ev)
        {
            Building.BaseHullStrengthProcessor.OnBaseHullStrengthCrushing(ev);
        }

        /**
         *
         * Renk değiştirme paleti seçimden çıktığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSubNameInputDeselected(SubNameInputDeselectedEventArgs ev)
        {
            Metadata.BaseControlRoomProcessor.OnSubNameInputDeselected(ev);
            Metadata.HoverpadProcessor.OnSubNameInputDeselected(ev);
            Metadata.MoonpoolProcessor.OnSubNameInputDeselected(ev);
        }

        /**
         *
         * Renk değiştirme paleti seçildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSubNameInputSelecting(SubNameInputSelectingEventArgs ev)
        {
            Metadata.BaseControlRoomProcessor.OnSubNameInputSelecting(ev);
            Metadata.HoverpadProcessor.OnSubNameInputSelecting(ev);
            Metadata.MoonpoolProcessor.OnSubNameInputSelecting(ev);
        }

        /**
         *
         * Kontrol odasındaki mini haritaya tıklanınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBaseControlRoomMinimapUsing(BaseControlRoomMinimapUsingEventArgs ev)
        {
            Metadata.BaseControlRoomProcessor.OnBaseControlRoomMinimapUsing(ev);
        }

        /**
         *
         * Kontrol odasındaki mini haritadan ayrılınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBaseControlRoomMinimapExiting(BaseControlRoomMinimapExitingEventArgs ev)
        {
            Metadata.BaseControlRoomProcessor.OnBaseControlRoomMinimapExiting(ev);
        }

        /**
         *
         * Kontrol odasındaki mini harita hücresine basıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBaseControlRoomCellPowerChanging(BaseControlRoomCellPowerChangingEventArgs ev)
        {
            Metadata.BaseControlRoomProcessor.OnBaseControlRoomCellPowerChanging(ev);
        }

        /**
         *
         * Kontrol odasındaki mini harita hareket ettiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBaseControlRoomMinimapMoving(BaseControlRoomMinimapMovingEventArgs ev)
        {
            Metadata.BaseControlRoomProcessor.OnBaseControlRoomMinimapMoving(ev);
        }

        /**
         *
         * Constructor bırakılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnConstructorDeploying(ConstructorDeployingEventArgs ev)
        {
            Items.ConstructorProcessor.OnConstructorDeploying(ev);
        }

        /**
         *
         * Constructor menüyü açtığında/kapattığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnConstructorEngageToggle(ConstructorEngageToggleEventArgs ev)
        {
            Items.ConstructorProcessor.OnConstructorEngageToggle(ev);
        }

        /**
         *
         * Bir araç yapılmaya çalışıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnConstructorCrafting(ConstructorCraftingEventArgs ev)
        {
            Items.ConstructorProcessor.OnConstructorCrafting(ev);
        }

        /**
         *
         * Oyuncu merdivene tırmanmaya çalışılınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPlayerClimbing(PlayerClimbingEventArgs ev)
        {
            Player.ClimbProcessor.OnPlayerClimbing(ev);
        }

        /**
         *
         * Yükseltme konsoluna tıklanınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnUpgradeConsoleOpening(UpgradeConsoleOpeningEventArgs ev)
        {
            Vehicle.UpgradeConsoleProcessor.OnUpgradeConsoleOpening(ev);
        }

        /**
         *
         * Yükseltme konsoluna modül eklenince tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnUpgradeConsoleModuleAdded(UpgradeConsoleModuleAddedEventArgs ev)
        {
            Vehicle.UpgradeConsoleProcessor.OnUpgradeConsoleModuleAdded(ev);
        }

        /**
         *
         * Yükseltme konsolundan modül kaldırılınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnUpgradeConsoleModuleRemoved(UpgradeConsoleModuleRemovedEventArgs ev)
        {
            Vehicle.UpgradeConsoleProcessor.OnUpgradeConsoleModuleRemoved(ev);
        }

        /**
         *
         * Hoverbike, pad üzerine takılınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnHoverpadDocking(HoverpadDockingEventArgs ev)
        {
            Metadata.HoverpadProcessor.OnHoverpadDocking(ev);
        }

        /**
         *
         * Hoverbike, pad üzerinden ayrılınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnHoverpadUnDocking(HoverpadUnDockingEventArgs ev)
        {
            Metadata.HoverpadProcessor.OnHoverpadUnDocking(ev);
        }

        /**
         *
         * Hoverbike yakınına gelince veya ayrılınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnHoverpadShowroomTriggering(HoverpadShowroomTriggeringEventArgs ev)
        {
            Metadata.HoverpadProcessor.OnHoverpadShowroomTriggering(ev);
        }

        /**
         *
         * Araca binerken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnVehicleEntering(VehicleEnteringEventArgs ev)
        {
            Vehicle.EnterProcessor.OnVehicleEntering(ev);
        }

        /**
         *
         * Araçtan inerken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnVehicleExited(VehicleExitedEventArgs ev)
        {
            Vehicle.ExitProcessor.OnVehicleExited(ev);
        }

        /**
         *
         * Araç konumu güncellendiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnVehicleUpdated(VehicleUpdatedEventArgs ev)
        {
            Vehicle.UpdatedProcessor.OnVehicleUpdated(ev);
        }

        /**
         *
         * Oyuncu öldüğünde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPlayerDead(PlayerDeadEventArgs ev)
        {
            Player.DeadProcessor.OnPlayerDead(ev);
        }

        /**
         *
         * Oyuncu doğduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPlayerSpawned()
        {
            Player.SpawnProcessor.OnPlayerSpawned();
        }

        /**
         *
         * Hoverbike bırakıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnHoverbikeDeploying(HoverbikeDeployingEventArgs ev)
        {
            Items.HoverbikeProcessor.OnHoverbikeDeploying(ev);
        }

        /**
         *
         * Pil yerleştirildiğinde/çıkarıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEnergyMixinSelecting(EnergyMixinSelectingEventArgs ev)
        {
            Vehicle.BatteryProcessor.OnEnergyMixinSelecting(ev);
        }

        /**
         *
         * Pil yerleştirilme alanına tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEnergyMixinClicking(EnergyMixinClickingEventArgs ev)
        {
            Vehicle.BatteryProcessor.OnEnergyMixinClicking(ev);
        }

        /**
         *
         * Exosuit ile zıplandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnExosuitJumping(ExosuitJumpingEventArgs ev)
        {
            Vehicle.ExosuitJumpProcessor.OnExosuitJumping(ev);
        }

        /**
         *
         * Pil yerleştirilme kapatıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEnergyMixinClosed(EnergyMixinClosedEventArgs ev)
        {
            General.InteractProcessor.OnEnergyMixinClosed(ev);
        }

        /**
         *
         * Su geçirmez depoyu bıraktığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnDeployableStorageDeploying(DeployableStorageDeployingEventArgs ev)
        {
            Items.DeployableStorageProcessor.OnDeployableStorageDeploying(ev);
        }

        /**
         *
         * Led ışığı yere konulduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnLEDLightDeploying(LEDLightDeployingEventArgs ev)
        {
            Items.LEDLightProcessor.OnLEDLightDeploying(ev);
        }

        /**
         *
         * Beacon yere konulduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBeaconDeploying(BeaconDeployingEventArgs ev)
        {
            Items.BeaconProcessor.OnBeaconDeploying(ev);
        }
        
        /**
         *
         * Beacon adı değişince tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBeaconLabelChanged(BeaconLabelChangedEventArgs ev)
        {
            Items.BeaconProcessor.OnBeaconLabelChanged(ev);
        }
        
        /**
         *
         * Spy Penguin bırakıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSpyPenguinDeploying(SpyPenguinDeployingEventArgs ev)
        {
            Items.SpyPenguinProcessor.OnSpyPenguinDeploying(ev);
        }
        
        /**
         *
         * Spy Penguin bir nesne aldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSpyPenguinItemPickedUp(SpyPenguinItemPickedUpEventArgs ev)
        {
            Items.SpyPenguinProcessor.OnSpyPenguinItemPickedUp(ev);
        }
        
        /**
         *
         * Spy Penguin kar avcısından kar kürkü alırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSpyPenguinSnowStalkerInteracting(SpyPenguinSnowStalkerInteractingEventArgs ev)
        {
            Items.SpyPenguinProcessor.OnSpyPenguinSnowStalkerInteracting(ev);
        }

        /**
         *
         * Spy Penguin bir animasyon halinde iken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSpyPenguinItemGrabing(SpyPenguinItemGrabingEventArgs ev)
        {
            Vehicle.UpdatedProcessor.OnSpyPenguinItemGrabing(ev);
        }

        /**
         *
         * İşaret fişeti yere konulduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnFlareDeploying(FlareDeployingEventArgs ev)
        {
            Items.FlareProcessor.OnFlareDeploying(ev);
        }
        
        /**
         *
         * Thumper yere konulurken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnThumperDeploying(ThumperDeployingEventArgs ev)
        {
            Items.ThumperProcessor.OnThumperDeploying(ev);
        }
        
        /**
         *
         * Işınlanma işlemi başladığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnTeleportationToolUsed(TeleportationToolUsedEventArgs ev)
        {
            Items.TeleportationToolProcessor.OnTeleportationToolUsed(ev);
        }
        /**
         *
         * Araç ışıkları yanıp/söndüğünde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnVehicleLightChanged(LightChangedEventArgs ev)
        {
            Vehicle.LightProcessor.OnVehicleLightChanged(ev);
        }
        
        /**
         *
         * Araca binerken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnVehicleInteriorToggle(VehicleInteriorToggleEventArgs ev)
        {
            Vehicle.InteriorProcessor.OnVehicleInteriorToggle(ev);
        }
        
        /**
         *
         * Seatruck modülü bağlanırken/ayrılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSeaTruckConnecting(SeaTruckConnectingEventArgs ev)
        {
            Vehicle.SeaTruckConnectionProcessor.OnSeaTruckConnecting(ev);
        }
        
        /**
         *
         * SeaTruck modül bağlantı kesme animasyonunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSeaTruckDetaching(SeaTruckDetachingEventArgs ev)
        {
            Vehicle.SeaTruckConnectionProcessor.OnSeaTruckDetaching(ev);
        }
        
        /**
         *
         * Exosuit ile yerden nesne alındığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnExosuitItemPickedUp(ExosuitItemPickedUpEventArgs ev)
        {
            Vehicle.ExosuitStorageProcessor.OnExosuitItemPickedUp(ev);
        }
        
        /**
         *
         * Exosuit ile maden kazarken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnExosuitDrilling(ExosuitDrillingEventArgs ev)
        {
            Vehicle.ExosuitDrillProcessor.OnExosuitDrilling(ev);
        }
        
        /**
         *
         * SeaTruck/Exosuit rıhtıma yanaşırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnVehicleDocking(VehicleDockingEventArgs ev)
        {
            Metadata.MoonpoolProcessor.OnVehicleDocking(ev);
            Vehicle.SeaTruckDockingModuleProcessor.OnVehicleDocking(ev);
        }
        
        /**
         *
         * SeaTruck/Exosuit rıhtımdan ayrılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnVehicleUndocking(VehicleUndockingEventArgs ev)
        {
            Metadata.MoonpoolProcessor.OnVehicleUndocking(ev);
            Vehicle.SeaTruckDockingModuleProcessor.OnVehicleUndocking(ev);
        }
        
        /**
         *
         * SeaTruck Resim çerçevesi açılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSeaTruckPictureFrameOpening(SeaTruckPictureFrameOpeningEventArgs ev)
        {
            Vehicle.SeaTruckSleeperModuleProcessor.OnSeaTruckPictureFrameOpening(ev);
        }
        
        /**
         *
         * SeaTruck Resim çerçevesi resim seçilirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSeaTruckPictureFrameImageSelecting(SeaTruckPictureFrameImageSelectingEventArgs ev)
        {
            Vehicle.SeaTruckSleeperModuleProcessor.OnSeaTruckPictureFrameImageSelecting(ev);
        }
        
        /**
         *
         * MapRoomCamera yanaşırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnMapRoomCameraDocking(MapRoomCameraDockingEventArgs ev)
        {
            Metadata.BaseMapRoomProcessor.OnMapRoomCameraDocking(ev);
        }
        
        /**
         *
         * SeaTruck modülü başlatılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSeaTruckModuleInitialized(SeaTruckModuleInitializedEventArgs ev)
        {
            Vehicle.SeaTruckSleeperModuleProcessor.OnSeaTruckModuleInitialized(ev);
            Vehicle.SeaTruckDockingModuleProcessor.OnSeaTruckModuleInitialized(ev);
        }
        
        /**
         *
         * Kaynak kırıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBreakableResourceBreaking(BreakableResourceBreakingEventArgs ev)
        {
            World.EntitySlotSpawnProcessor.OnBreakableResourceBreaking(ev);
        }
        
        /**
         *
         * Köprü sol konsola tıklanınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBridgeFluidClicking(BridgeFluidClickingEventArgs ev)
        {
            Story.BridgeProcessor.OnBridgeFluidClicking(ev);
        }
        
        /**
         *
         * Koprü terminaline tıklanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBridgeTerminalClicking(BridgeTerminalClickingEventArgs ev)
        {
            Story.BridgeProcessor.OnBridgeTerminalClicking(ev);
        }
        
        /**
         *
         * Koprü spawn olduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnBridgeInitialized(BridgeInitializedEventArgs ev)
        {
            Story.BridgeProcessor.OnBridgeInitialized(ev);
        }
        
        /**
         *
         * Radyo kulesi test modülü takılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnRadioTowerTOMUsing(RadioTowerTOMUsingEventArgs ev)
        {
            Story.RadioTowerProcessor.OnRadioTowerTOMUsing(ev);
        }

        /**
         *
         * Hedef tetiklenirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnStoryGoalTriggering(StoryGoalTriggeringEventArgs ev)
        {
            Story.TriggerProcessor.OnStoryGoalTriggering(ev);
        }
        
        /**
         *
         * Hikaye sinyali spawnlanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnStorySignalSpawning(StorySignalSpawningEventArgs ev)
        {
            Story.SignalProcessor.OnStorySignalSpawning(ev);
        }
        
        /**
         *
         * Hikaye cinematic tetiklenirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCinematicTriggering(CinematicTriggeringEventArgs ev)
        {
            Story.CinematicProcessor.OnCinematicTriggering(ev);
        }
        
        /**
         *
         * Hikaye çağrısı kabul/red edildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnStoryCalling(StoryCallingEventArgs ev)
        {
            Story.CallProcessor.OnStoryCalling(ev);
        }
        
        /**
         *
         * Terminal tıklanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnStoryHandClicking(StoryHandClickingEventArgs ev)
        {
            Story.InteractProcessor.OnStoryHandClicking(ev);
        }
        
        /**
         *
         * Cinematic başladığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnStoryCinematicStarted(StoryCinematicStartedEventArgs ev)
        {
            Story.PlayerVisibilityProcessor.OnStoryCinematicStarted(ev);
        }
        
        /**
         *
         * Cinematic bittiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnStoryCinematicCompleted(StoryCinematicCompletedEventArgs ev)
        {
            Story.PlayerVisibilityProcessor.OnStoryCinematicCompleted(ev);
        }
        
        /**
         *
         * MobileExtractorMachine başlatıldığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnMobileExtractorMachineInitialized()
        {
            Story.FrozenCreatureProcessor.OnMobileExtractorMachineInitialized();
        }
        
        /**
         *
         * anti virüs örneği eklenirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnMobileExtractorMachineSampleAdding(MobileExtractorMachineSampleAddingEventArgs ev)
        {
            Story.FrozenCreatureProcessor.OnMobileExtractorMachineSampleAdding(ev);
        }

        /**
         *
         * Konsola tıklanınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnMobileExtractorConsoleUsing(MobileExtractorConsoleUsingEventArgs ev)
        {
            Story.FrozenCreatureProcessor.OnMobileExtractorConsoleUsing(ev);
        }

        /**
         *
         * Kalkan üssüne girildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnShieldBaseEnterTriggering(ShieldBaseEnterTriggeringEventArgs ev)
        {
            Story.ShieldBaseProcessor.OnShieldBaseEnterTriggering(ev);
        }

        /**
         *
         * Kesici kullanılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnLaserCutterUsing(LaserCutterEventArgs ev)
        {
            WorldEntities.LaserCutterProcessor.OnLaserCutterUsing(ev);
        }
        
        /**
         *
         * Mühürlü bir nesne başlatılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSealedInitialized(SealedInitializedEventArgs ev)
        {
            WorldEntities.LaserCutterProcessor.OnSealedInitialized(ev);
        }

        /**
         *
         * Asansöre tıklandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnElevatorCalling(ElevatorCallingEventArgs ev)
        {
            WorldEntities.ElevatorProcessor.OnElevatorCalling(ev);
        }

        /**
         *
         * Bir nesne yok edildiğinde içinden başka nesne çıkarken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnSpawnOnKilling(SpawnOnKillingEventArgs ev)
        {
            World.SpawnOnKillProcessor.OnSpawnOnKilling(ev);
        }

        /**
         *
         * Hava durumu profili değiştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnWeatherProfileChanged(WeatherProfileChangedEventArgs ev)
        {
            World.WeatherProcessor.OnWeatherProfileChanged(ev);
        }

        /**
         *
         * Basınç hasarı alınınca tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCrushDamaging(CrushDamagingEventArgs ev)
        {
            Vehicle.HealthProcessor.OnCrushDamaging(ev);
        }

        /**
         *
         * Kozmetik dünyaya yerleştirilirken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCosmeticItemPlacing(CosmeticItemPlacingEventArgs ev)
        {
            World.CosmeticItemProcessor.OnCosmeticItemPlacing(ev);
        }

        /**
         *
         * Oyuncu donduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPlayerFreezed(PlayerFreezedEventArgs ev)
        {
            Player.FreezeProcessor.OnPlayerFreezed(ev);
        }

        /**
         *
         * Oyuncu donma sona erdiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnPlayerUnfreezed()
        {
            Player.FreezeProcessor.OnPlayerUnfreezed();
        }

        /**
         *
         * Balık animasyonu değiştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCreatureAnimationChanged(CreatureAnimationChangedEventArgs ev)
        {
            Creatures.AnimationChangedProcessor.OnCreatureAnimationChanged(ev);
        }

        /**
         *
         * Balina sürme başlarken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnGlowWhaleRideStarting(GlowWhaleRideStartingEventArgs ev)
        {
            Creatures.GlowWhaleProcessor.OnGlowWhaleRideStarting(ev);
        }

        /**
         *
         * Balina sürme sona erdiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnGlowWhaleRideStoped(GlowWhaleRideStopedEventArgs ev)
        {
            Creatures.GlowWhaleProcessor.OnGlowWhaleRideStoped(ev);
        }

        /**
         *
         * Balina göz animasyonu başlarken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnGlowWhaleEyeCinematicStarting(GlowWhaleEyeCinematicStartingEventArgs ev)
        {
            Creatures.GlowWhaleProcessor.OnGlowWhaleEyeCinematicStarting(ev);
        }

        /**
         *
         * Balina SFX tetiklendiğine çalışır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnGlowWhaleSFXTriggered(GlowWhaleSFXTriggeredEventArgs ev)
        {
            Creatures.GlowWhaleProcessor.OnGlowWhaleSFXTriggered(ev);
        }

        /**
         *
         * CrashFish patlarken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCrashFishInflating(CrashFishInflatingEventArgs ev)
        {
            Creatures.CrashFishProcessor.OnCrashFishInflating(ev);
        }

        /**
         *
         * Hipnoz başlarken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnLilyPaddlerHypnotizeStarting(LilyPaddlerHypnotizeStartingEventArgs ev)
        {
            Creatures.LilyPaddlerProcessor.OnLilyPaddlerHypnotizeStarting(ev);
        }

        /**
         *
         * Balık donarken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnFreezing(CreatureFreezingEventArgs ev)
        {
            Creatures.FreezeProcessor.OnFreezing(ev);
        }

        /**
         *
         * Yaratık bazı sesler oynarken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCallSoundTriggering(CreatureCallSoundTriggeringEventArgs ev)
        {
            Creatures.CallSoundProcessor.OnCallSoundTriggering(ev);
        }

        /**
         *
         * Yaratık en sonki hedefine saldırı başlatırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCreatureAttackLastTargetStarting(CreatureAttackLastTargetStartingEventArgs ev)
        {
            Creatures.AttackLastTargetProcessor.OnCreatureAttackLastTargetStarting(ev);
        }

        /**
         *
         * Yaratık en sonki hedefine saldırı başlatma sona erdiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCreatureAttackLastTargetStopped(CreatureAttackLastTargetStoppedEventArgs ev)
        {
            Creatures.AttackLastTargetProcessor.OnCreatureAttackLastTargetStopped(ev);
        }

        /**
         *
         * Leviathan bir nesne ile temasa geçtiğinde (saldırdığında) tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnLeviathanMeleeAttacking(CreatureLeviathanMeleeAttackingEventArgs ev)
        {
            Creatures.LeviathanMeleeAttackProcessor.OnLeviathanMeleeAttacking(ev);
        }

        /**
         *
         * Yaratık bir nesne ile temasa geçtiğinde (saldırdığında) tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnMeleeAttacking(CreatureMeleeAttackingEventArgs ev)
        {
            Creatures.MeleeAttackProcessor.OnMeleeAttacking(ev);
        }

        /**
         *
         * Yaratık etkinleştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCreatureEnabled(CreatureEnabledEventArgs ev)
        {
            Creatures.VoidLeviathanProcessor.OnCreatureEnabled(ev);
        }

        /**
         *
         * Yaratık pasifleştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCreatureDisabled(CreatureDisabledEventArgs ev)
        {
            Creatures.VoidLeviathanProcessor.OnCreatureDisabled(ev);
        }
    }
}