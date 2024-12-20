namespace Subnautica.API.Features
{
    using Subnautica.API.Enums;

    using UnityEngine;

    public class ZeroModal
    {
        /**
         *
         * Varsayılan değerler önbelleğe alındı mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool isCached = false;

        /**
         *
         * Varsayılan değerler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Vector2 DefaultPanelSizeDelta       = new Vector2();
        private static Vector2 DefaultDescriptionAnchorMin = new Vector2();
        private static Vector2 DefaultDescriptionAnchorMax = new Vector2();
        private static Vector2 DefaultDescriptionOffsetMin = new Vector2();
        private static Vector2 DefaultDescriptionOffsetMax = new Vector2();
        private static Vector2 DefaultDescriptionSizeDelta = new Vector2();
        private static Vector2 DefaultOkButtonOffsetMin    = new Vector2();
        private static Vector2 DefaultOkButtonOffsetMax    = new Vector2();
        private static Vector2 DefaultYesButtonOffsetMin   = new Vector2();
        private static Vector2 DefaultYesButtonOffsetMax   = new Vector2();
        private static Vector2 DefaultNoButtonOffsetMin    = new Vector2();
        private static Vector2 DefaultNoButtonOffsetMax    = new Vector2();

        /**
         *
         * Modal gözükme işlemini yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void Show(string title, string content, ZeroModalSize size = ZeroModalSize.Default)
        {
            var errorMessage = string.Format("<size=25px>{0}</size><br>{1}", title, content);

            ZeroModal.Show(errorMessage);
            ZeroModal.ResizeModal(size);
        }

        /**
         *
         * Modal gözükme işlemini yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void Show(string errorMessage)
        {
            uGUI.main.confirmation.Show(errorMessage);
        }

        /**
         *
         * Modal gözükme işlemini yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool ResizeModal(ZeroModalSize size)
        {
            var item = ZeroModalSizeItem.Create(size);
            if (item == null)
            {
                return false;
            }

            if (uGUI.main.confirmation.panel.gameObject.TryGetComponent<RectTransform>(out var panelRectTransform))
            {
                panelRectTransform.sizeDelta = new Vector2(item.Width, item.Height);

                if (uGUI.main.confirmation.description.TryGetComponent<RectTransform>(out var descriptionRectTransform))
                {
                    descriptionRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    descriptionRectTransform.anchorMax = new Vector2(0.5f, 1f);
                    descriptionRectTransform.offsetMin = new Vector2(descriptionRectTransform.offsetMin.x, item.DescriptionMinHeight);
                    descriptionRectTransform.offsetMax = new Vector2(descriptionRectTransform.offsetMax.x, item.DescriptionMaxHeight);
                    descriptionRectTransform.sizeDelta = new Vector2(item.Width * 1.25f, item.Height);
                }

                if (uGUI.main.confirmation.ok.TryGetComponent<RectTransform>(out var okRectTransform))
                {
                    okRectTransform.offsetMin = new Vector2(-70f, -59f - item.ButtonPosition);
                    okRectTransform.offsetMax = new Vector2(70f, -3f - item.ButtonPosition);
                }

                if (uGUI.main.confirmation.yes.TryGetComponent<RectTransform>(out var yesRectTransform))
                {
                    yesRectTransform.offsetMin = new Vector2(-168f, -59f - item.ButtonPosition);
                    yesRectTransform.offsetMax = new Vector2(-28f, -3f - item.ButtonPosition);
                }

                if (uGUI.main.confirmation.no.TryGetComponent<RectTransform>(out var noRectTransform))
                {
                    noRectTransform.offsetMin = new Vector2(27f, -59f - item.ButtonPosition);
                    noRectTransform.offsetMax = new Vector2(167f, -3f - item.ButtonPosition);
                }
            }

            return true;
        }

        /**
         *
         * Varsayılan ayarları uygular.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool ApplyDefaultSettings()
        {
            ZeroModal.InitializeCache();

            if (isCached && uGUI.main.confirmation.panel.gameObject.TryGetComponent<RectTransform>(out var panelRectTransform))
            {
                panelRectTransform.sizeDelta = ZeroModal.DefaultPanelSizeDelta;

                if (uGUI.main.confirmation.description.TryGetComponent<RectTransform>(out var descriptionRectTransform))
                {
                    descriptionRectTransform.anchorMin = ZeroModal.DefaultDescriptionAnchorMin;
                    descriptionRectTransform.anchorMax = ZeroModal.DefaultDescriptionAnchorMax;
                    descriptionRectTransform.offsetMin = ZeroModal.DefaultDescriptionOffsetMin;
                    descriptionRectTransform.offsetMax = ZeroModal.DefaultDescriptionOffsetMax;
                    descriptionRectTransform.sizeDelta = ZeroModal.DefaultDescriptionSizeDelta;
                }

                if (uGUI.main.confirmation.ok.TryGetComponent<RectTransform>(out var okRectTransform))
                {
                    okRectTransform.offsetMin = ZeroModal.DefaultOkButtonOffsetMin;
                    okRectTransform.offsetMax = ZeroModal.DefaultOkButtonOffsetMax;
                }

                if (uGUI.main.confirmation.yes.TryGetComponent<RectTransform>(out var yesRectTransform))
                {
                    yesRectTransform.offsetMin = ZeroModal.DefaultYesButtonOffsetMin;
                    yesRectTransform.offsetMax = ZeroModal.DefaultYesButtonOffsetMax;
                }

                if (uGUI.main.confirmation.no.TryGetComponent<RectTransform>(out var noRectTransform))
                {
                    noRectTransform.offsetMin = ZeroModal.DefaultNoButtonOffsetMin;
                    noRectTransform.offsetMax = ZeroModal.DefaultNoButtonOffsetMax;
                }

                return true;
            }

            return false;
        }

        /**
         *
         * Verileri önbelleğe alır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static bool InitializeCache()
        {
            if (isCached)
            {
                return false;
            }

            isCached = true;

            if (uGUI.main.confirmation.panel.gameObject.TryGetComponent<RectTransform>(out var panelRectTransform))
            {
                ZeroModal.DefaultPanelSizeDelta = panelRectTransform.sizeDelta;

                if (uGUI.main.confirmation.description.TryGetComponent<RectTransform>(out var descriptionRectTransform))
                {
                    ZeroModal.DefaultDescriptionAnchorMin = descriptionRectTransform.anchorMin;
                    ZeroModal.DefaultDescriptionAnchorMax = descriptionRectTransform.anchorMax;
                    ZeroModal.DefaultDescriptionOffsetMin = descriptionRectTransform.offsetMin;
                    ZeroModal.DefaultDescriptionOffsetMax = descriptionRectTransform.offsetMax;
                    ZeroModal.DefaultDescriptionSizeDelta = descriptionRectTransform.sizeDelta;
                }

                if (uGUI.main.confirmation.ok.TryGetComponent<RectTransform>(out var okRectTransform))
                {
                    ZeroModal.DefaultOkButtonOffsetMin = okRectTransform.offsetMin;
                    ZeroModal.DefaultOkButtonOffsetMax = okRectTransform.offsetMax;
                }

                if (uGUI.main.confirmation.yes.TryGetComponent<RectTransform>(out var yesRectTransform))
                {
                    ZeroModal.DefaultYesButtonOffsetMin = yesRectTransform.offsetMin;
                    ZeroModal.DefaultYesButtonOffsetMax = yesRectTransform.offsetMax;
                }

                if (uGUI.main.confirmation.no.TryGetComponent<RectTransform>(out var noRectTransform))
                {
                    ZeroModal.DefaultNoButtonOffsetMin = noRectTransform.offsetMin;
                    ZeroModal.DefaultNoButtonOffsetMax = noRectTransform.offsetMax;
                }
            }

            return true;
        }
    }

    public class ZeroModalSizeItem
    {
        /**
         *
         * Genişliği barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float Width { get; private set; }

        /**
         *
         * Yüksekliği barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float Height { get; private set; }

        /**
         *
         * Buton pozisyonunu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float ButtonPosition
        {
            get
            {
                return this.Height - 75f;
            }
        }

        /**
         *
         * Metin Min yüksekliği barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float DescriptionMinHeight
        {
            get
            {
                return -(((this.Height / 100) * 50) + 50);
            }
        }

        /**
         *
         * Metin Max yüksekliği barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public float DescriptionMaxHeight
        {
            get
            {
                return this.DescriptionMinHeight + this.Height;
            }
        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public ZeroModalSizeItem(float width, float height)
        {
            this.Width  = width;
            this.Height = height;
        }

        /**
         *
         * Boyut oluşturma işlemini yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static ZeroModalSizeItem Create(ZeroModalSize size)
        {
            var str = size.ToString();
            if (str.Contains("Size_"))
            {
                var data = str.Replace("Size_", "").Split('x');
                return new ZeroModalSizeItem(float.Parse(data[0]), float.Parse(data[1]));
            }

            return null;
        }
    }
}
