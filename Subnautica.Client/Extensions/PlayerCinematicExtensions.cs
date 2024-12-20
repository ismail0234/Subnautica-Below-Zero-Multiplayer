namespace Subnautica.Client.Extensions
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;
    using Subnautica.Client.Modules;
    using Subnautica.Client.Multiplayer.Cinematics;

    public static class PlayerCinematicExtensions
    {
        /**
         *
         * Oyuncuya ait tüm cinematikleri sıfırlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ResetCinematics(this ZeroPlayer player)
        {
            foreach (var cinematic in player.GetCinematics())
            {
                cinematic.EndCinematicMode();
            }
        }

        /**
         *
         * UniqueId'ye sahip tüm yapıların sinematiğini sıfırlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ResetCinematicsByUniqueId(this ZeroPlayer player, string uniqueId)
        {
            foreach (var zeroPlayer in ZeroPlayer.GetPlayers())
            {
                foreach (var cinematic in zeroPlayer.GetCinematics())
                {
                    if (cinematic.UniqueId == uniqueId)
                    {
                        cinematic.EndCinematicMode();
                    }
                }
            }
        }

        /**
         *
         * Moonpool üzerinden araç alma işlemini yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool OnHandClickMoonpoolUndocking(this ZeroPlayer player, string moonpoolId, bool isLeft)
        {
            player.ResetCinematicsByUniqueId(moonpoolId);

            var dockingBay = Network.Identifier.GetComponentByGameObject<global::VehicleDockingBay>(moonpoolId);
            if (dockingBay == null)
            {
                return false;
            }

            DockedVehicleHandTarget handTarget = null;

            foreach (var item in dockingBay.transform.parent.GetComponentsInChildren<DockedVehicleHandTarget>())
            {
                if ((isLeft && item.name.Contains("Left")) || (!isLeft && item.name.Contains("Right")))
                {
                    handTarget = item;
                    break;
                }
            }

            if (handTarget)
            {
                using (EventBlocker.Create(TechType.BaseMoonpool))
                {
                    if (CraftData.GetTechType(handTarget.dockingBay.GetDockedObject().gameObject) == TechType.Exosuit)
                    {
                        handTarget.dockingBay.exosuitDockPlayerCinematic.SkipCinematic();
                    }
                    else
                    {
                        handTarget.dockingBay.dockPlayerCinematic.SkipCinematic();
                    }

                    handTarget.isValidHandTarget = true;
                    handTarget.OnHandClick(global::Player.main.guiHand);
                }
            }

            return true;
        }

        /**
         *
         * Hoverpad üzerinden araç alma işlemini yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool OnHandClicHoverpadUndock(this ZeroPlayer player, string uniqueId)
        {
            player.ResetCinematicsByUniqueId(uniqueId);

            var trigger = Network.Identifier.GetComponentByGameObject<global::HoverpadUndockTrigger>(uniqueId);
            if (trigger == null)
            {
                return false;
            }

            using (EventBlocker.Create(TechType.Hoverbike))
            {
                trigger.OnHandClick(global::Player.main.guiHand);
            }

            return true;
        }

        /**
         *
         * Oyuncunun bölme kapısı açma/kapatma sinematiğini başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool OnHandClickBulkhead(this ZeroPlayer player, string uniqueId, bool isOpen, bool side)
        {
            player.ResetCinematicsByUniqueId(uniqueId);

            var door = Network.Identifier.GetComponentByGameObject<global::BulkheadDoor>(uniqueId);
            if (door == null)
            {
                return false;
            }

            door.quickSlot = Inventory.main.quickSlots.activeSlot;

            Inventory.main.ReturnHeld();

            door.animator.SetBool(global::BulkheadDoor.animPlayerInFront, side);

            if (!side)
            {
                if (!door.shouldPassThrough || !door.backPassthroughCinematicController)
                {
                    if (isOpen)
                    {
                        door.backOpenCinematicController.StartCinematicMode(global::Player.main);
                    }
                    else
                    {
                        door.backCloseCinematicController.StartCinematicMode(global::Player.main);
                    }
                }
                else
                {
                    door.backPassthroughCinematicController.StartCinematicMode(global::Player.main);
                }
            }
            else
            {
                if (!door.shouldPassThrough || !door.backPassthroughCinematicController)
                {
                    if (isOpen)
                    {
                        door.frontOpenCinematicController.StartCinematicMode(global::Player.main);
                    }
                    else
                    {
                        door.frontCloseCinematicController.StartCinematicMode(global::Player.main);
                    }
                }
                else
                {
                    door.frontPassthroughCinematicController.StartCinematicMode(global::Player.main);
                }
            }

            return true;
        }
        /**
         *
         * Oyuncunun yatak uyuma sinematiğini başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool OnHandClickBed(this ZeroPlayer player, string uniqueId, global::Bed.BedSide side)
        {
            player.ResetCinematicsByUniqueId(uniqueId);

            var bed = Network.Identifier.GetComponentByGameObject<global::Bed>(uniqueId);
            if (bed == null)
            {
                return false;
            }

            Multiplayer.Furnitures.Bed.UpdateBed(player.UniqueId);

            SleepScreen.Instance.Initialize();
            SleepScreen.Instance.StartSleeping();
            SleepScreen.Instance.SetUniqueId(uniqueId);
            SleepScreen.Instance.SetBedSide(side);

            switch (side)
            {
                case global::Bed.BedSide.Right:
                    bed.cinematicController = bed.rightLieDownCinematicController;
                    bed.currentStandUpCinematicController = bed.rightStandUpCinematicController;
                    bed.animator.transform.localPosition  = bed.rightAnimPosition;
                    break;
                default:
                    bed.cinematicController = bed.leftLieDownCinematicController;
                    bed.currentStandUpCinematicController = bed.leftStandUpCinematicController;
                    bed.animator.transform.localPosition  = bed.leftAnimPosition;
                    break;
            }

            bed.cinematicController.animator.Rebind();
            bed.ResetAnimParams(global::Player.main.playerAnimator);
            bed.StartCinematicMode(global::Player.main);
            return true;
        } 

        /**
         *
         * Oyuncunun local koltuk sinematiğini başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool OnHandClickBench(this ZeroPlayer player, string uniqueId, global::Bench.BenchSide side)
        {
            player.ResetCinematicsByUniqueId(uniqueId);

            var bench = Network.Identifier.GetComponentByGameObject<global::Bench>(uniqueId);
            if (bench == null)
            {
                return false;
            }

            bench.animator.Rebind();
            bench.animator.transform.localEulerAngles = side == global::Bench.BenchSide.Front ? bench.frontAnimRotation : bench.backAnimRotation;
            bench.ResetAnimParams(global::Player.main.playerAnimator);
            bench.StartCinematicMode(global::Player.main);
            return true;
        }

        /**
         *
         * Oyuncunun bölme kapısı açma/kapatma sinematiğini başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool OnHandClickConstructor(this ZeroPlayer player, string uniqueId)
        {
            player.ResetCinematicsByUniqueId(uniqueId);

            var constructor = Network.Identifier.GetComponentByGameObject<global::ConstructorCinematicController>(uniqueId);
            if (constructor == null)
            {
                return false;
            }

            constructor.quickSlot = Inventory.main.quickSlots.activeSlot;

            Inventory.main.ReturnHeld();

            constructor.ResetAnimParams(global::Player.main.playerAnimator);
            constructor.engageCinematicController.StartCinematicMode(global::Player.main);
            return true;
        }

        /**
         *
         * Radyo kulesi tom ekleme sinematiğini başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool OnHandClickRadioTowerInsertedItem(this ZeroPlayer player, string uniqueId)
        {
            player.ResetCinematicsByUniqueId(uniqueId);
            
            var radioTower = Network.Identifier.GetComponentByGameObject<global::RadioTowerController>(uniqueId);
            if (radioTower == null)
            {
                return false;
            }

            World.DestroyItemFromPlayer(TechType.RadioTowerTOM);

            radioTower.itemInsertedStoryGoal.Trigger();
            radioTower.StartCoroutine(radioTower.PlayInsertCinematic());
            return true;
        }

        /**
         *
         * Oyuncunun bölme kapısı açma/kapatma sinematiğini başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool OnHandClickUseableDiveHatch(this ZeroPlayer player, string uniqueId, bool isEnter)
        {
            player.ResetCinematicsByUniqueId(uniqueId);

            var gameObject = Network.Identifier.GetGameObject(uniqueId);
            if (gameObject == null)
            {
                return false;
            }

            var hatch = gameObject.GetComponentInChildren<global::UseableDiveHatch>();

            if (gameObject.name.Contains("BaseMoonpoolExpansion"))
            {
                foreach (var item in gameObject.GetComponentsInChildren<global::UseableDiveHatch>())
                {
                    if (item.name.Contains("SealDoor"))
                    {
                        continue;
                    }

                    hatch = item;
                }
            }

            hatch.quickSlot = Inventory.main.quickSlots.activeSlot;

            Inventory.main.ReturnHeld();

            if (isEnter)
            {

                hatch.enterCinematicController.informGameObject = hatch.gameObject;
                hatch.enterCinematicController.StartCinematicMode(global::Player.main);
            }
            else
            {
                hatch.exitCinematicController.informGameObject = hatch.gameObject;
                hatch.exitCinematicController.StartCinematicMode(global::Player.main);
            }

            if (hatch.secureInventory)
            {
                Inventory.Get().SecureItems(true);
            }

            return true;
        }

        /**
         *
         * Oyuncu merdivene tırmanma sinematiğini başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool OnHandClickClimb(this ZeroPlayer player, string uniqueId)
        {
            player.ResetCinematicsByUniqueId(uniqueId);

            var climb = Network.Identifier.GetComponentByGameObject<global::CinematicModeTrigger>(uniqueId);
            if (climb == null)
            {
                return false;
            }

            global::Inventory.main.ReturnHeld();
            
            climb.StartCinematicMode(global::Player.main);
            return true;
        }

        /**
         *
         * Işınlanma kapısının terminal animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool OnHandClickPrecursorTerminal(this ZeroPlayer player, string uniqueId)
        {
            player.ResetCinematicsByUniqueId(uniqueId);

            var terminal = Network.Identifier.GetComponentByGameObject<global::PrecursorTeleporterActivationTerminal>(uniqueId);
            if (terminal == null)
            {
                return false;
            }

            using (EventBlocker.Create(API.Enums.ProcessType.PrecursorTeleporter))
            {
                terminal.OnProxyHandClick(null);
            }

            return true;
        }

        /**
         *
         * SeaTruck/Exosuit demirleme animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void StartMooonpoolUndockingCinematic(this ZeroPlayer player, string uniqueId, bool isLeft)
        {
            var cinematic = player.GetCinematic<MoonpoolCinematic>();
            if (cinematic)
            {
                PlayerCinematicQueue.AddQueue(cinematic, cinematic.StartUnDockingCinematic, player.UniqueId, uniqueId, new GenericProperty("IsLeft", isLeft));
            }
        }

        /**
         *
         * SeaTruck/Exosuit demirleme animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void StartMooonpoolDockingCinematic(this ZeroPlayer player, string uniqueId, TechType techType)
        {
            var cinematic = player.GetCinematic<MoonpoolCinematic>();
            if (cinematic)
            {
                PlayerCinematicQueue.AddQueue(cinematic, cinematic.StartDockingCinematic, player.UniqueId, uniqueId, new GenericProperty("TechType", techType));
            }
        }

        /**
         *
         * Işınlanma kapısının terminal animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ActivatePrecursorTerminal(this ZeroPlayer player, string uniqueId)
        {
            var cinematic = player.GetCinematic<PrecursorTeleporterTerminalCinematic>();
            if (cinematic)
            {
                PlayerCinematicQueue.AddQueue(cinematic, cinematic.ActivatePrecursorTerminalCinematic, player.UniqueId, uniqueId);
            }
        }

        /**
         *
         * Bölme Kapısı -> Açılma animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OpenStartCinematicBulkhead(this ZeroPlayer player, string uniqueId, bool side)
        {
            var cinematic = player.GetCinematic<BulkheadDoorCinematic>();
            if (cinematic)
            {
                PlayerCinematicQueue.AddQueue(cinematic, cinematic.OpenDoorStartCinematic, player.UniqueId, uniqueId, new GenericProperty("Side", side));
            }
        }

        /**
         *
         * Bölme Kapısı -> Kapatma animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void CloseStartCinematicBulkhead(this ZeroPlayer player, string uniqueId, bool side)
        {
            var cinematic = player.GetCinematic<BulkheadDoorCinematic>();
            if (cinematic)
            {
                PlayerCinematicQueue.AddQueue(cinematic, cinematic.CloseDoorStartCinematic, player.UniqueId, uniqueId, new GenericProperty("Side", side));
            }
        }

        /**
         *
         * Yatak -> Uyuma animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void LieDownStartCinematicBed(this ZeroPlayer player, string uniqueId, global::Bed.BedSide side)
        {
            var cinematic = player.GetCinematic<BedCinematic>();
            if (cinematic)
            {
                PlayerCinematicQueue.AddQueue(cinematic, cinematic.LieDownStartCinematic, player.UniqueId, uniqueId, new GenericProperty("Side", side));
            }
        }

        /**
         *
         * Yatak -> Kalkma animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void StandupStartCinematicBed(this ZeroPlayer player, string uniqueId, global::Bed.BedSide side)
        {
            var cinematic = player.GetCinematic<BedCinematic>();
            if (cinematic)
            {
                PlayerCinematicQueue.AddQueue(cinematic, cinematic.StandupStartCinematic, player.UniqueId, uniqueId, new GenericProperty("Side", side));
            }
        }

        /**
         *
         * Sandalye -> Oturma animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SitDownStartCinematicBench(this ZeroPlayer player, string uniqueId, global::Bench.BenchSide side)
        {
            var cinematic = player.GetCinematic<BenchCinematic>();
            if (cinematic)
            {
                PlayerCinematicQueue.AddQueue(cinematic, cinematic.SitDownStartCinematic, player.UniqueId, uniqueId, new GenericProperty("Side", side));
            }
        }

        /**
         *
         * Sandalye -> Kalkma animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void StandupStartCinematicBench(this ZeroPlayer player, string uniqueId, global::Bench.BenchSide side)
        {
            var cinematic = player.GetCinematic<BenchCinematic>();
            if (cinematic)
            {
                PlayerCinematicQueue.AddQueue(cinematic, cinematic.StandupStartCinematic, player.UniqueId, uniqueId, new GenericProperty("Side", side));
            }
        }

        /**
         *
         * Bölme Kapısı -> Kapatma animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void EngageStartCinematicConstructor(this ZeroPlayer player, string uniqueId)
        {
            var cinematic = player.GetCinematic<ConstructorCinematic>();
            if (cinematic)
            {
                PlayerCinematicQueue.AddQueue(cinematic, cinematic.EngageStartCinematic, player.UniqueId, uniqueId);
            }
        }
        
        /**
         *
         * Bölme Kapısı -> Kapatma animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void DisengageStartCinematicConstructor(this ZeroPlayer player, string uniqueId)
        {
            var cinematic = player.GetCinematic<ConstructorCinematic>();
            if (cinematic)
            {
                PlayerCinematicQueue.AddQueue(cinematic, cinematic.DisengageStartCinematic, player.UniqueId, uniqueId);
            }
        }
        
        /**
         *
         * SeaTruck modül bağlantı kesme animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void RadioTowerInsertedItemStartCinematic(this ZeroPlayer player, string uniqueId)
        {
            var cinematic = player.GetCinematic<RadioTowerCinematic>();
            if (cinematic)
            {
                PlayerCinematicQueue.AddQueue(cinematic, cinematic.InsertedItemStartCinematic, player.UniqueId, uniqueId);
            }
        }
        
        /**
         *
         * Bölme Kapısı -> Kapatma animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ClimbStartCinematic(this ZeroPlayer player, string uniqueId)
        {
            var cinematic = player.GetCinematic<ClimbCinematic>();
            if (cinematic)
            {
                PlayerCinematicQueue.AddQueue(cinematic, cinematic.ClimbStartCinematic, player.UniqueId, uniqueId);
            }
        }

        /**
         *
         * LifePod / Su altı veya üstü yapı kapısı v.s -> Açılma animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void EnterStartCinematicUseableDiveHatch(this ZeroPlayer player, string uniqueId)
        {
            var cinematic = player.GetCinematic<UseableDiveHatchCinematic>();
            if (cinematic)
            {
                PlayerCinematicQueue.AddQueue(cinematic, cinematic.EnterStartCinematic, player.UniqueId, uniqueId);
            }
        }

        /**
         *
         * LifePod / Su altı veya yapı kapısı v.s -> Kapatma animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ExitStartCinematicUseableDiveHatch(this ZeroPlayer player, string uniqueId)
        {
            var cinematic = player.GetCinematic<UseableDiveHatchCinematic>();
            if (cinematic)
            {
                PlayerCinematicQueue.AddQueue(cinematic, cinematic.ExitStartCinematic, player.UniqueId, uniqueId);
            }
        }
        
        /**
         *
         * SeaTruck pilotluk animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SeaTruckStartPilotingCinematic(this ZeroPlayer player, string uniqueId)
        {
            var cinematic = player.GetCinematic<SeaTruckPilotingCinematic>();
            if (cinematic)
            {
                PlayerCinematicQueue.AddQueue(cinematic, cinematic.StartPilotingCinematic, player.UniqueId, uniqueId);
            }
        }
        
        /**
         *
         * SeaTruck pilotluk animasyonunu sonlandırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SeaTruckStopPilotingCinematic(this ZeroPlayer player, string uniqueId)
        {
            var cinematic = player.GetCinematic<SeaTruckPilotingCinematic>();
            if (cinematic)
            {
                PlayerCinematicQueue.AddQueue(cinematic, cinematic.StopPilotingCinematic, player.UniqueId, uniqueId);
            }
        }
        
        /**
         *
         * SeaTruck ışınlanma animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SeaTruckTeleportationStartCinematic(this ZeroPlayer player, string uniqueId)
        {
            var cinematic = player.GetCinematic<SeaTruckTeleportationCinematic>();
            if (cinematic)
            {
                PlayerCinematicQueue.AddQueue(cinematic, cinematic.SeaTruckTeleportationStartCinematic, player.UniqueId, uniqueId);
            }
        }
        
        /**
         *
         * SeaTruck modül bağlantı kesme animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SeaTruckStartDetachCinematic(this ZeroPlayer player, string uniqueId)
        {
            var cinematic = player.GetCinematic<SeaTruckDetachCinematic>();
            if (cinematic)
            {
                PlayerCinematicQueue.AddQueue(cinematic, cinematic.DetachSeaTruckCinematic, player.UniqueId, uniqueId);
            }
        }

        /**
         *
         * Hovarpad'e bağlı, hoverbike oturma animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void UndockStartCinematicHoverpad(this ZeroPlayer player, string uniqueId)
        {
            var cinematic = player.GetCinematic<HoverpadCinematic>();
            if (cinematic)
            {
                PlayerCinematicQueue.AddQueue(cinematic, cinematic.UndockStartCinematic, player.UniqueId, uniqueId);
            }
        }

        /**
         *
         * Hovarpad'e bağlı, hoverbike inme animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void DockStartCinematicHoverpad(this ZeroPlayer player, string uniqueId)
        {
            var cinematic = player.GetCinematic<HoverpadCinematic>();
            if (cinematic)
            {
                PlayerCinematicQueue.AddQueue(cinematic, cinematic.DockStartCinematic, player.UniqueId, uniqueId);
            }
        }
    }
}