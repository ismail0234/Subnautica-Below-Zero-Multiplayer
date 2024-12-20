namespace Subnautica.Client.Synchronizations.Processors.Building
{
    using System.Collections;

    using Subnautica.API.Enums;
    using Subnautica.API.Features;
    using Subnautica.Client.Abstracts;
    using Subnautica.Client.Core;
    using Subnautica.Events.EventArgs;
    using Subnautica.Network.Models.Core;

    using UnityEngine;

    using ServerModel = Subnautica.Network.Models.Server;

    public class FurnitureDeconstructionBeginProcessor : NormalProcessor
    {
        /**
         *
         * Event Blocker nesnesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool IsBlocked;

        /**
         *
         * Event Blocker nesnesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Coroutine CurrentCoroutine;

        /**
         *
         * Gelen veriyi işler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override bool OnDataReceived(NetworkPacket networkPacket)
        {
            var packet = networkPacket.GetPacket<ServerModel.FurnitureDeconstructionBeginArgs>();
            if (string.IsNullOrEmpty(packet.UniqueId))
            {
                return false;
            }

            FurnitureDeconstructionBeginProcessor.KillCoroutine(CurrentCoroutine);

            using (EventBlocker.Create(ProcessType.FurnitureDeconstructionBegin))
            {
                Multiplayer.Constructing.Builder.Deconstruct(packet.UniqueId, packet.Id, true);

                ConstructionSyncedProcessor.UpdateConstructionSync();
            }

            IsBlocked = false;
            return true;
        }

        /**
         *
         * Sınıf başlatılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public override void OnStart()
        {
            this.SetWaitingForNextFrame(true);
        }

        /**
         *
         * Mobilya inşaası ilk kaldırma işlemi olduğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnFurnitureDeconstructionBegin(FurnitureDeconstructionBeginEventArgs ev)
        {
            ev.IsAllowed = false;

            if (IsBlocked || Interact.IsBlocked(ev.UniqueId))
            {
                return;
            }

            if(!IsAllowedDeconstruction(ev.TechType, ev.UniqueId))
            {
                return;
            }

            IsBlocked = true;

            FurnitureDeconstructionBeginProcessor.KillCoroutine(CurrentCoroutine);

            CurrentCoroutine = UWE.CoroutineHost.StartCoroutine(ReleaseBlock());

            ServerModel.FurnitureDeconstructionBeginArgs request = new ServerModel.FurnitureDeconstructionBeginArgs()
            {
                UniqueId = ev.UniqueId,
            };

            NetworkClient.SendPacket(request);
        }

        /**
         *
         * Yıkılıp/Yıkılamayacağı kontrolü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsAllowedDeconstruction(TechType techType, string constructionId)
        {
            if (techType != TechType.Fabricator && techType != TechType.Workbench)
            {
                return true;
            }

            var gameObject = Network.Identifier.GetGameObject(constructionId);
            if (gameObject == null)
            {
                return true;
            }

            var crafter = gameObject.GetComponentInChildren<GhostCrafter>();
            if (crafter.opened)
            {
                return false;
            }

            return true;
        }

        /**
         *
         * Bloklanma durumunu serbest bırakır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static IEnumerator ReleaseBlock()
        {
            yield return new WaitForSecondsRealtime(0.5f);

            IsBlocked = false;
        }

        /**
         *
         * Akitf coroutine'yi öldürür.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void KillCoroutine(Coroutine coroutine)
        {
            if (coroutine != null)
            {
                UWE.CoroutineHost.StopCoroutine(coroutine);

                coroutine = null;
            }
        }
    }
}