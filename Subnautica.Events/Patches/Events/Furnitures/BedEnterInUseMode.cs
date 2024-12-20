namespace Subnautica.Events.Patches.Events.Furnitures
{
    using System;

    using HarmonyLib;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    [HarmonyPatch(typeof(global::Bed), nameof(global::Bed.OnHandClick))]
    public static class BedEnterInUseMode
    {
        /**
         *
         * Fonksiyonu yamalar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool Prefix(global::Bed __instance, global::GUIHand hand)
        {
            if (!Network.IsMultiplayerActive)
            {
                return true;
            }

            var bedItem = BedInformationItem.GetInformation(__instance);
            if (bedItem == null)
            {
                return false;
            }

            __instance.isValidHandTarget = __instance.GetCanSleep(hand.player, true);
            if (!__instance.isValidHandTarget)
            {
                return false;
            }

            var side = __instance.GetSide(hand.player);
            if (side == global::Bed.BedSide.None)
            {
                ErrorMessage.AddWarning(global::Language.main.Get("NotEnoughSpaceToLieDown"));
                return false;
            }

            try
            {
                BedEnterInUseModeEventArgs args = new BedEnterInUseModeEventArgs(bedItem.UniqueId, side, bedItem.TechType, bedItem.IsSeaTruckModule);

                Handlers.Furnitures.OnBedEnterInUseMode(args);

                return args.IsAllowed;
            }
            catch (Exception e)
            {
                Log.Error($"Furnitures.BedEnterInUseMode: {e}\n{e.StackTrace}");
                return true;
            }
        }
    }
    public class BedInformationItem
    {
        /**
         *
         * TechType barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public TechType TechType { get; set; }

        /**
         *
         * UniqueId barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string UniqueId { get; set; }

        /**
         *
         * IsSeaTruckModule barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsSeaTruckModule { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public BedInformationItem(string uniqueId, TechType techType, bool isSeaTruckModule)
        {
            this.UniqueId         = uniqueId;
            this.TechType         = techType;
            this.IsSeaTruckModule = isSeaTruckModule;
        }

        /**
         *
         * Bilgileri döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static BedInformationItem GetInformation(global::Bed __instance)
        {
            var constructable = __instance.GetComponentInParent<global::Constructable>();
            if (constructable)
            {
                return new BedInformationItem(constructable.gameObject.GetIdentityId(), constructable.techType, false);
            }

            var seaTruckSegment = __instance.GetComponentInParent<global::SeaTruckSegment>();
            if (seaTruckSegment)
            {
                return new BedInformationItem(seaTruckSegment.gameObject.GetIdentityId(), TechType.Bed1, true);
            }

            return null;
        }
    }
}