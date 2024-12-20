namespace Subnautica.API.Features.Creatures.MonoBehaviours.Shared
{
    using System.Collections;

    using Subnautica.API.Extensions;
    using Subnautica.Network.Structures;

    using UnityEngine;
    using UWE;

    public class MultiplayerLeviathanMeleeAttack : BaseMultiplayerCreature
    {
        /**
         *
         * LeviathanMeleeAttack sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::LeviathanMeleeAttack LeviathanMeleeAttack { get; set; }

        /**
         *
         * MultiplayerMeleeAttack sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private MultiplayerMeleeAttack MultiplayerMeleeAttack { get; set; }

        /**
         *
         * Sınıf uyanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Awake()
        {
            this.LeviathanMeleeAttack   = this.GetComponent<global::LeviathanMeleeAttack>();
            this.MultiplayerMeleeAttack = this.GetComponent<MultiplayerMeleeAttack>();
        }

        /**
         *
         * Sahiplik değiştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnChangedOwnership()
        {
            this.LeviathanMeleeAttack.ReleaseVehicle(true);
        }

        /**
         *
         * Pasif olurken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnDisable()
        {
            this.LeviathanMeleeAttack.ReleaseVehicle(true);
        }

        /**
         *
         * Yakın dövüş saldırısını başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool StartMeleeAttack(ZeroLastTarget lastTarget)
        {
            if (this.MultiplayerCreature.CreatureItem.IsMine())
            {
                var target = lastTarget.GetGameObject();
                if (target == null)
                {
                    return false;
                }

                this.LeviathanMeleeAttack.creature.Aggression.FullOn();

                if (lastTarget.IsPlayer())
                {
                    if (lastTarget.IsDead)
                    {
                        this.LeviathanMeleeAttack.cinematicActive = true;
                        this.LeviathanMeleeAttack.timeLastBite    = Time.time;

                        if (this.LeviathanMeleeAttack.timelineManager)
                        {
                            this.LeviathanMeleeAttack.timelineManager.OnCinematicStart();
                        }

                        ZeroPlayer.CurrentPlayer.Main.GetPDA().Close();
                        Inventory.main.quickSlots.DeselectImmediate();

                        this.LeviathanMeleeAttack.playerAttackCinematicController.StartCinematicMode(ZeroPlayer.CurrentPlayer.Main);
                        this.LeviathanMeleeAttack.playerAttackSfx.Play();
                        this.LeviathanMeleeAttack.creature.Aggression.Add(0f - this.LeviathanMeleeAttack.biteAggressionDecrement);
                    }
                    else
                    {
                        this.MultiplayerMeleeAttack.StartMeleeAttack(target);
                    }
                }
                else if (lastTarget.IsExosuit())
                {
                    if (target.TryGetComponent<global::Exosuit>(out var exosuit))
                    {
                        this.LeviathanMeleeAttack.GrabExosuit(exosuit);
                    }
                }
                else if (lastTarget.IsSeatruck())
                {
                    if (target.TryGetComponent<global::SeaTruckSegment>(out var seaTruckSegment))
                    {
                        this.LeviathanMeleeAttack.GrabSeatruck(seaTruckSegment);
                    }
                }
                else
                {
                    this.MultiplayerMeleeAttack.StartMeleeAttack(target);
                }

                return true;
            }

            return false;
        }


        /**
         *
         * Diğer oyuncular için saldırıyı simüle eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool SimulateMeleeAttack(ZeroLastTarget lastTarget)
        {
            var target = lastTarget.GetGameObject();
            if (target == null)
            {
                return false;
            }

            if (!lastTarget.IsPlayer() && !lastTarget.IsVehicle())
            {
                return false;
            }

            if (lastTarget.IsPlayer())
            {
                this.MultiplayerMeleeAttack.StartMeleeAttack(target);
                return true;
            }

            if (this.LeviathanMeleeAttack.holdingVehicle)
            {
                Log.Info("HOLDING VEHICLE ==> " + this.LeviathanMeleeAttack.holdingVehicle);
                this.LeviathanMeleeAttack.ReleaseVehicle(true);
            }

            this.LeviathanMeleeAttack.heldSeatruck           = null;
            this.LeviathanMeleeAttack.heldExosuit            = null;
            this.LeviathanMeleeAttack.timeVehicleGrabbed     = Time.time;
            this.LeviathanMeleeAttack.vehicleInitialPosition = target.transform.position;
            this.LeviathanMeleeAttack.vehicleInitialRotation = target.transform.rotation;
            this.LeviathanMeleeAttack.creature.Aggression.FullOff();
            this.LeviathanMeleeAttack.CancelInvoke("DamageVehicle");

            if (lastTarget.IsExosuit())
            {
                if (target.TryGetComponent<global::Exosuit>(out var exosuit))
                {
                    this.LeviathanMeleeAttack.heldExosuit          = exosuit;
                    this.LeviathanMeleeAttack.defaultCullingMode   = this.LeviathanMeleeAttack.animator.cullingMode;
                    this.LeviathanMeleeAttack.animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                    this.LeviathanMeleeAttack.animator.SetBool("exosuit_attack", true);
                    this.LeviathanMeleeAttack.exosuitAttackStartSfx.Play();
                    this.LeviathanMeleeAttack.exosuitAttackLoopSfx.Play();
                }
            }
            else if (lastTarget.IsSeatruck())
            {
                if (target.TryGetComponent<global::SeaTruckSegment>(out var seaTruckSegment))
                {
                    this.LeviathanMeleeAttack.heldSeatruck         = seaTruckSegment;
                    this.LeviathanMeleeAttack.defaultCullingMode   = this.LeviathanMeleeAttack.animator.cullingMode;
                    this.LeviathanMeleeAttack.animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                    this.LeviathanMeleeAttack.animator.SetBool("seatruck_attack", true);
                    this.LeviathanMeleeAttack.heldSeatruck.animator.SetBool(this.LeviathanMeleeAttack.seatruckStartAttackAnimation, true);
                    this.LeviathanMeleeAttack.seatruckAttackStartSfx.Play();
                    this.LeviathanMeleeAttack.seatruckAttackLoopSfx.Play();
                }
            }

            if (this.LeviathanMeleeAttack.timelineManager)
            {
                this.LeviathanMeleeAttack.timelineManager.OnCinematicStart();
                this.StartCoroutine(FixShadowLeviathanCollider(target));
            }

            this.LeviathanMeleeAttack.holdingVehicle = this.LeviathanMeleeAttack.heldSeatruck || this.LeviathanMeleeAttack.heldExosuit;
            this.LeviathanMeleeAttack.SetVehicleIsPilotedSfxParam(false);
            this.LeviathanMeleeAttack.SyncUpdatingState();
            return true;
        }

        /**
         *
         * Shadow Leviathan collider sorunnu düzeltir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private IEnumerator FixShadowLeviathanCollider(GameObject target)
        {
            var player = global::Player.main;

            for (int i = 0; i < 50; i++)
            {
                yield return CoroutineUtils.waitForFixedUpdate;

                if (this.LeviathanMeleeAttack == null || target == null || player == null || player.transform.parent == null || player.transform.parent.gameObject.GetTechType() != TechType.SeaTruck)
                {
                    continue;
                }

                if (target != player.transform.parent.gameObject)
                {
                    continue;
                }

                if (target.TryGetComponent<global::SeaTruckSegment>(out var seaTruckSegment))
                {
                    if (seaTruckSegment.isRearConnected)
                    {
                        continue;
                    }

                    seaTruckSegment.Exit();
                }
            }
        }
    }
}
