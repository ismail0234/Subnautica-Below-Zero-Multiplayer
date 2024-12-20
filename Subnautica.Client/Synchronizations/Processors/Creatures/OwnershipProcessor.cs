namespace Subnautica.Client.Synchronizations.Processors.Creatures
{
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Network.Models.Core;

    using ServerModel = Subnautica.Network.Models.Server;

    public class OwnershipProcessor : NormalProcessor
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
            var packet = networkPacket.GetPacket<ServerModel.WorldCreatureOwnershipChangedArgs>();
            if (packet == null)
            {
                return false;
            }

            foreach (var creature in packet.Creatures)
            {
                if (creature.IsWaitingRegistation())
                {
                    Network.Creatures.RegisterCreature(creature);
                }

                if (creature.IsExistsOwnership())
                {
                    Network.Creatures.UpdateCreature(creature);

                    if (Network.Creatures.IsActiveCreature(creature.Id))
                    {
                        Network.Creatures.ChangeOwnershipToQueue(creature.Id);
                    }
                    else
                    {
                        Network.Creatures.SpawnToQueue(creature.Id);
                    }
                }
                else
                {
                    if (creature.IsPlayDeathAnimation())
                    {
                        Network.Creatures.DeathToQueue(creature.Id);
                    }
                    else
                    {
                        Network.Creatures.RemoveToQueue(creature.Id);
                    }
                }
            }

            return true;
        }
    }
}
