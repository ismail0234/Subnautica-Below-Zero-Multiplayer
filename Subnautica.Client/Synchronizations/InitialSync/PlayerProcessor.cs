namespace Subnautica.Client.Synchronizations.InitialSync
{
    using System.Collections.Generic;
    using System.Linq;

    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Client.MonoBehaviours.World;
    using Subnautica.Network.Models.Storage.Player;

    using UnityEngine;

    using ServerModel = Subnautica.Network.Models.Server;

    public class PlayerProcessor
    {
        /**
         *
         * Oyuncu verileri yüklendikten sonra tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlayerInitialized()
        {
            if (Network.Session.Current.PlayerHealth < 2f)
            {
                Network.Session.Current.PlayerHealth = 2f;
            }

            if (Network.Session.Current.PlayerUsedTools?.Count > 0)
            {
                global::Player.main.usedTools.AddRange(Network.Session.Current.PlayerUsedTools);
            }

            global::Player.main.oxygenMgr.Restore();
            global::Player.main.timeLastSleep    = Network.Session.Current.PlayerTimeLastSleep;
            global::Player.main.liveMixin.health = Network.Session.Current.PlayerHealth;
            global::Player.main.GetComponent<Survival>().food  = Network.Session.Current.PlayerFood;
            global::Player.main.GetComponent<Survival>().water = Network.Session.Current.PlayerWater;

            if (Network.Session.Current.InteractList != null)
            {
                Interact.SetList(Network.Session.Current.InteractList);
            }

            Inventory.main.SecureItems(true);

            ZeroPlayer.CurrentPlayer.OnCurrentPlayerLoaded();
        }

        /**
         *
         * İlk ekipmanları duruma göre başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlayerFirstEquipmentInitialized()
        {
            if (GameModeManager.GetOption<bool>(GameOption.InitialEquipmentPack) && !Network.Session.Current.IsInitialEquipmentAdded)
            {
                NetworkClient.SendPacket(new ServerModel.PlayerInitialEquipmentArgs());
            }
        }

        /**
         *
         * Oyuncu başlangıç konumunu ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlayerPositionInitialized()
        {
            if (Network.Session.Current.PlayerPosition != null)
            {
                global::Player.main.transform.position = Network.Session.Current.PlayerPosition.ToVector3();
                global::Player.main.lastPosition       = Network.Session.Current.PlayerPosition.ToVector3();
                global::Player.main.transform.rotation = Network.Session.Current.PlayerRotation.ToQuaternion();
            }

            if (Vector3.zero == global::Player.main.transform.position)
            {
                var gameData = Player.main.GetGameData(SaveLoadManager.main.storyVersion);
                if (Network.Session.Current.GameMode == GameModePresetId.Creative)
                {
                    Player.main.SetPosition(gameData.creativeStartLocation.position, Quaternion.Euler(gameData.creativeStartLocation.rotation));
                }
                else
                {
                    Player.main.SetPosition(gameData.storyStartLocation.position, Quaternion.Euler(gameData.storyStartLocation.rotation));
                }
            }
        }

        /**
         *
         * Oyuncu başlangıç konumunu ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlayerSubRootInitialized()
        {
            if (Network.Session.Current.PlayerSubRootId.IsNotNull())
            {
                using (EventBlocker.Create(ProcessType.InteriorToggle))
                using (EventBlocker.Create(ProcessType.SubrootToggle))
                {
                    var subroot = Network.Identifier.GetComponentByGameObject<SubRoot>(Network.Session.Current.PlayerSubRootId, true);
                    if (subroot != null)
                    {
                        global::Player.main.SetCurrentSub(subroot);
                        global::Player.main.playerController.SetEnabled(true);
                    }
                }
            }
            else if (Network.Session.Current.PlayerInteriorId.IsNotNull())
            {
                global::SeaTruckSegment newSeaTruckSegment = null;

                using (EventBlocker.Create(ProcessType.InteriorToggle))
                using (EventBlocker.Create(ProcessType.SubrootToggle))
                using (EventBlocker.Create(ProcessType.VehicleEnter))
                {
                    var interior = Network.Identifier.GetComponentByGameObject<global::IInteriorSpace>(Network.Session.Current.PlayerInteriorId);
                    if (interior != null)
                    {
                        var respawnPoint = interior.GetRespawnPoint();
                        if (respawnPoint)
                        {
                            global::Player.main.SetPosition(respawnPoint.GetSpawnPosition());

                            if (interior.GetGameObject().TryGetComponent<global::SeaTruckSegment>(out var seaTruckSegment))
                            {
                                if (seaTruckSegment == seaTruckSegment.GetFirstSegment())
                                {
                                    seaTruckSegment.Enter(global::Player.main);
                                }
                                else
                                {
                                    newSeaTruckSegment = seaTruckSegment.GetFirstSegment();
                                }
                            }
                            else
                            {
                                global::Player.main.EnterInterior(interior);
                            }
                        }
                        else
                        {
                            global::Player.main.EnterInterior(interior);
                        }
                    }
                }

                if (newSeaTruckSegment)
                {
                    newSeaTruckSegment.GetFirstSegment().Enter(global::Player.main);
                }
            }

            PlayerProcessor.FixMoonpoolExpansionPlayerPosition(global::Player.main, Network.Session.Current.PlayerSubRootId);
        }

        /**
         *
         * Oyuncu yeniden doğma konumunu ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPlayerRespawnPointInitialized()
        {
            if (Network.Session.Current.SupplyDrops?.Count > 0)
            {
                foreach (var supplyDrop in Network.Session.Current.SupplyDrops.Where(q => q.ZoneId != -1))
                {
                    global::Player.main.fallbackRespawnInteriorUID = supplyDrop.UniqueId;
                }
            }

            if (Network.Session.Current.PlayerRespawnPointId.IsNotNull() && Network.Identifier.GetGameObject(Network.Session.Current.PlayerRespawnPointId, true) != null)
            {
                global::Player.main.currentRespawnInteriorUID = Network.Session.Current.PlayerRespawnPointId;
            }
        }

        /**
         *
         * Diğer oyuncuları oluşturur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnOtherPlayersInitialized(List<PlayerItem> players)
        {
            if (players?.Count > 0)
            {
                foreach (var packet in players)
                {
                    var player = ZeroPlayer.CreateOrGetPlayerByUniqueId(packet.UniqueId, packet.PlayerId);
                    player.SetPlayerName(packet.PlayerName);
                    player.SetSubRootId(packet.SubrootId);
                    player.SetInteriorId(packet.InteriorId);

                    if (!player.IsCreatedModel)
                    {
                        player.CreateModel(packet.Position.ToVector3(true), packet.Rotation.ToQuaternion(true));
                        player.InitBehaviours();
                    }
                }
            }
        }

        /**
         *
         * Ay havuzundaki oyuncunun konumunu düzeltir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool FixMoonpoolExpansionPlayerPosition(global::Player player, string subrootId)
        {
            var firstSegment = player.GetUnderGameObject<global::SeaTruckSegment>()?.GetFirstSegment();
            if (firstSegment)
            {
                var moonpoolExpansionManager = firstSegment.GetDockedMoonpoolExpansion(true);
                if (moonpoolExpansionManager)
                {
                    var dockingBay = moonpoolExpansionManager.bay.gameObject.EnsureComponent<MultiplayerVehicleDockingBay>();
                    if (dockingBay && dockingBay.ExpansionManager.IsPlayerInMoonpoolExpansion())
                    {
                        firstSegment.Exit(skipAnimations: true, newInterior: moonpoolExpansionManager.interior);

                        player.SetPosition(dockingBay.ExpansionManager.GetTerminalSpawnPosition());
                        return true;
                    }
                }
            }

            player.UpdateIsUnderwater();

            if (subrootId.IsNotNull())
            {
                var subroot = Network.Identifier.GetComponentByGameObject<SubRoot>(subrootId, true);
                if (subroot)
                {
                    foreach (var vehicleDockingBay in subroot.GetComponentsInChildren<global::VehicleDockingBay>())
                    {
                        var dockingBay = vehicleDockingBay.gameObject.EnsureComponent<MultiplayerVehicleDockingBay>();
                        if (dockingBay && dockingBay.ExpansionManager.IsActive() && dockingBay.ExpansionManager.IsPlayerInMoonpoolExpansion())
                        {
                            player.SetPosition(dockingBay.ExpansionManager.GetTerminalSpawnPosition());
                            return true;
                        }
                    }
                }
            }
            else if (player.IsUnderwater())
            {
                foreach (var vehicleDockingBay in GameObject.FindObjectsOfType<global::VehicleDockingBay>())
                {
                    var dockingBay = vehicleDockingBay.gameObject.EnsureComponent<MultiplayerVehicleDockingBay>();
                    if (dockingBay && dockingBay.ExpansionManager.IsActive() && dockingBay.ExpansionManager.IsPlayerInMoonpoolExpansion())
                    {
                        player.SetPosition(dockingBay.ExpansionManager.GetOutsideSpawnPosition());
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
