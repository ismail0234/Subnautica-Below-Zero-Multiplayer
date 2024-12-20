namespace Subnautica.API.Features.NetworkUtility
{
    using System.Collections.Generic;

    using Subnautica.API.Extensions;

    public class HandTarget
    {
        /**
         *
         * Hedefleri barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Dictionary<string, float> Targets = new Dictionary<string, float>();

        /**
         *
         * Gecikmeyi barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private float Delay { get; set; } = 0.5f;

        /**
         *
         * Hedef nesnenin kullanımda olup olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsBlocked(string uniqueId)
        {
            if (uniqueId.IsNull())
            {
                return false;
            }

            if (this.IsUsingFromCache(uniqueId))
            {
                return true;
            }

            if (Interact.IsBlocked(uniqueId))
            {
                this.AddTemporaryBlock(uniqueId);
                return true;
            }

            if (this.IsUsingFromCinematics(uniqueId))
            {
                this.AddTemporaryBlock(uniqueId);
                return true;
            }

            return false;
        }

        /**
          *
          * Cinematic üzerinden kullanım durumunu döner.
          *
          * @author Ismail <ismaiil_0234@hotmail.com>
          *
          */
        private bool IsUsingFromCinematics(string uniqueId)
        {
            foreach (var player in ZeroPlayer.GetPlayers())
            {
                if (player.IsCinematicModeActive && player.CurrentCinematicUniqueId == uniqueId)
                {
                    return true;
                }

                if (player.UsingRoomId == uniqueId)
                {
                    return true;
                }
            }

            return false;
        }

        /**
         *
         * Önbellekten kullanım durumunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool IsUsingFromCache(string uniqueId)
        {
            return this.Targets.TryGetValue(uniqueId, out var target) && Network.Session.GetWorldTime() < target;
        }
 
        /**
         *
         * Hedef nesnenin kullanımda olup olmadığına bakar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void AddTemporaryBlock(string uniqueId)
        {
            this.Targets[uniqueId] = Network.Session.GetWorldTime() + this.Delay;
        }

        /**
         *
         * Verileri temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Dispose()
        {
            this.Targets.Clear();
        }
    }
}
