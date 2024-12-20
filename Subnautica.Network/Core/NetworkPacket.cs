namespace Subnautica.Network.Models.Core
{
    using System;

    using LiteNetLib;

    using MessagePack;

    using Subnautica.API.Enums;

    using ClientModel = Subnautica.Network.Models.Client;
    using ServerModel = Subnautica.Network.Models.Server;

    [Union(0, typeof(ClientModel.JoiningServerArgs))]
    [Union(2, typeof(ClientModel.InteractArgs))]
    [Union(3, typeof(ClientModel.AnotherPlayerConnectedArgs))]
    [Union(4, typeof(ClientModel.WorldLoadedArgs))]
    [Union(5, typeof(ClientModel.StoryPlayerVisibilityArgs))]
    [Union(6, typeof(ClientModel.WeatherChangedArgs))]
    [Union(7, typeof(ClientModel.ConnectionRejectArgs))]
    [Union(100, typeof(ServerModel.ConstructionAmountChangedArgs))]
    [Union(101, typeof(ServerModel.ConstructionCompletedArgs))]
    [Union(102, typeof(ServerModel.ConstructionGhostMovingArgs))]
    [Union(103, typeof(ServerModel.ConstructionGhostTryPlacingArgs))]
    [Union(104, typeof(ServerModel.ConstructionRemovedArgs))]
    [Union(105, typeof(ServerModel.SleepTimeSkipArgs))]
    [Union(106, typeof(ServerModel.EncyclopediaAddedArgs))]
    [Union(107, typeof(ServerModel.ExosuitDrillArgs))]
    [Union(108, typeof(ServerModel.InventoryEquipmentArgs))]
    [Union(109, typeof(ServerModel.InventoryItemArgs))]
    [Union(110, typeof(ServerModel.InventoryQuickSlotItemArgs))]
    [Union(111, typeof(ServerModel.ItemDropArgs))]
    [Union(112, typeof(ServerModel.ItemPinArgs))]
    [Union(113, typeof(ServerModel.JoiningServerArgs))]
    [Union(114, typeof(ServerModel.PDALogAddedArgs))]
    [Union(115, typeof(ServerModel.IntroStartArgs))]
    [Union(116, typeof(ServerModel.EntityScannerCompletedArgs))]
    [Union(117, typeof(ServerModel.PlayerDisconnectedArgs))]
    [Union(118, typeof(ServerModel.PlayerStatsArgs))]
    [Union(119, typeof(ServerModel.PlayerUpdatedArgs))]
    [Union(120, typeof(ServerModel.ScannerCompletedArgs))]
    [Union(121, typeof(ServerModel.TechnologyAddedArgs))]
    [Union(122, typeof(ServerModel.TechnologyFragmentAddedArgs))]
    [Union(123, typeof(ServerModel.NotificationAddedArgs))]
    [Union(124, typeof(ServerModel.TechAnalyzeAddedArgs))]
    [Union(125, typeof(ServerModel.BatteryChargerTransmissionArgs))]
    [Union(126, typeof(ServerModel.EnergyTransmissionArgs))]
    [Union(127, typeof(ServerModel.DeconstructionBeginArgs))]
    [Union(128, typeof(ServerModel.FurnitureDeconstructionBeginArgs))]
    [Union(129, typeof(ServerModel.MetadataComponentArgs))]
    [Union(130, typeof(ServerModel.WorldLoadedArgs))]
    [Union(131, typeof(ServerModel.JukeboxDiskAddedArgs))]
    [Union(132, typeof(ServerModel.InteractArgs))]
    [Union(133, typeof(ServerModel.SubrootToggleArgs))]
    [Union(134, typeof(ServerModel.LifepodArgs))]
    [Union(135, typeof(ServerModel.StaticEntityPickedUpArgs))]
    [Union(136, typeof(ServerModel.WorldEntityActionArgs))]
    [Union(137, typeof(ServerModel.PlayerItemActionArgs))]
    [Union(138, typeof(ServerModel.PlayerAnimationChangedArgs))]
    [Union(139, typeof(ServerModel.FiltrationMachineTransmissionArgs))]
    [Union(140, typeof(ServerModel.UseableDiveHatchArgs))]
    [Union(141, typeof(ServerModel.InteriorToggleArgs))]
    [Union(142, typeof(ServerModel.BaseHullStrengthTakeDamagingArgs))]
    [Union(143, typeof(ServerModel.WeatherChangedArgs))]
    [Union(144, typeof(ServerModel.ItemPickupArgs))]
    [Union(145, typeof(ServerModel.WorldDynamicEntityPositionArgs))]
    [Union(146, typeof(ServerModel.WorldDynamicEntityOwnershipChangedArgs))]
    [Union(147, typeof(ServerModel.PlayerClimbArgs))]
    [Union(148, typeof(ServerModel.VehicleUpgradeConsoleArgs))]
    [Union(149, typeof(ServerModel.VehicleEnterArgs))]
    [Union(150, typeof(ServerModel.VehicleExitArgs))]
    [Union(151, typeof(ServerModel.VehicleUpdatedArgs))]
    [Union(152, typeof(ServerModel.HoverpadChargeTransmissionArgs))]
    [Union(153, typeof(ServerModel.PlayerDeadArgs))]
    [Union(154, typeof(ServerModel.VehicleHealthArgs))]
    [Union(155, typeof(ServerModel.VehicleEnergyTransmissionArgs))]
    [Union(156, typeof(ServerModel.VehicleBatteryArgs))]
    [Union(157, typeof(ServerModel.ExosuitStorageArgs))]
    [Union(158, typeof(ServerModel.VehicleLightArgs))]
    [Union(159, typeof(ServerModel.VehicleInteriorArgs))]
    [Union(160, typeof(ServerModel.EnergyMixinTransmissionArgs))]
    [Union(161, typeof(ServerModel.SeaTruckConnectionArgs))]
    [Union(162, typeof(ServerModel.SeaTruckStorageModuleArgs))]
    [Union(163, typeof(ServerModel.SeaTruckAquariumModuleArgs))]
    [Union(164, typeof(ServerModel.SeaTruckFabricatorModuleArgs))]
    [Union(165, typeof(ServerModel.ExosuitJumpArgs))]
    [Union(166, typeof(ServerModel.WorldCreaturePositionArgs))]
    [Union(167, typeof(ServerModel.EntitySlotProcessArgs))]
    [Union(168, typeof(ServerModel.WorldCreatureOwnershipChangedArgs))]
    [Union(169, typeof(ServerModel.CreatureHealthArgs))]
    [Union(170, typeof(ServerModel.StoryBridgeArgs))]
    [Union(171, typeof(ServerModel.StorySignalArgs))]
    [Union(172, typeof(ServerModel.StoryTriggerArgs))]
    [Union(173, typeof(ServerModel.StoryRadioTowerArgs))]
    [Union(174, typeof(ServerModel.StoryCinematicTriggerArgs))]
    [Union(175, typeof(ServerModel.StoryCallArgs))]
    [Union(176, typeof(ServerModel.StoryInteractArgs))]
    [Union(177, typeof(ServerModel.StoryPlayerVisibilityArgs))]
    [Union(178, typeof(ServerModel.StoryFrozenCreatureArgs))]
    [Union(179, typeof(ServerModel.StoryShieldBaseArgs))]
    [Union(180, typeof(ServerModel.StoryEndGameArgs))]
    [Union(181, typeof(ServerModel.StorageOpenArgs))]
    [Union(182, typeof(ServerModel.SpawnOnKillArgs))]
    [Union(183, typeof(ServerModel.PrecursorTeleporterArgs))]
    [Union(184, typeof(ServerModel.WelderArgs))]
    [Union(185, typeof(ServerModel.BaseCellWaterLevelArgs))]
    [Union(186, typeof(ServerModel.BrinicleArgs))]
    [Union(187, typeof(ServerModel.ConstructionHealthArgs))]
    [Union(188, typeof(ServerModel.PlayerToolEnergyArgs))]
    [Union(189, typeof(ServerModel.SeaTruckSleeperModuleArgs))]
    [Union(190, typeof(ServerModel.BaseMapRoomTransmissionArgs))]
    [Union(191, typeof(ServerModel.CosmeticItemArgs))]
    [Union(192, typeof(ServerModel.ResourceDiscoverArgs))]
    [Union(193, typeof(ServerModel.PlayerConsoleCommandArgs))]
    [Union(194, typeof(ServerModel.SeaTruckDockingModuleArgs))]
    [Union(195, typeof(ServerModel.PlayerRespawnPointArgs))]
    [Union(196, typeof(ServerModel.VehicleRepairArgs))]
    [Union(197, typeof(ServerModel.PlayerInitialEquipmentArgs))]
    [Union(198, typeof(ServerModel.PingArgs))]
    [Union(199, typeof(ServerModel.CreatureProcessArgs))]
    [Union(200, typeof(ServerModel.CreatureAnimationArgs))]
    [Union(201, typeof(ServerModel.PlayerFreezeArgs))]
    [Union(202, typeof(ServerModel.CreatureFreezeArgs))]
    [Union(203, typeof(ServerModel.CreatureCallArgs))]
    [Union(204, typeof(ServerModel.CreatureAttackLastTargetArgs))]
    [Union(205, typeof(ServerModel.CreatureLeviathanMeleeAttackArgs))]
    [Union(206, typeof(ServerModel.PlayerSpawnArgs))]
    [Union(207, typeof(ServerModel.CreatureMeleeAttackArgs))]
    [MessagePackObject]
    public abstract class NetworkPacket
    {
        /**
         *
         * Packet Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(0)]
        public abstract ProcessType Type { get; set; }

        /**
         *
         * Packet Kanal Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(1)]
        public virtual NetworkChannel ChannelType { get; set; } = NetworkChannel.Default;

        /**
         *
         * Packet Teslim Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(2)]
        public virtual DeliveryMethod DeliveryMethod { get; set; } = DeliveryMethod.ReliableOrdered;

        /**
         *
         * Packet Kanal Id
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(3)]
        public virtual byte ChannelId { get; set; } = 0;

        /**
         *
         * Paketin Sahibi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        [Key(4)]
        public byte PacketOwnerId { get; set; } = 0;

        /**
         *
         * Ağdaki paketi döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetPacketOwnerId(byte ownerId)
        {
            this.PacketOwnerId = ownerId;
        }

        /**
         *
         * Ağdaki paketi döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public byte GetPacketOwnerId()
        {
            return this.PacketOwnerId;
        }

        /**
         *
         * Ağdaki paketi döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public T GetPacket<T>()
        {
            return (T) Convert.ChangeType(this, this.GetType());
        }
    }
}