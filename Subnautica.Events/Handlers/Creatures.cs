namespace Subnautica.Events.Handlers
{
    using Subnautica.Events.EventArgs;

    using static Subnautica.API.Extensions.EventExtensions;

    public class Creatures
    {
        /**
         *
         * Enabled İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<CreatureEnabledEventArgs> Enabled;

        /**
         *
         * Enabled Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnEnabled(CreatureEnabledEventArgs ev) => Enabled.CustomInvoke(ev);

        /**
         *
         * Disabled İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<CreatureDisabledEventArgs> Disabled;

        /**
         *
         * Disabled Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnDisabled(CreatureDisabledEventArgs ev) => Disabled.CustomInvoke(ev);

        /**
         *
         * Freezing İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<CreatureFreezingEventArgs> Freezing;

        /**
         *
         * Freezing Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnFreezing(CreatureFreezingEventArgs ev) => Freezing.CustomInvoke(ev);

        /**
         *
         * MeleeAttacking İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<CreatureMeleeAttackingEventArgs> MeleeAttacking;

        /**
         *
         * MeleeAttacking Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnMeleeAttacking(CreatureMeleeAttackingEventArgs ev) => MeleeAttacking.CustomInvoke(ev);
        
        /**
         *
         * CreatureAttackLastTargetStopped İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<CreatureAttackLastTargetStoppedEventArgs> CreatureAttackLastTargetStopped;

        /**
         *
         * CreatureAttackLastTargetStopped Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnCreatureAttackLastTargetStopped(CreatureAttackLastTargetStoppedEventArgs ev) => CreatureAttackLastTargetStopped.CustomInvoke(ev);

        /**
         *
         * LeviathanMeleeAttacking İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<CreatureLeviathanMeleeAttackingEventArgs> LeviathanMeleeAttacking;

        /**
         *
         * LeviathanMeleeAttacking Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnLeviathanMeleeAttacking(CreatureLeviathanMeleeAttackingEventArgs ev) => LeviathanMeleeAttacking.CustomInvoke(ev);

        /**
         *
         * CreatureAttackLastTargetStarting İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<CreatureAttackLastTargetStartingEventArgs> CreatureAttackLastTargetStarting;

        /**
         *
         * CreatureAttackLastTargetStarting Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnCreatureAttackLastTargetStarting(CreatureAttackLastTargetStartingEventArgs ev) => CreatureAttackLastTargetStarting.CustomInvoke(ev);

        /**
         *
         * CallSoundTriggering İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<CreatureCallSoundTriggeringEventArgs> CallSoundTriggering;

        /**
         *
         * CallSoundTriggering Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnCallSoundTriggering(CreatureCallSoundTriggeringEventArgs ev) => CallSoundTriggering.CustomInvoke(ev);

        /**
         *
         * GlowWhaleSFXTriggered İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<GlowWhaleSFXTriggeredEventArgs> GlowWhaleSFXTriggered;

        /**
         *
         * GlowWhaleSFXTriggered Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnGlowWhaleSFXTriggered(GlowWhaleSFXTriggeredEventArgs ev) => GlowWhaleSFXTriggered.CustomInvoke(ev);

        /**
         *
         * GlowWhaleRideStarting İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<GlowWhaleRideStartingEventArgs> GlowWhaleRideStarting;

        /**
         *
         * GlowWhaleRideStarting Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnGlowWhaleRideStarting(GlowWhaleRideStartingEventArgs ev) => GlowWhaleRideStarting.CustomInvoke(ev);

        /**
         *
         * GlowWhaleRideStoped İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<GlowWhaleRideStopedEventArgs> GlowWhaleRideStoped;

        /**
         *
         * GlowWhaleRideStoped Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnGlowWhaleRideStoped(GlowWhaleRideStopedEventArgs ev) => GlowWhaleRideStoped.CustomInvoke(ev);

        /**
         *
         * GlowWhaleEyeCinematicStarting İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<GlowWhaleEyeCinematicStartingEventArgs> GlowWhaleEyeCinematicStarting;

        /**
         *
         * GlowWhaleEyeCinematicStarting Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnGlowWhaleEyeCinematicStarting(GlowWhaleEyeCinematicStartingEventArgs ev) => GlowWhaleEyeCinematicStarting.CustomInvoke(ev);

        /**
         *
         * AnimationChanged İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<CreatureAnimationChangedEventArgs> AnimationChanged;

        /**
         *
         * AnimationChanged Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnAnimationChanged(CreatureAnimationChangedEventArgs ev) => AnimationChanged.CustomInvoke(ev);

        /**
         *
         * CrashFishInflating İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<CrashFishInflatingEventArgs> CrashFishInflating;

        /**
         *
         * CrashFishInflating Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnCrashFishInflating(CrashFishInflatingEventArgs ev) => CrashFishInflating.CustomInvoke(ev);

        /**
         *
         * LilyPaddlerHypnotizeStarting İşleyicisi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static event SubnauticaPluginEventHandler<LilyPaddlerHypnotizeStartingEventArgs> LilyPaddlerHypnotizeStarting;

        /**
         *
         * LilyPaddlerHypnotizeStarting Olayı 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnLilyPaddlerHypnotizeStarting(LilyPaddlerHypnotizeStartingEventArgs ev) => LilyPaddlerHypnotizeStarting.CustomInvoke(ev);
    }
}
