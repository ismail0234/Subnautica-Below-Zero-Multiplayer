namespace Subnautica.Client.Modules
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Subnautica.API.Features;
    using Subnautica.Events.EventArgs;

    using TMPro;

    using UnityEngine;
    using UnityEngine.UI;

    public static class InviteCodeModule
    {
        /**
         *
         * Eklenti aktifleştiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnPluginEnabled()
        {
            Application.wantsToQuit += InviteCodeModule.OnWantsToQuit;

            ConnectToNetwork();
        }

        /**
         *
         * Sahne yüklendiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnSceneLoaded(SceneLoadedEventArgs ev)
        {
            if (ev.Scene.name == "XMenu")
            {
                ConnectToNetwork();
            }
        }

        /**
         *
         * Ağa bağlanır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void ConnectToNetwork()
        {
            Network.InviteCode.SetInviteCode(null);
            Network.InviteCode.SetAccessToken(null);

            if (!NetBirdApi.Instance.IsConnectingToNetwork())
            {
                Task.Run(() =>
                {
                    if (!NetBirdApi.Instance.IsConnectingToNetwork())
                    {
                        NetBirdApi.Instance.Refresh();

                        if (!NetBirdApi.Instance.IsReady())
                        {
                            NetBirdApi.Instance.Connect();
                        }
                    }
                }).ContinueWith((t) => {
                    if (t.IsFaulted)
                    {
                        Log.Error($"ConnectToNetwork Ex: {t.Exception}");
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        /**
         *
         * Oyuncu oyundan çıkarken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool OnWantsToQuit()
        {
            try
            {
                NetBirdApi.Instance.Disconnect();
            }
            catch (Exception)
            {

            }

            return true;
        }

        /**
         *
         * Oyuncu ana menüye gittiğinde tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnQuittingToMainMenu(QuittingToMainMenuEventArgs ev)
        {
            if (Network.IsHost)
            {
                UWE.CoroutineHost.StartCoroutine(Network.InviteCode.LeaveFromServerAsync(NetBirdApi.Instance.GetPeerIp()));
            }
        }

        /**
         *
         * Oyun içi menü açılırken tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void OnInGameMenuOpened(InGameMenuOpenedEventArgs ev)
        {
            if (Network.IsMultiplayerActive && Network.IsHost)
            {
                var feedbackBtn = IngameMenu.main.transform.Find("Main/ButtonLayout/ButtonFeedback").gameObject;
                if (feedbackBtn.activeSelf)
                {
                    CreateInviteCodeButtons();

                    IngameMenu.main.feedbackButton.gameObject.SetActive(false);
                    IngameMenu.main.transform.Find("Main/ButtonLayout/ButtonFeedback").gameObject.SetActive(false);
                }

                foreach (var item in IngameMenu.main.helpButton.transform.parent.gameObject.GetComponentsInChildren<Button>())
                {
                    if (item.name.Contains("InviteCodeTextButton"))
                    {
                        item.GetComponentInChildren<TextMeshProUGUI>().text = string.Empty;
                    }
                }
            }
        }

        /**
         *
         * Invite code butonunu oluşturur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void CreateInviteCodeButtons()
        {
            var inviceCodeButtonShow = GameObject.Instantiate(IngameMenu.main.helpButton.gameObject, IngameMenu.main.helpButton.transform.parent);
            var inviceCodeButtonText = GameObject.Instantiate(IngameMenu.main.helpButton.gameObject, IngameMenu.main.helpButton.transform.parent);
            inviceCodeButtonShow.gameObject.name = "InviteCodeShowButton";
            inviceCodeButtonText.gameObject.name = "InviteCodeTextButton";

            inviceCodeButtonShow.SetActive(true);
            inviceCodeButtonText.SetActive(true);

            inviceCodeButtonShow.GetComponentInChildren<TextMeshProUGUI>().text = ZeroLanguage.Get("GAME_SHOW_INVITE_CODE");
            inviceCodeButtonText.GetComponentInChildren<TextMeshProUGUI>().text = "";

            inviceCodeButtonText.GetComponent<RectTransform>().SetAsFirstSibling();
            inviceCodeButtonShow.GetComponent<RectTransform>().SetAsFirstSibling();

            if (inviceCodeButtonText.TryGetComponent<Button>(out var textBtn))
            {
                textBtn.enabled = false;
                textBtn.GetComponent<Image>().enabled = false;
            }

            if (inviceCodeButtonShow.TryGetComponent<Button>(out var showBtn))
            {
                showBtn.onClick = new Button.ButtonClickedEvent();
                showBtn.onClick.AddListener(() => {
                    inviceCodeButtonText.GetComponentInChildren<TextMeshProUGUI>().text = Network.InviteCode.GetInviteCode();
                });
            }
        }
    }
}
