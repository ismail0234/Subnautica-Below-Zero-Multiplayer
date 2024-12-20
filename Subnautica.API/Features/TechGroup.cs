namespace Subnautica.API.Features
{
    using System.Collections.Generic;

    public class TechGroup
    {
        /**
         *
         * Yatakları barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static List<TechType> Beds { get; set; } = new List<TechType>()
        {
            TechType.Bed1,
            TechType.Bed2,
            TechType.NarrowBed,
            TechType.BedJeremiah,
            TechType.BedSam,
            TechType.BedZeta,
            TechType.BedDanielle,
            TechType.BedEmmanuel,
            TechType.BedFred,
            TechType.BedParvan,
        };
        
        /**
         *
         * Sandalyeleri barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static List<TechType> Chairs { get; set; } = new List<TechType>()
        {
            TechType.Bench,
            TechType.StarshipChair,
            TechType.StarshipChair2,
            TechType.StarshipChair3,
        };
        
        /**
         *
         * Planter'leri barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static List<TechType> Planters { get; set; } = new List<TechType>()
        {
            TechType.PlanterPot,
            TechType.PlanterPot2,
            TechType.PlanterPot3,
            TechType.PlanterBox,
            TechType.PlanterShelf,
            TechType.FarmingTray,
        };

        /**
         *
         * Enerji Yapıları'nı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static List<TechType> EnergyConstructions { get; set; } = new List<TechType>()
        {
            TechType.SolarPanel,
            TechType.BaseNuclearReactor,
            TechType.BaseBioReactor,
            TechType.ThermalPlant,
        };

        /**
         *
         * Şarj Cihaz Yapıları'nı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static List<TechType> BatteryChargers { get; set; } = new List<TechType>()
        {
            TechType.BatteryCharger,
            TechType.PowerCellCharger
        };

        /**
         *
         * Locker'leri barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static List<TechType> Lockers { get; set; } = new List<TechType>()
        {
            TechType.Locker,
            TechType.SmallLocker,
        };

        /**
         *
         * Reaktör'leri barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static List<TechType> Reactors { get; set; } = new List<TechType>()
        {
            TechType.BaseBioReactor,
            TechType.BaseNuclearReactor,
        };

        /**
         *
         * Global Entity Türleri
         * 
         * 1. Oyunun her yerinden gözükürler.
         * 2. Fizik alanın'dan çıkınca fizik kapatılır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static List<TechType> GlobalEntityTypes { get; set; } = new List<TechType>()
        {
            TechType.SpyPenguin,
            TechType.Hoverbike,
            TechType.Exosuit,
            TechType.SeaTruck,
            TechType.SeaTruckFabricatorModule,
            TechType.SeaTruckStorageModule,
            TechType.SeaTruckAquariumModule,
            TechType.SeaTruckDockingModule,
            TechType.SeaTruckSleeperModule,
            TechType.SeaTruckTeleportationModule,
            TechType.MapRoomCamera,

            TechType.Thumper,
            TechType.Constructor,
            TechType.Beacon,
        };

        /**
         *
         * BatteryCharger Slotları
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static List<string> BatteryChargerSlots = new List<string>()
        {
            "BatteryCharger1",
            "BatteryCharger2",
            "BatteryCharger3",
            "BatteryCharger4"
        };

        /**
         *
         * PowerCellCharger Slotları
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static List<string> PowerCellChargerSlots = new List<string>()
        {
            "PowerCellCharger1",
            "PowerCellCharger2"
        };

        /**
         *
         * Global nesne olup olmadığını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsGlobalEntity(TechType techType)
        {
            return GlobalEntityTypes.Contains(techType);
        }

        /**
         *
         * Batarya Slot miktarını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static byte GetBatterySlotAmount(TechType techType)
        {
            return techType == TechType.BatteryCharger ? (byte) 4 : (byte) 2;
        }

        /**
         *
         * Batarya Slot adını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetBatterySlotId(TechType techType, int index)
        {
            if (techType == TechType.BatteryCharger)
            {
                return string.Format("BatteryCharger{0}", index);
            }

            return string.Format("PowerCellCharger{0}", index);
        }

        /**
         *
         * Kontrol odasının customizer id değerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetBaseControlRoomCustomizerId(string uniqueId)
        {
            return string.Format("{0}_Customizer", uniqueId);
        }

        /**
         *
         * Kontrol odasının mini harita id değerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetBaseControlRoomNavigateId(string uniqueId)
        {
            return string.Format("{0}_NavigateMiniMap", uniqueId);
        }
    }
}
