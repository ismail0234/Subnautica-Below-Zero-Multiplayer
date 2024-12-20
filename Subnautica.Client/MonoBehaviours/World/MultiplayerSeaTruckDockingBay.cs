namespace Subnautica.Client.MonoBehaviours.World
{
    using UnityEngine;

    using Subnautica.API.Features;
    using Subnautica.Network.Structures;
    using Subnautica.API.Extensions;

    public class MultiplayerSeaTruckDockingBay : MonoBehaviour
    {
        /**
         *
         * DockingBay nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private SeaTruckDockingBay DockingBay { get; set; }

        /**
         *
         * DockedVehicle nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private global::Vehicle DockedVehicle { get; set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Awake()
        {
            this.DockingBay    = this.GetComponentInChildren<SeaTruckDockingBay>();
            this.DockedVehicle = null;
        }

        /**
         *
         * Rıhtıma yanaşma işlemini başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void StartDocking(string vehicleId, bool playSound = true, bool fastTeleport = false)
        {
            var vehicle = Network.Identifier.GetGameObject(vehicleId);
            if (vehicle && vehicle.TryGetComponent<Dockable>(out var dockAble))
            {
                if (vehicle.TryGetComponent<global::Exosuit>(out var exosuit))
                {
                    exosuit.leftArm?.ResetArm();
                    exosuit.rightArm?.ResetArm();
                }

                if (playSound)
                {
                    Utils.PlayFMODAsset(this.DockingBay.dockSound, this.DockingBay.transform);
                }

                this.DockingBay.dockedObject = dockAble;
                this.SetDocked(this.DockingBay, global::Vehicle.DockType.Seatruck, true);
                this.DockingBay.dockedObject.transform.parent = this.DockingBay.dockingPosition;
                this.DockingBay.truckSegment.SetWeight(1.2f);
                this.DockingBay.ejectButton.SetActive(true);
                this.DockingBay.enterExosuitTrigger.SetActive(true);

                if (fastTeleport)
                {
                    this.DockingBay.dockedObject.transform.localPosition = Vector3.zero;
                    this.DockingBay.dockedObject.transform.localRotation = Quaternion.identity;
                }
            }
        }

        /**
         *
         * Rıhtım'dan ayrılma işlemini başlatır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void StartUndocking(byte playerId, string ownershipId, bool isEnterUndock, ZeroVector3 undockPosition)
        {
            this.EjectDocked(ZeroPlayer.IsPlayerMine(ownershipId));

            if (this.DockedVehicle && isEnterUndock && ZeroPlayer.IsPlayerMine(playerId))
            {
                this.DockedVehicle.EnterVehicle(global::Player.main, true);
            }

            if (this.DockedVehicle)
            {
                if (undockPosition.Distance(this.DockedVehicle.transform.position) > 25f)
                {
                    this.DockedVehicle.transform.position = undockPosition.ToVector3();
                }
            }

            this.DockedVehicle = null;
        }

        /**
         *
         * Araç bağlantısını keser.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void EjectDocked(bool isMine)
        {
            if (this.DockingBay)
            {
                this.DockedVehicle = this.DockingBay.dockedObject.vehicle;

                this.SetDocked(null, global::Vehicle.DockType.Seatruck, isMine);

                if (this.DockingBay.undockingForce != 0.0f)
                {
                    this.DockingBay.dockedObject.rb.AddForce(this.DockingBay.dockingPosition.forward * this.DockingBay.undockingForce, ForceMode.VelocityChange);
                }
                    
                this.DockingBay.dockedObject = null;
                this.DockingBay.timeUndocked = Time.time;
                this.DockingBay.truckSegment.SetWeight(0.5f);

                Utils.PlayFMODAsset(this.DockingBay.undockSound, this.transform);
            }

            this.DockingBay.ejectButton.SetActive(false);
            this.DockingBay.enterExosuitTrigger.SetActive(false);
        }

        /**
         *
         * Dock durumunu ayarlar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetDocked(IDockingBay bay, global::Vehicle.DockType dockType, bool isMine)
        {
            this.DockingBay.dockedObject.bay = bay;

            if (bay != null)
            {
                if (this.DockingBay.dockedObject.vehicle)
                {
                    this.DockingBay.dockedObject.vehicle.EjectPlayer(dockType == global::Vehicle.DockType.Seatruck);
                    this.DockingBay.dockedObject.vehicle.docked = true;
                }

                this.DockingBay.dockedObject.rb.SetKinematic();
            }
            else
            {
                this.DockingBay.dockedObject.transform.parent = null;

                if (this.DockingBay.dockedObject.vehicle)
                {
                    this.DockingBay.dockedObject.vehicle.docked = false;
                }

                if (isMine)
                {
                    this.DockingBay.dockedObject.rb.SetNonKinematic();
                }
                else
                {
                    this.DockingBay.dockedObject.rb.SetKinematic();
                }
            }

            if (this.DockingBay.dockedObject.truckSegment)
            {
                this.DockingBay.dockedObject.truckSegment.SetDockedCollisionEnabled(bay == null);
            }
        }
    }
}
