namespace Subnautica.Client.Synchronizations.Processors.WorldEntities
{
    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts.Processors;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Core.Components;

    using UnityEngine;

    using EntityModel = Subnautica.Network.Models.WorldEntity;
    using ServerModel = Subnautica.Network.Models.Server;

    public class ElevatorProcessor : WorldEntityProcessor
    {
        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkWorldEntityComponent packet, byte requesterId, bool isSpawning)
        {
            var entity = packet.GetComponent<EntityModel.Elevator>();
            if (entity.UniqueId.IsNull())
            {
                return false;
            }

            if (!isSpawning)
            {
                Network.StaticEntity.AddStaticEntity(entity);
            }

            this.ElevatorMove(entity.UniqueId, entity.IsUp, entity.StartTime);
            return true;
        }

        /**
         *
         * Asansörü hareket ettirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool ElevatorMove(string uniqueId, bool isUp, float startTime)
        {
            var elevator = Network.Identifier.GetComponentByGameObject<global::Rocket>(uniqueId, true);
            if (elevator == null)
            {
                return false;
            }

            var num1 = 0.0f;
            if (isUp)
            {
                num1 = 1f;
            }

            elevator.elevatorPosition = isUp ? 0.0f : 1f;
            elevator.elevatorState    = isUp ? Rocket.RocketElevatorStates.Up : Rocket.RocketElevatorStates.Down;

            var diffTime = Mathf.Abs(DayNightCycle.main.timePassedAsFloat - startTime);
            if (diffTime < 1f)
            {
                elevator.useLiftSFX.Play();
            }

            if (diffTime > 9f)
            {
                elevator.elevatorPosition = num1;
            }
            else
            {
                while (startTime < DayNightCycle.main.timePassedAsFloat)
                {
                    var deltaTime = Time.deltaTime;
                    if (deltaTime == 0f)
                    {
                        deltaTime = 0.01f;
                    }

                    var num2 = elevator.elevatorDampening.Evaluate(elevator.elevatorPosition);

                    elevator.elevatorPosition = Mathf.MoveTowards(elevator.elevatorPosition, num1, deltaTime / num2);

                    startTime += deltaTime;
                }
            }

            elevator.Update();
            return true;
        }

        /**
         *
         * Asansöre tıklandığında tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnElevatorCalling(ElevatorCallingEventArgs ev)
        {
            ev.IsAllowed = false;

            ServerModel.WorldEntityActionArgs request = new ServerModel.WorldEntityActionArgs()
            {
                Entity = new EntityModel.Elevator()
                {
                    UniqueId = ev.UniqueId,
                    IsUp     = ev.IsUp,
                },
            };

            NetworkClient.SendPacket(request);
        }
    }
}