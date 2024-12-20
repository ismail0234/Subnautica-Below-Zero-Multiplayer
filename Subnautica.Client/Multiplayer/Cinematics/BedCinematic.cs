namespace Subnautica.Client.Multiplayer.Cinematics
{
    using Subnautica.API.Extensions;
    using Subnautica.Client.MonoBehaviours.Player;

    using UnityEngine;

    public class BedCinematic : CinematicController
    {
        /**
         *
         * Yatağı barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::Bed Bed { get; set; }

        /**
         *
         * Uyku modülü mü?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsSleeperModule { get; set; }

        /**
         *
         * Animasyon başlamadan önce tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnStart(PlayerCinematicQueueItem item)
        {
            base.OnStart(item);

            if (item.CinematicAction == this.LieDownStartCinematic)
            {
                Multiplayer.Furnitures.Bed.UpdateBed(this.ZeroPlayer.UniqueId);
            }
            else
            {
                Multiplayer.Furnitures.Bed.ClearBed(this.ZeroPlayer.UniqueId);
            }
        }

        /**
         *
         * Animasyonu resetler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnResetAnimations(PlayerCinematicQueueItem item)
        {
            this.IsSleeperModule = this.Target.name.Contains("SeaTruckSleeperModule");
            if (this.IsSleeperModule)
            {
                this.Bed = this.Target.GetComponentInChildren<global::Bed>();
            }
            else
            {
                this.Bed = this.Target.GetComponent<global::Bed>();
            }

            this.Bed.animator.Rebind();
        }

        /**
         *
         * Yatma animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void LieDownStartCinematic()
        {
            var bedSide = this.GetProperty<global::Bed.BedSide>("Side");

            this.Bed.cinematicController               = this.GetLieDownCinematicController(bedSide);
            this.Bed.currentStandUpCinematicController = this.GetStandupCinematicController(bedSide);
            this.Bed.animator.transform.localPosition  = this.GetAnimationPosition(bedSide);
            this.Bed.ResetAnimParams(this.PlayerAnimator);

            this.SetCinematic(this.Bed.cinematicController);

            if (this.IsSleeperModule)
            {
                this.ZeroPlayer.SetParent(this.Bed.transform);
                this.SetCinematicEndMode(this.LieDownEndCinematicMode, false);
            }

            this.StartCinematicMode();
        }

        /**
         *
         * Kalkma animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void StandupStartCinematic()
        {
            var bedSide = this.GetProperty<global::Bed.BedSide>("Side");

            this.Bed.cinematicController               = this.GetLieDownCinematicController(bedSide);
            this.Bed.currentStandUpCinematicController = this.GetStandupCinematicController(bedSide);
            this.Bed.animator.transform.localPosition  = this.GetAnimationPosition(bedSide);
            this.Bed.ResetAnimParams(this.PlayerAnimator);

            if (this.IsSleeperModule)
            {
                this.PrepareLieDownCinematic(bedSide);
                this.ZeroPlayer.SetParent(this.Bed.transform);
                this.SetCinematic(this.Bed.currentStandUpCinematicController);
                this.SetCinematicEndMode(this.StandupEndCinematicMode, false);
            }
            else
            {
                this.SetCinematic(this.Bed.currentStandUpCinematicController, true);
            }

            this.StartCinematicMode();
        }

        /**
         *
         * Cinematik bittiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void LieDownEndCinematicMode()
        {
            this.Animator.SetBool(this.AnimParam, false);
            this.AnimState = false;
        }

        /**
         *
         * Cinematik bittiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void StandupEndCinematicMode()
        {
            if (this.ZeroPlayer != null && this.ZeroPlayer.IsInSeaTruck)
            {
                this.ZeroPlayer.GetComponent<PlayerAnimation>().UpdateIsInSeaTruck(true, true);
            }
        }

        /**
         *
         * Uyuma sinematik hazırlığını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void PrepareLieDownCinematic(global::Bed.BedSide side)
        {
            var cinematic = this.GetLieDownCinematicController(side);
            if (cinematic != null)
            {
                cinematic.animator.ImmediatelyPlay(cinematic.animParam, true);
                this.PlayerAnimator.ImmediatelyPlay(cinematic.playerViewAnimationName, true);
            }
        }

        /**
         *
         * Yatma animasyon yönünü döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::PlayerCinematicController GetLieDownCinematicController(global::Bed.BedSide side)
        {
            return side == Bed.BedSide.Right ? this.Bed.rightLieDownCinematicController : this.Bed.leftLieDownCinematicController;
        }

        /**
         *
         * Kalkma animasyon yönünü döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::PlayerCinematicController GetStandupCinematicController(global::Bed.BedSide side)
        {
            return side == Bed.BedSide.Right ? this.Bed.rightStandUpCinematicController : this.Bed.leftStandUpCinematicController;
        }

        /**
         *
         * Animasyon pozisyonunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Vector3 GetAnimationPosition(global::Bed.BedSide side)
        {
            return side == Bed.BedSide.Right ? this.Bed.rightAnimPosition : this.Bed.leftAnimPosition;
        }
    }
}
