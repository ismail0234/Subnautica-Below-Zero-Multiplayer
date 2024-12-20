namespace Subnautica.Events.Handlers
{
    using Subnautica.Events.EventArgs;

    using static Subnautica.API.Extensions.EventExtensions;

    public class Building
    {
        /**
         *
         * ConstructingGhostMoved İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ConstructionGhostMovedEventArgs> ConstructingGhostMoved;

        /**
         *
         * ConstructingGhostMoved Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnConstructingGhostMoved(ConstructionGhostMovedEventArgs ev) => ConstructingGhostMoved.CustomInvoke(ev);

        /**
         *
         * ConstructingGhostTryPlacing İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ConstructionGhostTryPlacingEventArgs> ConstructingGhostTryPlacing;

        /**
         *
         * ConstructingGhostTryPlacing Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnConstructingGhostTryPlacing(ConstructionGhostTryPlacingEventArgs ev) => ConstructingGhostTryPlacing.CustomInvoke(ev);

        /**
         *
         * ConstructingAmountChanged İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ConstructionAmountChangedEventArgs> ConstructingAmountChanged;

        /**
         *
         * ConstructingGhostTryPlacing Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnConstructingAmountChanged(ConstructionAmountChangedEventArgs ev) => ConstructingAmountChanged.CustomInvoke(ev);

        /**
         *
         * ConstructingCompleted İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ConstructionCompletedEventArgs> ConstructingCompleted;

        /**
         *
         * ConstructingCompleted Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnConstructingCompleted(ConstructionCompletedEventArgs ev) => ConstructingCompleted.CustomInvoke(ev);
        /**
         *
         * ConstructingRemoved İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<ConstructionRemovedEventArgs> ConstructingRemoved;

        /**
         *
         * ConstructingRemoved Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnConstructingRemoved(ConstructionRemovedEventArgs ev) => ConstructingRemoved.CustomInvoke(ev);

        /**
         *
         * DeconstructionBegin İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<DeconstructionBeginEventArgs> DeconstructionBegin;

        /**
         *
         * DeconstructionBegin Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnDeconstructionBegin(DeconstructionBeginEventArgs ev) => DeconstructionBegin.CustomInvoke(ev);

        /**
         *
         * FurnitureDeconstructionBegin İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<FurnitureDeconstructionBeginEventArgs> FurnitureDeconstructionBegin;

        /**
         *
         * FurnitureDeconstructionBegin Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnFurnitureDeconstructionBegin(FurnitureDeconstructionBeginEventArgs ev) => FurnitureDeconstructionBegin.CustomInvoke(ev);

        /**
         *
         * BaseHullStrengthCrushing İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<BaseHullStrengthCrushingEventArgs> BaseHullStrengthCrushing;

        /**
         *
         * BaseHullStrengthCrushing Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnBaseHullStrengthCrushing(BaseHullStrengthCrushingEventArgs ev) => BaseHullStrengthCrushing.CustomInvoke(ev);
    }
}