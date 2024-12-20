namespace Subnautica.Server.Extensions
{
    using Subnautica.Network.Structures;
    using Subnautica.Server.Core;

    public static class NetworkExtensions
    {        
        /**
         *
         * Hedef sahip id numarasını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static byte GetTargetOwnerId(this ZeroLastTarget lastTarget, byte ownerPlayerId)
        {
            if (lastTarget.IsCreature())
            {
                return ownerPlayerId;
            }
            else if (lastTarget.IsVehicle())
            {
                foreach (var player in Server.Instance.GetPlayers())
                {
                    if (player.VehicleId == lastTarget.TargetId)
                    {
                        return player.PlayerId;
                    }
                }
            }
            else if (lastTarget.IsPlayer())
            {
                foreach (var player in Server.Instance.GetPlayers())
                {
                    if (player.UniqueId == lastTarget.TargetId)
                    {
                        return player.PlayerId;
                    }
                }
            }

            return 0;
        }
    }
}
