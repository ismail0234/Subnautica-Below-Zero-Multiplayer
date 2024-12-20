namespace Subnautica.Client.Multiplayer.Cinematics.CreatureCinematics
{
    using Subnautica.Client.MonoBehaviours.Player;
    using Subnautica.API.Features;
    using Subnautica.API.Extensions;

    using UnityEngine;

    public class GlowWhaleCinematic : CinematicController
    {
        /**
         *
         * Balina yüzme sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::GlowWhaleRide GlowWhaleRide { get; set; }

        /**
         *
         * Cinematic sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::PlayerCinematicController Cinematic { get; set; }

        /**
         *
         * Animasyonu resetler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnResetAnimations(PlayerCinematicQueueItem item)
        {
            this.GlowWhaleRide = this.Target.GetComponentInChildren<global::GlowWhaleRide>();
            this.Cinematic     = this.Target.GetComponentInChildren<PlayerCinematicController>();
        }

        /**
         *
         * Sürme animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void StartRideCinematic()
        {
            this.PlayerAnimator.SetBool(this.GlowWhaleRide.playerRideAnimation, true);
            this.PlayerAnimator.SetBool(GlowWhaleRide.animCinematicWithPitch, true);

            this.GlowWhaleRide.animator.SetBool(this.GlowWhaleRide.rideAnimation, true);
            this.GlowWhaleRide.director.SetBinding("Player", this.PlayerAnimator, typeof(Animator));
            this.GlowWhaleRide.director.Play();
            this.GlowWhaleRide.attachSfx.Play();

            this.DisableResetCinematic();

            this.ZeroPlayer.SetParent(this.GlowWhaleRide.playerAttach, true);
            this.ZeroPlayer.EnableCinematicMode();
        }

        /**
         *
         * Sürme animasyonunu durdurur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void StopRideCinematic()
        {
            if (this.UniqueId.IsNotNull() && Network.Creatures.TryGetCreature(this.UniqueId.ToCreatureId(), out var creature))
            {
                creature.SetBusy(false);
            }

            this.PlayerAnimator.SetBool(this.GlowWhaleRide.playerRideAnimation, false);
            this.PlayerAnimator.SetBool(GlowWhaleRide.animCinematicWithPitch, false);

            this.GlowWhaleRide.animator.SetBool(this.GlowWhaleRide.rideAnimation, false);
            this.GlowWhaleRide.director.Stop();

            this.ZeroPlayer.SetParent(null);
            this.ZeroPlayer.DisableCinematicMode();
        }

        /**
         *
         * Oyuncu bağlantısı kesildiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnPlayerDisconnected()
        {
            if (this.UniqueId.IsNotNull())
            {
                if (this.GlowWhaleRide != null && this.PlayerAnimator.GetBool(this.GlowWhaleRide.playerRideAnimation))
                {
                    this.StopRideCinematic();
                }

                if (this.Cinematic != null && this.Animator != null && this.Animator.GetBool("player_eye_cin"))
                {
                    this.EndCinematicMode();
                }
            }

            return true;
        }

        /**
         *
         * Göz Etkileşim animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void StartEyeInteractCinematic()
        {
            this.SetCinematic(this.Cinematic);
            this.SetCinematicEndMode(this.EyeInteractCinematicEndMode);
            this.StartCinematicMode();
        }

        /**
         *
         * Sinematik sona erdiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void EyeInteractCinematicEndMode()
        {
            if (this.UniqueId.IsNotNull() && Network.Creatures.TryGetCreature(this.UniqueId.ToCreatureId(), out var creature))
            {
                creature.SetBusy(false);
            }
        }
    }
}