namespace Subnautica.Client.MonoBehaviours.Entity
{
    using System.Collections.Generic;

    using Subnautica.API.Extensions;
    using Subnautica.API.Features;

    using UnityEngine;

    public class MultiplayerCrafter : MonoBehaviour
    {
        /**
         *
         * Crafter Sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public global::GhostCrafter Crafter { get; set; }

        /**
         *
         * İşlem sahibi miyim?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsMine { get; private set; }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Awake()
        {
            this.Initialize();
        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Initialize()
        {
            if (this.Crafter == null)
            {
                this.Crafter = this.gameObject.GetGhostCrafter();
                if (this.Crafter && this.Crafter.logic == null && this.Crafter.TryGetComponent<CrafterLogic>(out var logic))
                {
                    this.Crafter.logic = logic;
                }
            }
        }

        /**
         *
         * İşlemi sahibini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetIsMine(bool isMine)
        {
            this.IsMine = isMine;
        }

        /**
         *
         * Zanaatkarı açar
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Open()
        {
            using (EventBlocker.Create(TechType.Fabricator))
            using (EventBlocker.Create(TechType.Workbench))
            using (EventBlocker.Create(TechType.BaseUpgradeConsole))
            {
                this.Crafter.opened = true;

                if (this.IsMine)
                {
                    uGUI.main.craftingMenu.Open(this.Crafter.craftTree, this.Crafter);
                }
            }
        }

        /**
         *
         * Zanaatkarı kapatır
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Close()
        {
            using (EventBlocker.Create(TechType.Fabricator))
            using (EventBlocker.Create(TechType.Workbench))
            using (EventBlocker.Create(TechType.BaseUpgradeConsole))
            {
                if (this.Crafter.logic && this.Crafter.logic.inProgress == false)
                {
                    this.Crafter.opened = false;
                }
            }
        }

        /**
         *
         * Zanaatkar'dan üretilen nesneyi alır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void TryPickup()
        {
            using (EventBlocker.Create(TechType.Fabricator))
            using (EventBlocker.Create(TechType.Workbench))
            using (EventBlocker.Create(TechType.BaseUpgradeConsole))
            {
                this.Crafter.opened = true;

                if (this.IsMine)
                {
                    this.Crafter.logic.TryPickup();
                }
                else
                {
                    this.Crafter.state = false;
                    this.Crafter.logic.pickingUp = false;
                    this.Crafter.logic.ResetCrafter();
                }
            }
        }

        /**
         *
         * Zanaatkar'da nesne üretir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Craft(TechType techType, float startTime, float duration)
        {
            using (EventBlocker.Create(TechType.Fabricator))
            using (EventBlocker.Create(TechType.Workbench))
            using (EventBlocker.Create(TechType.BaseUpgradeConsole))
            {
                this.Crafter.opened = true;

                ZeroGame.Craft(this.Crafter, techType, startTime, duration, this.IsMine);
            }
        }

        /**
         *
         * Craft tamamlandığında bildirim gönderir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void OnCrafterEnded()
        {
            if (this.PlayerIsInRange(10f))
            {
                ErrorMessage.AddMessage(global::Language.main.GetFormat<string>("CraftingEnd", global::Language.main.Get(this.Crafter.logic.craftingTechType.AsString())));
            }
        }

        /**
         *
         * Tamamlanmış eşyayı alıp/alamayacağını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAllowedPickup(TechType techType, int amount)
        {
            List<Vector2int> sizes = new List<Vector2int>();

            for (int i = 0; i < amount; i++)
            {
                sizes.Add(TechData.GetItemSize(techType));
            }

            return global::Inventory.main._container.HasRoomFor(sizes);
        }

        /**
         *
         * Otomatik alma işlemi aktif mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsActiveAutoPickup()
        {
            if (this.IsMine && this.Crafter.logic && this.Crafter.logic.craftingTechType != TechType.None && this.PlayerIsInRange(this.Crafter.closeDistance))
            {
                if (this.IsAllowedPickup(this.Crafter.logic.craftingTechType, this.Crafter.logic.numCrafted))
                {
                    return true;
                }
            }

            return false;
        }

        private bool PlayerIsInRange(float distance)
        {
            return (global::Player.main.transform.position - this.Crafter.transform.position).sqrMagnitude < distance * distance;
        }
    }
}
