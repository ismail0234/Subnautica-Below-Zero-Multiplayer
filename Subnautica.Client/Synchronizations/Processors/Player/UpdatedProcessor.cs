namespace Subnautica.Client.Synchronizations.Processors.Player
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Client.Extensions;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Core.Components;
    using Subnautica.Network.Models.Core;

    using UnityEngine;

    using ItemModel = Subnautica.Network.Models.Items;
    using ServerModel = Subnautica.Network.Models.Server;

    public class UpdatedProcessor : NormalProcessor
    {
        /**
         *
         * Default Hand Rotation
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Quaternion DefaultHandRotation { get; set; } = new Quaternion(0f, 0.2f, 0f, 1f);

        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.PlayerUpdatedArgs>();
            if (packet.GetPacketOwnerId() == 0 || !World.IsLoaded || IntroVignette.isIntroActive)
            {
                return false;
            }

            var player = ZeroPlayer.GetPlayerById(packet.GetPacketOwnerId());
            if (player == null)
            {
                return false;
            }

            if (player.IsMine)
            {
                return true;
            }

            if (player.IsCreatedModel)
            {
                player.Position       = packet.CompressedPosition.ToVector3();
                player.Rotation       = packet.CompressedRotation.ToQuaternion();
                player.Equipments     = packet.Equipments;
                player.IsPrecursorArm = packet.IsPrecursorArm;
                player.EmoteIndex     = (float) packet.EmoteIndex;
                player.VehicleId      = 0;
                player.VehicleType    = TechType.None;
                player.RightHandItemRotation = packet.CompressedRightHandItemRotation == -1 ? UpdatedProcessor.DefaultHandRotation : packet.CompressedRightHandItemRotation.ToQuaternion();
                player.LeftHandItemRotation  = packet.CompressedLeftHandItemRotation  == -1 ? UpdatedProcessor.DefaultHandRotation : packet.CompressedLeftHandItemRotation.ToQuaternion();
                player.SetHandItem(packet.ItemInHand);
                player.SetHandItemComponent(packet.HandItemComponent);
                player.SetCameraPitch(packet.CompressedCameraPitch.ToFloat());
                player.SetCameraForward(packet.CompressedCameraForward.ToVector3());
                player.SetSurfaceType(packet.SurfaceType);
            }

            return true;
        }

        /**
         *
         * Oyuncu verileri tetiklendikten sonra çalışır
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */

        public static void OnPlayerUpdated(PlayerUpdatedEventArgs ev)
        {
            if (World.IsLoaded && !global::Player.main.cinematicModeActive && !ZeroGame.IsPlayerPiloting())
            {
                ServerModel.PlayerUpdatedArgs request = new ServerModel.PlayerUpdatedArgs()
                {
                    CompressedPosition              = ev.Position.Compress(),
                    CompressedLocalPosition         = Network.Session.IsInSeaTruck ? ev.LocalPosition.Compress() : 0,
                    CompressedRotation              = ev.Rotation.Compress(),
                    CompressedCameraPitch           = ev.CameraPitch.ToShort(),
                    CompressedLeftHandItemRotation  = UpdatedProcessor.GetHandItemRotation(ev.TechTypeInHand, true),
                    CompressedRightHandItemRotation = UpdatedProcessor.GetHandItemRotation(ev.TechTypeInHand, false),
                    CompressedCameraForward         = ev.CameraForward.CompressToInt(),
                    ItemInHand  = ev.TechTypeInHand,
                    SurfaceType = ev.SurfaceType,

                    // NOT OPTIMIZED
                    Equipments        = ev.Equipments,
                    EmoteIndex        = (byte) ev.EmoteIndex,
                    IsPrecursorArm    = ev.IsPrecursorArm,
                    HandItemComponent = UpdatedProcessor.GetItemComponent(ev.TechTypeInHand),
                };

                NetworkClient.SendPacket(request);
            }
        }

        /**
         *
         * Eşya açısını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static long GetHandItemRotation(TechType techType, bool isLeft)
        {
            if (techType == TechType.None || techType == TechType.QuantumLocker || techType == TechType.Seaglide || techType == TechType.TeleportationTool)
            {
                return 0;
            }

            var localRotation = isLeft ? global::Player.main.armsController.leftAim.aimer.solver.bones[0].transform.localRotation : global::Player.main.armsController.rightAim.aimer.solver.bones[0].transform.localRotation;
            if (localRotation == UpdatedProcessor.DefaultHandRotation)
            {
                return -1;
            }

            return localRotation.Compress();
        }

        /**
         *
         * Eşya bileşenini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static NetworkPlayerItemComponent GetItemComponent(TechType techType)
        {
            if (techType == TechType.Flashlight)
            {
                var tool = global::Player.main.guiHand.GetTool() as global::FlashLight;
                if (tool == null)
                {
                    return null;
                }

                return new ItemModel.FlashLight()
                {
                    IsActivated = tool.toggleLights.lightsActive 
                };
            }
            else if (techType == TechType.Seaglide)
            {
                var tool = global::Player.main.guiHand.GetTool() as global::Seaglide;
                if (tool == null)
                {
                    return null;
                }

                return new ItemModel.Seaglide()
                {
                    IsActivated       = tool.activeState,
                    IsLightsActivated = tool.toggleLights.lightsActive,
                };
            }
            else if (techType == TechType.Flare)
            {
                var tool = global::Player.main.guiHand.GetTool() as global::Flare;
                if (tool == null)
                {
                    return null;
                }

                return new ItemModel.Flare()
                {
                    Intensity    = tool.light.intensity,
                    Range        = tool.light.range,
                    Energy       = tool.energyLeft < 3 ? 2f : tool.energyLeft,
                };
            }
            else if (techType == TechType.Welder)
            {
                var tool = global::Player.main.guiHand.GetTool() as global::Welder;
                if (tool == null)
                {
                    return null;
                }

                return new ItemModel.Welder()
                {
                    IsActivated = tool.fxIsPlaying,
                };
            }
            else if (techType == TechType.LaserCutter)
            {
                var tool = global::Player.main.guiHand.GetTool() as global::LaserCutter;
                if (tool == null)
                {
                    return null;
                }

                return new ItemModel.LaserCutter()
                {
                    IsPlaying = tool.fxIsPlaying,
                };
            }
            else if (techType == TechType.MetalDetector)
            {
                var tool = global::Player.main.guiHand.GetTool() as global::MetalDetector;
                if (tool == null)
                {
                    return null;
                }

                return new ItemModel.MetalDetector()
                {
                    TechTypeIndex = tool.targetTechTypeIndex == -1 || tool.targetTechTypeIndex > tool.detectableTechTypes.Count ? TechType.None : tool.detectableTechTypes[tool.targetTechTypeIndex],
                    IsUsing       = tool.energyMixin.charge > 0.0,
                    Wiggle        = tool.animator.GetFloat(MetalDetector.animWiggle),
                    ScreenState   = tool.screenState,
                };
            }
            else if (techType == TechType.AirBladder)
            {
                var tool = global::Player.main.guiHand.GetTool() as global::AirBladder;
                if (tool == null)
                {
                    return null;
                }

                return new ItemModel.AirBladder()
                {
                    Value = tool.animator.GetFloat(AirBladder.kAnimInflate),
                };
            }

            return null;
        }
    }
}
