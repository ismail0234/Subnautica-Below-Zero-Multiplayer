namespace Subnautica.API.Extensions
{
    using Subnautica.API.Features;

    using UnityEngine;

    public static class TechExtensions
    {
        /**
         *
         * Çok oyunculu oyuncu mu?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsMultiplayerPlayer(this string name)
        {
            return name.Contains(ZeroPlayer.PlayerSignalName);
        }

        /**
         *
         * Teknoloji türünü döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static TechType GetTechType(this GameObject gameObject)
        {
            return CraftData.GetTechType(gameObject);
        }

        /**
         *
         * Teknoloji türünü değiştirir
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SetTechType(this GameObject gameObject, TechType techType)
        {
            gameObject.EnsureComponent<global::TechTag>().type = techType;
        }

        /**
         *
         * ClassId değerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetClassId(this TechType techType)
        {
            if (Network.EntityDatabase.TryGetInfoByTechType(techType, out var info))
            {
                return info.classId;
            }

            return null;
        }

        /**
         *
         * Nesne alınma bildirimi gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ShowPickupNotify(this TechType techType)
        {
            uGUI_IconNotifier.main.Play(techType, uGUI_IconNotifier.AnimationType.From);
        }

        /**
         *
         * Nesne alınma bildirimi gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ShowPickupNotify(this global::Pickupable pickupable)
        {
            pickupable.GetTechType().ShowPickupNotify();
        }

        /**
         *
         * Teknoloji türünün tarandıktan sonra yok olup olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsDestroyAfterScan(this TechType techType)
        {
            var entry = global::PDAScanner.GetEntryData(techType);
            if (entry == null)
            {
                return false;
            }

            return entry.destroyAfterScan;
        }
        
        /**
         *
         * Teknoloji türünün parça olup olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsFragment(this TechType techType)
        {
            return techType == TechType.Fragment || global::PDAScanner.IsFragment(techType);
        }

        /**
         *
         * Oyuncu olup olmadığını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsPlayer(this TechType techType)
        {
            return techType == TechType.Player;
        }

        /**
         *
         * Araç olup olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsVehicle(this TechType techType, bool isCheckVehicle = true, bool isModule = true)
        {
            if (isCheckVehicle)
            {
                switch (techType)
                {
                    case TechType.Hoverbike:
                    case TechType.Exosuit:
                    case TechType.SeaTruck:
                    case TechType.SpyPenguin:
                    case TechType.MapRoomCamera:
                        return true;
                }
            }

            if (isModule && techType.IsSeaTruckModule())
            {
                return true;
            }

            return false;
        }

        /**
         *
         * Teknoloji türünün seatruck modülü olup olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsSeaTruckModule(this TechType techType, bool checkSeaTruck = false)
        {
            if (checkSeaTruck && techType == TechType.SeaTruck)
            {
                return true;
            }

            switch (techType)
            {
                case TechType.SeaTruckFabricatorModule:
                case TechType.SeaTruckStorageModule:
                case TechType.SeaTruckAquariumModule:
                case TechType.SeaTruckDockingModule:
                case TechType.SeaTruckSleeperModule:
                case TechType.SeaTruckTeleportationModule:
                    return true;
            }

            return false;
        }
                
        /**
         *
         * Teknoloji türün adını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetTechName(this TechType techType)
        {
            return techType.AsString();
        }

        /**
         *
         * Teknoloji türünün poster olup olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsPoster(this TechType techType)
        {
            switch (techType)
            {
                case TechType.PosterAurora:
                case TechType.PosterExoSuit1:
                case TechType.PosterExoSuit2:
                case TechType.PosterKitty:
                case TechType.PosterSpyPenguin:
                case TechType.PosterMotivational:
                case TechType.PosterSeatruck:
                case TechType.PosterLilArchitect:
                case TechType.PosterJeremiahNoBirds:
                case TechType.PosterSeatruck2:
                case TechType.PosterMercury:
                case TechType.PosterMotivational2:
                case TechType.PosterMotivational3:
                case TechType.Poster:
                case TechType.PosterAlterraBounty:
                case TechType.PosterParvan:
                case TechType.PosterBunkerCommunity:
                case TechType.PosterZetaRollerDerby:
                case TechType.PosterSpyPenguinConcepts:
                case TechType.PosterBoardgame:
                case TechType.PosterHangInThere:
                case TechType.PosterSpyPenguinBlueprint:
                case TechType.PosterParvanBiome:
                    return true;
            }

            return false;
        }

        /**
         *
         * Teknoloji türünün poster olup olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsPictureFrame(this TechType techType)
        {
            switch (techType)
            {
                case TechType.PictureFredPengling:
                case TechType.PictureSamPotato:
                case TechType.PicturePotatoPortrait:
                case TechType.PictureSamDanielleHappy:
                case TechType.PictureVinhBiologyArt:
                case TechType.PictureDanielleAbstractArt:
                case TechType.PictureSamHand:
                    return true;
            }

            return false;
        }

        /**
         *
         * Yatak olup olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsBed(this TechType techType)
        {
            switch (techType)
            {
                case TechType.Bed1:
                case TechType.Bed2:
                case TechType.NarrowBed:
                case TechType.BedDanielle:
                case TechType.BedEmmanuel:
                case TechType.BedFred:
                case TechType.BedJeremiah:
                case TechType.BedSam:
                case TechType.BedZeta:
                case TechType.BedParvan:
                    return true;
            }

            return false;
        }

        /**
         *
         * Saksı olup olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsPlanterPot(this TechType techType)
        {
            switch (techType)
            {
                case TechType.PlanterPot:
                case TechType.PlanterPot2:
                case TechType.PlanterPot3:
                case TechType.PlanterBox:
                case TechType.PlanterShelf:
                    return true;
            }

            return false;
        }

        /**
         *
         * Üs parçası olup olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsBasePiece(this TechType techType)
        {
            switch (techType)
            {
                case TechType.BaseObservatory:
                case TechType.BaseWindow:
                case TechType.BaseCorridorI:
                case TechType.BaseCorridorL:
                case TechType.BaseCorridorT:
                case TechType.BaseCorridorX:
                case TechType.BaseCorridorGlassI:
                case TechType.BaseCorridorGlassL:
                case TechType.BaseLargeRoom:
                case TechType.BaseLargeGlassDome:
                case TechType.BaseRoom:
                case TechType.BaseGlassDome:
                case TechType.BaseReinforcement:
                case TechType.BaseHatch:
                case TechType.BaseFoundation:
                case TechType.BaseConnector:
                case TechType.BaseControlRoom:
                case TechType.BaseMoonpool:
                case TechType.BaseMoonpoolExpansion:
                case TechType.BaseMapRoom:
                    return true;
            }

            return false;
        }

        /**
         *
         * Mobilya olup olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsFurniture(this TechType techType)
        {
            if (techType.IsPlanterPot() || techType.IsBed())
            {
                return true;
            }

            switch (techType)
            {
                case TechType.BarTable:
                case TechType.ExecutiveDesk:
                case TechType.SingleWallShelf:
                case TechType.WallShelves:
                case TechType.StarshipDesk:
                case TechType.LabCounter:
                case TechType.VendingMachine:
                case TechType.Toilet:
                case TechType.AromatherapyLamp:
                case TechType.EmmanuelPendulum:
                case TechType.Shower:
                case TechType.Sink:
                case TechType.SmallStove:
                case TechType.Sign:
                case TechType.BaseLadder:
                case TechType.BasePlanter:
                case TechType.PictureFrame:
                case TechType.Jukebox:
                case TechType.Speaker:
                case TechType.Trashcans:
                case TechType.LabTrashcan:
                case TechType.Aquarium:
                case TechType.Workbench:
                case TechType.Fabricator:
                case TechType.StarshipChair:
                case TechType.StarshipChair2:
                case TechType.StarshipChair3:
                case TechType.Bench:
                case TechType.Techlight:
                case TechType.Spotlight:
                case TechType.Snowman:
                case TechType.SmallLocker:
                case TechType.Locker:
                case TechType.PowerTransmitter:
                case TechType.ThermalPlant:
                case TechType.SolarPanel:
                case TechType.BaseBioReactor:
                case TechType.BaseNuclearReactor:
                case TechType.BasePartition:
                case TechType.BasePartitionDoor:
                case TechType.BatteryCharger:
                case TechType.PowerCellCharger:
                case TechType.Recyclotron:
                case TechType.CoffeeVendingMachine:
                case TechType.Fridge:
                case TechType.BaseFiltrationMachine:
                case TechType.FarmingTray:
                case TechType.BaseBulkhead:
                case TechType.Hoverpad:
                case TechType.BaseUpgradeConsole:
                case TechType.BaseWaterPark:
                    return true;
            }

            return false;
        }

        /**
         *
         * Kırılabilen nesne olup olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsBreakable(this TechType techType)
        {
            switch (techType)
            {
                case TechType.BreakableSilver:
                case TechType.BreakableGold:
                case TechType.BreakableLead:
                    return true;
            }

            return false;
        }

        /**
         *
         * Kazılabilen nesne olup olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsDrillable(this TechType techType)
        {
            switch (techType)
            {
                case TechType.DrillableAluminiumOxide:
                case TechType.DrillableCopper:
                case TechType.DrillableDiamond:
                case TechType.DrillableGold:
                case TechType.DrillableKyanite:
                case TechType.DrillableLead:
                case TechType.DrillableLithium:
                case TechType.DrillableMagnetite:
                case TechType.DrillableMercury:
                case TechType.DrillableNickel:
                case TechType.DrillableQuartz:
                case TechType.DrillableSalt:
                case TechType.DrillableSilver:
                case TechType.DrillableSulphur:
                case TechType.DrillableTitanium:
                case TechType.DrillableUranium:
                case TechType.OreVein:
                    return true;
            }

            return false;
        }

        /**
         *
         * Yaratık yumurtası olup olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsCreatureEgg(this TechType techType)
        {
            switch (techType)
            {
                case TechType.LavaZoneEgg:
                case TechType.ShockerEgg:
                case TechType.ShockerEggUndiscovered:
                case TechType.GenericEgg:
                case TechType.CutefishEggUndiscovered:
                case TechType.SeaMonkeyEgg:
                case TechType.CrashEgg:
                case TechType.CrashEggUndiscovered:
                case TechType.ArcticRayEgg:
                case TechType.ArcticRayEggUndiscovered:
                case TechType.BruteSharkEgg:
                case TechType.BruteSharkEggUndiscovered:
                case TechType.LilyPaddlerEgg:
                case TechType.LilyPaddlerEggUndiscovered:
                case TechType.PinnacaridEgg:
                case TechType.PinnacaridEggUndiscovered:
                case TechType.SquidSharkEgg:
                case TechType.SquidSharkEggUndiscovered:
                case TechType.TitanHolefishEgg:
                case TechType.TitanHolefishEggUndiscovered:
                case TechType.TrivalveBlueEgg:
                case TechType.TrivalveYellowEgg:
                case TechType.TrivalveBlueEggUndiscovered:
                case TechType.TrivalveYellowEggUndiscovered:
                case TechType.BrinewingEgg:
                case TechType.BrinewingEggUndiscovered:
                case TechType.CryptosuchusEgg:
                case TechType.CryptosuchusEggUndiscovered:
                case TechType.GlowWhaleEgg:
                case TechType.GlowWhaleEggUndiscovered:
                case TechType.JellyfishEgg:
                case TechType.JellyfishEggUndiscovered:
                case TechType.PenguinEgg:
                case TechType.PenguinEggUndiscovered:
                case TechType.RockPuncherEgg:
                case TechType.RockPuncherEggUndiscovered:
                case TechType.PrecursorLostRiverLabEgg:
                    return true;
            }

            return false;
        }

        /**
         *
         * Yaratık yumurtasına dönüştürür.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static TechType ToCreatureEgg(this TechType techType)
        {
            switch (techType)
            {
                case TechType.ShockerEggUndiscovered:           return TechType.ShockerEgg;
                case TechType.ArcticRayEggUndiscovered:         return TechType.ArcticRayEgg;
                case TechType.BruteSharkEggUndiscovered:        return TechType.BruteSharkEgg;
                case TechType.LilyPaddlerEggUndiscovered:       return TechType.LilyPaddlerEgg;
                case TechType.PinnacaridEggUndiscovered:        return TechType.PinnacaridEgg;
                case TechType.SquidSharkEggUndiscovered:        return TechType.SquidSharkEgg;
                case TechType.TitanHolefishEggUndiscovered:     return TechType.TitanHolefishEgg;
                case TechType.TrivalveBlueEggUndiscovered:      return TechType.TrivalveBlueEgg;
                case TechType.TrivalveYellowEggUndiscovered:    return TechType.TrivalveYellowEgg;
                case TechType.BrinewingEggUndiscovered:         return TechType.BrinewingEgg;
                case TechType.CryptosuchusEggUndiscovered:      return TechType.CryptosuchusEgg;
                case TechType.GlowWhaleEggUndiscovered:         return TechType.GlowWhaleEgg;
                case TechType.JellyfishEggUndiscovered:         return TechType.JellyfishEgg;
                case TechType.PenguinEggUndiscovered:           return TechType.PenguinEgg;
                case TechType.RockPuncherEggUndiscovered:       return TechType.RockPuncherEgg;

                case TechType.PrecursorLostRiverLabEgg:
                case TechType.SeaMonkeyEgg:
                case TechType.GenericEgg:
                case TechType.LavaZoneEgg:
                case TechType.ShockerEgg:
                case TechType.ArcticRayEgg:
                case TechType.BruteSharkEgg:
                case TechType.LilyPaddlerEgg:
                case TechType.PinnacaridEgg:
                case TechType.SquidSharkEgg:
                case TechType.TitanHolefishEgg:
                case TechType.TrivalveBlueEgg:
                case TechType.TrivalveYellowEgg:
                case TechType.BrinewingEgg:
                case TechType.CryptosuchusEgg:
                case TechType.GlowWhaleEgg:
                case TechType.JellyfishEgg:
                case TechType.PenguinEgg:
                case TechType.RockPuncherEgg:
                    return techType;
            }

            return TechType.None;
        }

        /**
         *
         * Balık/Yaratık olup olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsCreature(this TechType techType, bool ignoreSchool = false)
        {
            switch (techType)
            {
                case TechType.Spinefish:
                case TechType.Skyray:
                case TechType.Boomerang:
                case TechType.Bladderfish:
                case TechType.Hoopfish:
                case TechType.Crash:
                case TechType.GlowWhale:
                case TechType.LilyPaddler:
                case TechType.Penguin:
                case TechType.PenguinBaby:
                case TechType.Pinnacarid:
                case TechType.RockPuncher:
                case TechType.SpinnerFish:
                case TechType.ArcticRay:
                case TechType.Rockgrub:
                case TechType.Symbiote:
                case TechType.BruteShark:
                case TechType.TrivalveBlue:
                case TechType.TrivalveYellow:
                case TechType.ArcticPeeper:
                case TechType.ArrowRay:
                case TechType.SeaMonkey:
                case TechType.TitanHolefish:
                case TechType.NootFish:
                case TechType.Brinewing:
                case TechType.Triops:
                case TechType.SquidShark:
                case TechType.SeaMonkeyBaby:
                case TechType.Chelicerate:
                case TechType.SnowStalker:
                case TechType.SnowStalkerBaby:
                case TechType.FeatherFish:
                case TechType.FeatherFishRed:
                case TechType.ShadowLeviathan:
                case TechType.Jellyfish:
                case TechType.DiscusFish:
                case TechType.Cryptosuchus:
                case TechType.IceWorm:
                case TechType.LargeVentGarden:
                case TechType.SmallVentGarden:
                case TechType.GhostLeviathan:
                    return true;
                case TechType.HoopfishSchool:
                case TechType.BladderFishSchool:
                case TechType.HoleFishSchool:
                case TechType.BoomerangFishSchool:
                case TechType.SpinefishSchool:
                    return !ignoreSchool;
            }

            return false;
        }

        /**
         *
         * Nesne respawn süresini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static int GetRespawnDuration(this TechType techType)
        {
            if (techType.IsDrillable())
            {
                return Tools.Random.Next(600, 900);
            }
            else if (techType.IsCreatureEgg())
            {
                return Tools.Random.Next(1200, 1800);
            }
            else if (techType == TechType.CrashHome)
            {
                return -1;
            }
            else if (techType.IsFragment())
            {
                return -1;
            }
            else if (techType.IsBreakable())
            {
                return Tools.Random.Next(480, 720);
            }
            else if (techType == TechType.HeatFruitPlant || techType == TechType.LeafyFruitPlant)
            {
                return Tools.Random.Next(60, 180);
            }
            else if (techType == TechType.Creepvine || techType == TechType.CreepvineSeedCluster)
            {
                return 240;
            }

            return Tools.Random.Next(180, 300);
        }
    }
}