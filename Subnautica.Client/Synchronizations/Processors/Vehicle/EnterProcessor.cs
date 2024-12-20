namespace Subnautica.Client.Synchronizations.Processors.Vehicle
{
    using Subnautica.API.Enums;
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class EnterProcessor : NormalProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.VehicleEnterArgs>();
            if (packet.UniqueId.IsNull())
            {
                return false;
            }

            var player = ZeroPlayer.GetPlayerById(packet.GetPacketOwnerId());
            if (player == null)
            {
                return false;
            }

            if (packet.Vehicle != null)
            {
                Network.DynamicEntity.AddEntity(packet.Vehicle);
            }

            var entity = Network.DynamicEntity.GetEntity(packet.UniqueId);
            if (entity == null)
            {
                return false;
            }

            API.Features.Log.Info("EnterProcessor 1 ==> " + packet.Vehicle?.UniqueId + ", pos: " + packet.Vehicle?.Position);
            API.Features.Log.Info("EnterProcessor 2 ==> " + packet.UniqueId + ", pos: " + packet.Position);

            entity.IsUsingByPlayer = true;

            player.VehiclePosition = packet.Position.ToVector3();
            player.VehicleRotation = packet.Rotation.ToQuaternion();
            player.ResetCinematicsByUniqueId(packet.UniqueId);

            if (packet.TechType == TechType.Hoverbike)
            {
                var vehicle = Network.Identifier.GetComponentByGameObject<global::Hoverbike>(packet.UniqueId);
                if (vehicle == null)
                {
                    return false;
                }

                ZeroLiveMixin.AddHealth(vehicle.liveMixin, packet.Health);

                if (player.IsMine)
                {
                    vehicle.EnterVehicle();
                }
            }
            else if (packet.TechType == TechType.Exosuit)
            {
                var vehicle = Network.Identifier.GetComponentByGameObject<global::Exosuit>(packet.UniqueId);
                if (vehicle == null)
                {
                    return false;
                }

                vehicle.mainAnimator.Rebind();

                if (player.IsMine)
                {
                    vehicle.useRigidbody.SetNonKinematic(true);
                    vehicle.EnterVehicle(global::Player.main, true);
                }
                else
                {
                    vehicle.useRigidbody.SetKinematic();
                }
            }
            else if (packet.TechType == TechType.SpyPenguin)
            {
                var vehicle = Network.Identifier.GetComponentByGameObject<global::SpyPenguin>(packet.UniqueId);
                if (vehicle == null)
                {
                    return false;
                }

                ZeroLiveMixin.AddHealth(vehicle.liveMixin, packet.Health);

                if (player.IsMine)
                {
                    SpyPenguinRemoteManager.main.currentPenguin = vehicle;
                    SpyPenguinRemoteManager.main.currentPenguin.EnablePenguinCam();
                }
            }
            else if (packet.TechType.IsSeaTruckModule(true))
            {
                var vehicle = Network.Identifier.GetComponentByGameObject<global::SeaTruckMotor>(packet.UniqueId);
                if (vehicle == null)
                {
                    return false;
                }

                ZeroLiveMixin.AddHealth(vehicle.liveMixin, packet.Health);

                if (player.IsMine)
                {
                    vehicle.StartPiloting();

                    if (vehicle.seatruckanimation)
                    {
                        vehicle.seatruckanimation.currentAnimation = SeaTruckAnimation.Animation.BeginPilot;
                    }
                }
                else
                {
                    vehicle.useRigidbody.SetKinematic();
                }
            }
            else if (packet.TechType == TechType.MapRoomCamera)
            {
                var mapRoom = Network.Identifier.GetComponentByGameObject<global::BaseDeconstructable>(packet.CustomId)?.GetMapRoomFunctionality();
                var camera  = Network.Identifier.GetComponentByGameObject<global::MapRoomCamera>(packet.UniqueId);
                if (mapRoom == null || camera == null)
                {
                    return false;
                }

                ZeroLiveMixin.AddHealth(camera.liveMixin, packet.Health);

                player.SetUsingRoomId(packet.CustomId);

                if (player.IsMine)
                {
                    var mapRoomScreen = mapRoom.GetComponentInChildren<MapRoomScreen>();
                    if (mapRoomScreen)
                    {
                        if (mapRoomScreen.currentCamera && mapRoomScreen.currentCamera != camera)
                        {
                            using (EventBlocker.Create(ProcessType.VehicleExit))
                            {
                                mapRoomScreen.currentCamera.FreeCamera(false);
                                mapRoomScreen.currentCamera.ExitLockedMode(false);
                            }
                        }

                        camera.rigidBody.SetNonKinematic();
                        camera.ControlCamera(global::Player.main, mapRoomScreen);

                        mapRoomScreen.currentCamera = camera;
                    }
                }

                if (camera.dockingPoint != null)
                {
                    using (EventBlocker.Create(TechType.MapRoomCamera))
                    {
                        camera.dockingPoint.UndockCamera();
                    }
                }
            }

            return true;
        }

        /**
         *
         * Araca binerken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnVehicleEntering(VehicleEnteringEventArgs ev)
        {
            ev.IsAllowed = false;

            if (!Network.HandTarget.IsBlocked(ev.UniqueId))
            {
                ServerModel.VehicleEnterArgs request = new ServerModel.VehicleEnterArgs()
                {
                    UniqueId = ev.UniqueId,
                    TechType = ev.TechType,
                };

                NetworkClient.SendPacket(request);
            }
        }
    }
}