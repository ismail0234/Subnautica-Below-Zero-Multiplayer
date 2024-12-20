namespace Subnautica.Client.MonoBehaviours.Player
{
    using System.Collections.Generic;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;
    using Subnautica.API.Features.Helper;

    using UnityEngine;

    public class PlayerHandItemManager : MonoBehaviour
    {
        /**
         *
         * Oyuncu Sınıfı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroPlayer Player { get; set; }

        /**
         *
         * Elindeki Nesne Türü
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private TechType ActiveTechType { get; set; } = TechType.None;

        /**
         *
         * Elindeki Nesne Adı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private string ActiveToolName { get; set; } = null;

        /**
         *
         * Nesne Havuzu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private Dictionary<TechType, Pickupable> ItemPool { get; set; } = new Dictionary<TechType, Pickupable>();

        /**
         *
         * LoadingItems Havuzu
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private List<TechType> LoadingItems { get; set; } = new List<TechType>();

        /**
         *
         * DefaultMask Index Numarası
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private int DefaultMaskIndex { get; set; } = -1;

        /**
         *
         * ViewMask Index Numarası
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private int ViewMaskIndex { get; set; } = -1;

        /**
         *
         * QueueAction Sınıfı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private ItemQueueAction QueueAction { get; set; }
        /**
         *
         * Sınıf başlatılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Awake()
        {
            this.DefaultMaskIndex = LayerMask.NameToLayer("default");
            this.ViewMaskIndex    = LayerMask.NameToLayer("Viewmodel");

            this.QueueAction = new ItemQueueAction();
            this.QueueAction.OnEntitySpawned = this.OnEntitySpawned;
        }

        /**
         *
         * Eldeki eşyayı kaldırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void ClearHand()
        {
            this.SetHand(TechType.None);
        }

        /**
         *
         * Eline bir eşya verir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool SetHand(TechType techType)
        {
            if (this.ActiveTechType == techType)
            {
                return true;
            }

            if (this.ActiveToolName.IsNotNull())
            {
                SafeAnimator.SetBool(this.Player.Animator, string.Format("holding_{0}", this.ActiveToolName), false);
            }

            if (this.ActiveTechType != TechType.None && this.ItemPool.ContainsKey(this.ActiveTechType))
            {
                this.HolsterItem(this.GetItem(this.ActiveTechType));
            }

            if (techType == TechType.PDA)
            {
                this.OpenPda();
                this.OnChangedItem(TechType.PDA);
            }
            else if (this.ActiveTechType == TechType.PDA)
            {
                this.ClosePda();
            }

            if (techType == TechType.None)
            {
                if (this.ActiveTechType == TechType.None)
                {
                    return false;
                }

                this.ActiveToolName = null;
                this.OnChangedItem(TechType.None);
                return true;
            }

            if (techType == TechType.PDA)
            {
                return true;
            }

            if (this.ItemPool.ContainsKey(techType))
            {
                return this.DrawItem(this.GetItem(techType));
            }

            return this.CreateItem(techType);
        }

        /**
         *
         * Havuzdan Eşya nesnesini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Pickupable GetItem(TechType techType)
        {
            if (this.ItemPool.TryGetValue(techType, out var pickupable))
            {
                return pickupable;
            }

            return null;
        }

        /**
         *
         * Eldeki eşyayı gösterir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool DrawItem(Pickupable pickupable)
        {
            if (pickupable == null)
            {
                return false;
            }

            var tool = pickupable.GetComponent<PlayerTool>();
            if (tool == null)
            {
                return false;
            }

            this.OnChangedItem(pickupable.GetTechType());
            this.ActiveToolName = tool.animToolName;

            ModelPlug.PlugIntoSocket(tool, this.Player.RightHandItemTransform);

            Utils.SetLayerRecursively(tool.gameObject, this.ViewMaskIndex);

            foreach (Animator componentsInChild in tool.GetComponentsInChildren<Animator>())
            {
                componentsInChild.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            }

            if (tool.mainCollider != null)
            {
                tool.mainCollider.enabled = false;
            }
            
            UWE.Utils.SetIsKinematicAndUpdateInterpolation(tool.GetComponent<Rigidbody>(), true);

            pickupable.DisableColliders();

            tool.gameObject.SetActive(true);

            SafeAnimator.SetBool(this.Player.Animator, string.Format("holding_{0}", this.ActiveToolName), true);

            this.SendEquipmentEvent(pickupable, true);

            this.Player.GetComponent<PlayerLighting>().ApplySkybox();
            return true;
        }

        /**
         *
         * Eldeki eşyayı gizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool HolsterItem(Pickupable pickupable)
        {
            var tool = pickupable.GetComponent<PlayerTool>();
            if (tool == null)
            {
                return false;
            }

            tool.gameObject.SetActive(false);

            Utils.SetLayerRecursively(tool.gameObject, this.DefaultMaskIndex);

            if (tool.mainCollider != null)
            {
                tool.mainCollider.enabled = true;
            }

            UWE.Utils.SetIsKinematicAndUpdateInterpolation(tool.GetComponent<Rigidbody>(), false);

            foreach (Animator componentsInChild in tool.GetComponentsInChildren<Animator>())
            {
                componentsInChild.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
            }

            tool.OnHolster();

            this.SendEquipmentEvent(pickupable, false);
            return true;
        }

        /**
         *
         * Bir eşya oluşturur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool CreateItem(TechType techType)
        {
            if (this.LoadingItems.Contains(techType))
            {
                return true;
            }

            this.LoadingItems.Add(techType);

            Entity.SpawnToQueue(techType, Network.Identifier.GenerateUniqueId(), this.QueueAction);
            return true;
        }

        /**
         *
         * Prefab oluştuğunda tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnEntitySpawned(ItemQueueProcess item, Pickupable pickupable, GameObject gameObject)
        {
            if (item.TechType == TechType.SnowBall && pickupable.TryGetComponent<SnowBall>(out var snowBall))
            {
                snowBall.despawnTime = Time.time + (86400 * 7);
            }

            this.LoadingItems.Remove(item.TechType);
            this.ItemPool.Add(item.TechType, pickupable);

            this.SetHand(item.TechType);
        }

        /**
         *
         * Sol eldeki PDA'yı aktif eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void OpenPda()
        {
            SafeAnimator.SetBool(this.Player.Animator, "using_pda", true);
            this.Player.LeftHandItemTransform.gameObject.SetActive(true);
        }

        /**
         *
         * Sol eldeki PDA'yı pasif yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void ClosePda()
        {
            SafeAnimator.SetBool(this.Player.Animator, "using_pda", false);
            this.Player.LeftHandItemTransform.gameObject.SetActive(false);
        }

        /**
         *
         * Eldeki eşya değiştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void OnChangedItem(TechType techType)
        {
            this.ActiveTechType = techType;
            this.Player.TechTypeInHand = techType;
        }

        /**
         *
         * Ekipman olaylarını tetikler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SendEquipmentEvent(Pickupable pickupable, bool status)
        {
           if (pickupable.gameObject.TryGetComponent(out FPModel fpModel))
            {
                fpModel.SetState(status);
            }
        }

        /**
         *
         * Havuz'dan ilk eklenen nesneyi siler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnDestroy()
        {
            foreach (var item in this.ItemPool)
            {
                if (item.Value?.gameObject == null)
                {
                    continue;
                }

                GameObject.Destroy(item.Value.gameObject);
            }

            this.ItemPool.Clear();
            this.LoadingItems.Clear();
        }
    }
}
