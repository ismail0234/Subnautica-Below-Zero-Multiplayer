namespace Subnautica.Client.MonoBehaviours
{
    using Subnautica.API.Features;
    using Subnautica.Network.Structures;

    using UnityEngine;

    using Constructing = Subnautica.Client.Multiplayer.Constructing;

    public class BuilderGhostAnimation : MonoBehaviour
    {
        /**
         *
         * Builder sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Constructing.Builder Builder;

        /**
         *
         * En sonki hız değerleri
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Quaternion SmoothedRotationVelocity;

        /**
         *
         * StopwatchItem nesnesini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private StopwatchItem Timing = new StopwatchItem(BroadcastInterval.ConstructingGhostMoved);

        /**
         *
         * Her karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Update()
        {   
            if (this.Builder != null && this.Builder.IsActive && this.Builder.IsGhostModelAnimation)
            {
                if (this.Timing.IsFinished())
                {
                    this.Timing.Restart();

                    if (Time.time - this.Builder.UpdatedTime > 0.25f)
                    {
                        this.Builder.IsActive = false;
                        Constructing.Builder.Destroy(this.Builder.UniqueId);
                        return;
                    }

                    this.Builder.Update();
                }

                this.UpdatePosition();
            }
        }

        /**
         *
         * Pozisyonu günceller
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void UpdatePosition()
        {            
            Vector3 positionVelocity = Vector3.zero;

            this.Builder.GhostModel.transform.position = Vector3.SmoothDamp(this.Builder.GhostModel.transform.position, this.Builder.PlacePosition, ref positionVelocity, 0.05f);
            this.Builder.GhostModel.transform.rotation = BroadcastInterval.QuaternionSmoothDamp(this.Builder.GhostModel.transform.rotation, this.Builder.PlaceRotation, ref this.SmoothedRotationVelocity, 0.04f);
        }
    }
}
