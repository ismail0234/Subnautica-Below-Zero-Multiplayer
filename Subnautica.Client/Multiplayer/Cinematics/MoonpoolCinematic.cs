namespace Subnautica.Client.Multiplayer.Cinematics
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Extensions;
    using Subnautica.Client.MonoBehaviours.Player;
    using Subnautica.Client.MonoBehaviours.World;

    public class MoonpoolCinematic : CinematicController
    {
        /**
        *
        * Hoverpadi barındırır.
        *
        * @author Ismail <ismaiil_0234@hotmail.com>
        *
        */
        private global::VehicleDockingBay DockingBay { get; set; }

        /**
         *
         * Animasyonu resetler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnResetAnimations(PlayerCinematicQueueItem item)
        {
            this.DockingBay = this.Target.GetComponentInChildren<global::VehicleDockingBay>();

            if (item.CinematicAction == this.StartDockingCinematic)
            {
                this.DockingBay.dockPlayerCinematic.animator.Rebind();
                this.DockingBay.exosuitDockPlayerCinematic.animator.Rebind();

                this.ZeroPlayer.IsVehicleDocking = true;
            }
        }

        /**
         *
         * Demirleme animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void StartDockingCinematic()
        {
            if (this.GetProperty<TechType>("TechType") == TechType.Exosuit)
            {
                this.ZeroPlayer.ExitVehicle();

                this.SetCinematic(this.DockingBay.exosuitDockPlayerCinematic, isSkipEndAnimation: true);
            }
            else
            {
                this.ZeroPlayer.GetComponent<PlayerVehicleManagement>().VehicleUniqueId = null;
                this.ZeroPlayer.ExitVehicle();

                this.ZeroPlayer.SetInteriorId(null);
                this.ZeroPlayer.GetComponent<PlayerAnimation>().FixedUpdate();

                this.SetCinematic(this.DockingBay.dockPlayerCinematic);
            }

            this.SetCinematicEndMode(this.OnDockingComplete);
            this.StartCinematicMode();
        }

        /**
         *
         * Ayrılma animasyonunu başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void StartUnDockingCinematic()
        {
            foreach (var item in this.DockingBay.transform.parent.GetComponentsInChildren<DockedVehicleHandTarget>())
            {
                if ((item.name.Contains("Left") && this.GetProperty<bool>("IsLeft")) || (item.name.Contains("Right") && !this.GetProperty<bool>("IsLeft")))
                {
                    if (item.dockingBay.GetDockedObject() == null)
                    {
                        continue;
                    }

                    if (CraftData.GetTechType(item.dockingBay.GetDockedObject().gameObject) == TechType.Exosuit)
                    {
                        this.SetCinematic(item.exosuitCinematicController, isSkipFirstAnimation: false, isFastInterpolation: false);
                    }
                    else
                    {
                        this.SetCinematic(item.seamothCinematicController, isSkipFirstAnimation: false, isFastInterpolation: false);
                    }

                    this.OnUndockingStart();
                    this.SetCinematicEndMode(this.OnUndockingComplete);
                    this.StartCinematicMode();
                }
            }
        }

        /**
         *
         * Docking başlarken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void OnDockingComplete()
        {
            this.DockingBay?.dockedObject?.OnDockingComplete();
        }

        /**
         *
         * Undocking başlarken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnUndockingStart()
        {
            if (this.DockingBay)
            {
                this.DockingBay.docked_param = false;

                if (this.DockingBay.disableDockableCollisionInProcess)
                {
                    this.DockingBay.dockedObject.rb.detectCollisions = false;
                    this.DockingBay.dockedObject.isInTransition = true;
                }

                this.DockingBay.LockSeatruckRedocking();
            }
        }

        /**
         *
         * Undocking tamamlanırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void OnUndockingComplete()
        {
            this.SetAnimState(false);

            if (this.DockingBay?.dockedObject && this.DockingBay.TryGetComponent<MultiplayerVehicleDockingBay>(out var docking))
            {
                var vehicleId = this.DockingBay.dockedObject.gameObject.GetIdentityId();

                docking.Undock();

                var entity = Network.DynamicEntity.GetEntity(vehicleId);
                if (entity != null)
                {
                    this.ZeroPlayer.RefreshVehicle(entity.Id);
                }
            }
        }
    }
}
