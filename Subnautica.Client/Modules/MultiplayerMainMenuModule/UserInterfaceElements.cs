namespace Subnautica.Client.Modules.MultiplayerMainMenuModule
{
    using TMPro;

    using UnityEngine.Events;
    using UnityEngine;

    using Subnautica.API.Features;
    using Subnautica.API.Extensions;

    using UnityEngine.UI;

    public class UserInterfaceElements
    {

        /**
         *
         * Tek oyunculu buton kancasını ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SinglePlayerButtonAddEvent(UnityAction btnListener)
        {
            var btn = StartButtonPrefab.GetComponent<Button>();
            btn.onClick = new Button.ButtonClickedEvent();
            btn.onClick.AddListener(btnListener);
        }

        /**
         *
         * Sol menüye buton ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static GameObject CreateSidebarButton(string buttonText, UnityAction btnListener = null)
        {
            var btnGameObject = GameObject.Instantiate(StartButtonPrefab, StartButtonPrefab.transform.parent);
            btnGameObject.name = string.Format("CustomBtn_{0}", System.Guid.NewGuid().ToString());
            btnGameObject.transform.SetSiblingIndex(3);

            var button = btnGameObject.GetComponent<Button>();
            button.onClick = new Button.ButtonClickedEvent();
            button.onClick.AddListener(btnListener);

            var text = btnGameObject.transform.Find(GameIndex.MAIN_MENU_BUTTON_CIRCLE).GetComponent<TextMeshProUGUI>();
            text.SetText(buttonText, true);
            text.GetComponent<TranslationLiveUpdate>().translationKey = buttonText;

            return btnGameObject;
        }

        /**
         *
         * Multiplayer Ana İçerik Grubunu Oluşturur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static GameObject CreateMultiplayerBaseContent(string groupname, string headerText, string hostGameText, string joinGameText, UnityAction hostGameBtnListener, UnityAction joinGameBtnListener)
        {
            GameObject groupContent = GameObject.Instantiate(SavedGamesPrefab, MainMenuRightSide.main.transform);
            groupContent.name = groupname;

            TextMeshProUGUI header = groupContent.transform.Find(GameIndex.MAIN_MENU_HEADER_IN_GROUP).GetComponent<TextMeshProUGUI>();
            header.SetText(headerText, true);
            header.GetComponent<TranslationLiveUpdate>().translationKey = headerText;

            GameObject.Destroy(groupContent.GetComponent<MainMenuLoadPanel>());
            
            DestroyGameObject(groupContent, GameIndex.MAIN_MENU_BUTTON_IN_GROUP);

            CreateButtonInGroup(groupContent, 11f, 46f, hostGameText, hostGameBtnListener);
            CreateButtonInGroup(groupContent, 11f, 126f, joinGameText, joinGameBtnListener);
            return groupContent;
        }

        /**
         *
         * Multiplayer Host Game İçerik Grubunu Oluşturur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static GameObject CreateHostBaseContent(string groupname, string headerText, UnityAction createServerBtnListener)
        {
            GameObject groupContent = GameObject.Instantiate(SavedGamesPrefab, MainMenuRightSide.main.transform);
            groupContent.name = groupname;

            TextMeshProUGUI header = groupContent.transform.Find(GameIndex.MAIN_MENU_HEADER_IN_GROUP).GetComponent<TextMeshProUGUI>();
            header.SetText(headerText, true);
            header.GetComponent<TranslationLiveUpdate>().translationKey = headerText;

            DestroyGameObject(groupContent, GameIndex.MAIN_MENU_BUTTON_IN_GROUP);

            CreateButtonInGroup(groupContent, 11f, 46f, ZeroLanguage.Get("GAME_MULTIPLAYER_CREATE_SERVER"), createServerBtnListener);
            return groupContent;
        }

        /**
         *
         * Multiplayer Join Game İçerik Grubunu Oluşturur.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static GameObject CreateJoinBaseContent(string groupname, string headerText, string addServerHeaderText, UnityAction addServerBtnListener)
        {
            GameObject groupContent = GameObject.Instantiate(SavedGamesPrefab, MainMenuRightSide.main.transform);
            groupContent.name = groupname;

            TextMeshProUGUI header = groupContent.transform.Find(GameIndex.MAIN_MENU_HEADER_IN_GROUP).GetComponent<TextMeshProUGUI>();
            header.SetText(headerText, true);
            header.GetComponent<TranslationLiveUpdate>().translationKey = headerText;

            var loadPanel = groupContent.GetComponent<MainMenuLoadPanel>();
            loadPanel.transform.localPosition = new Vector3(loadPanel.transform.localPosition.x, loadPanel.transform.localPosition.y, loadPanel.transform.localPosition.z);

            DestroyGameObject(groupContent, GameIndex.MAIN_MENU_BUTTON_IN_GROUP);

            CreateInviteCodeGroup(groupContent, addServerBtnListener);

            // Add Server Button - OLD (for Hamaci/Radmin/Port Forward)
            // CreateButtonInGroup(groupContent, 11f, 46f, addServerHeaderText, addServerBtnListener);
            return groupContent;
        }

        /**
         *
         * Çok oyunculu sunucu ekle sayfa içeriğini ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static GameObject CreateInviteCodeGroup(GameObject groupContent, UnityAction serverAddListener)
        {
            TextMeshProUGUI header = groupContent.transform.Find("Header").GetComponent<TextMeshProUGUI>();
            header.SetText(ZeroLanguage.Get("GAME_MULTIPLAYER_JOIN_GAME"), true);
            header.GetComponent<TranslationLiveUpdate>().translationKey = ZeroLanguage.Get("GAME_MULTIPLAYER_JOIN_GAME");

            GameObject.Destroy(groupContent.GetComponent<MainMenuLoadPanel>());
            DestroyGameObject(groupContent, GameIndex.MAIN_MENU_BUTTON_IN_GROUP);

            Vector3 currentPosition = groupContent.transform.Find("Header").transform.position;
            currentPosition.x -= 0.022f;
            currentPosition.y -= 0.05f;

            // Sayfa içeriği
            Vector3 inviteCodePosition = currentPosition;

            AddInputField(groupContent, inviteCodePosition, ZeroLanguage.Get("GAME_INVITE_CODE_OR_IP"), ZeroLanguage.Get("GAME_INVITE_CODE_PLACEHOLDER"), "GAME_INVITE_CODE");

            GameObject btnGameObject = GameObject.Instantiate(SmallButtonPrefab, groupContent.transform);
            btnGameObject.name = string.Format("CustomBtn_{0}", System.Guid.NewGuid().ToString());
            btnGameObject.transform.position   = new Vector3(inviteCodePosition.x + 0.495f, inviteCodePosition.y - 0.17f, inviteCodePosition.z);
            btnGameObject.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

            Button button = btnGameObject.transform.Find("ButtonBack").GetComponent<Button>();
            button.onClick = new Button.ButtonClickedEvent();
            button.onClick.AddListener(serverAddListener);

            TextMeshProUGUI text = button.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            text.SetText(ZeroLanguage.Get("GAME_MULTIPLAYER_JOIN_GAME"), true);
            text.GetComponent<TranslationLiveUpdate>().translationKey = ZeroLanguage.Get("GAME_MULTIPLAYER_JOIN_GAME");

            DestroyGameObject(btnGameObject, "ButtonApply");
            DestroyGameObject(btnGameObject, "EmptySpace");
            return groupContent;
        }

        /**
         *
         * Çok oyunculu sunucu ekle sayfa içeriğini ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static GameObject CreateAddServerGroup(string groupname, UnityAction serverAddListener)
        {
            GameObject groupContent = GameObject.Instantiate(SavedGamesPrefab, MainMenuRightSide.main.transform);
            groupContent.name = groupname;

            TextMeshProUGUI header = groupContent.transform.Find("Header").GetComponent<TextMeshProUGUI>();
            header.SetText(ZeroLanguage.Get("GAME_ADD_SERVER"), true);
            header.GetComponent<TranslationLiveUpdate>().translationKey = ZeroLanguage.Get("GAME_ADD_SERVER");

            GameObject.Destroy(groupContent.GetComponent<MainMenuLoadPanel>());
            DestroyGameObject(groupContent, GameIndex.MAIN_MENU_BUTTON_IN_GROUP);

            Vector3 currentPosition = groupContent.transform.Find("Header").transform.position;
            currentPosition.x -= 0.022f;
            currentPosition.y -= 0.05f;

            // Sayfa içeriği
            Vector3 serverNamePosition = currentPosition;
            Vector3 serverIpPosition   = currentPosition;

            serverIpPosition.y   -= 0.12f;

            AddInputField(groupContent, serverNamePosition, ZeroLanguage.Get("GAME_SERVER_NAME"), ZeroLanguage.Get("GAME_SERVER_NAME_PLACEHOLDER"), "GAME_SERVER_NAME");
            AddInputField(groupContent, serverIpPosition, ZeroLanguage.Get("GAME_SERVER_IP"), ZeroLanguage.Get("GAME_SERVER_IP_PLACEHOLDER"), "GAME_SERVER_IP");

            GameObject btnGameObject = GameObject.Instantiate(SmallButtonPrefab, groupContent.transform);
            btnGameObject.name = string.Format("CustomBtn_{0}", System.Guid.NewGuid().ToString());
            btnGameObject.transform.position = new Vector3(serverIpPosition.x + 0.495f, serverIpPosition.y - 0.17f, serverIpPosition.z);
            btnGameObject.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

            Button button = btnGameObject.transform.Find("ButtonBack").GetComponent<Button>();
            button.onClick = new Button.ButtonClickedEvent();
            button.onClick.AddListener(serverAddListener);

            TextMeshProUGUI text = button.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            text.SetText(ZeroLanguage.Get("GAME_ADD_SERVER"), true);
            text.GetComponent<TranslationLiveUpdate>().translationKey = ZeroLanguage.Get("GAME_ADD_SERVER");

            DestroyGameObject(btnGameObject, "ButtonApply");
            DestroyGameObject(btnGameObject, "EmptySpace");
            return groupContent;
        }

        /**
         *
         * Çok oyunculu sunucu ekle sayfa içeriğini ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static GameObject CreateServerHostGroup(string groupname, UnityAction<GameModePresetId> serverCreateListener)
        {
            GameObject groupContent = GameObject.Instantiate(NewGamePrefab, MainMenuRightSide.main.transform);
            groupContent.name = groupname;

            TextMeshProUGUI header = groupContent.transform.Find("Header").GetComponent<TextMeshProUGUI>();
            header.SetText(ZeroLanguage.Get("GAME_MULTIPLAYER_CREATE_SERVER"), true);
            header.GetComponent<TranslationLiveUpdate>().translationKey = ZeroLanguage.Get("GAME_MULTIPLAYER_CREATE_SERVER");

            foreach (Transform item in groupContent.transform.Find("NewGameOptions/Viewport/Content"))
            {
                var gameModeId = item.gameObject.GetButtonGameModeId();
                if (gameModeId == GameModePresetId.Survival || gameModeId == GameModePresetId.Freedom || gameModeId == GameModePresetId.Creative)
                {
                    Button button = item.transform.GetComponent<Button>();
                    button.onClick = new Button.ButtonClickedEvent();
                    button.onClick.AddListener(() => { serverCreateListener(gameModeId); });
                }
                else
                {
                    UnityEngine.GameObject.Destroy(item.gameObject);
                }
            }

            return groupContent;
        }

        /**
         *
         * Grup içinde buton yaratır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static GameObject CreateButtonInGroup(GameObject groupContainer, float positionX, float positionY, string buttonText, UnityAction btnListener)
        {
            GameObject container = GameObject.Instantiate(GroupButtonPrefab, groupContainer.transform);
            container.transform.localPosition = new Vector3(container.transform.localPosition.x + positionX, container.transform.localPosition.y - positionY, container.transform.localPosition.z);

            Button button = container.GetComponent<Button>();
            button.onClick = new Button.ButtonClickedEvent();
            button.onClick.AddListener(btnListener);

            TextMeshProUGUI text = container.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            text.SetText(buttonText, true);
            text.GetComponent<TranslationLiveUpdate>().translationKey = buttonText;

            return container;
        }

        /**
         *
         * Input Field Nesnesi Ekler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void AddInputField(GameObject parentLayer, Vector3 position, string headerText, string placeHolderName, string inputName)
        {
            GameObject container = GameObject.Instantiate(InputBoxPrefab, parentLayer.transform);
            container.name = inputName;
            container.transform.position = new Vector3(position.x, position.y, position.z);

            TextMeshProUGUI header = container.transform.Find("HeaderText").GetComponent<TextMeshProUGUI>();
            header.SetText(headerText, true);
            header.GetComponent<TranslationLiveUpdate>().translationKey = headerText;
            header.fontSize *= 0.75f;

            TMP_InputField inputField = container.transform.Find("InputField").GetComponent<TMP_InputField>();
            inputField.text = "";

            TextMeshProUGUI placeholder = inputField.placeholder.GetComponent<TextMeshProUGUI>();
            placeholder.SetText(placeHolderName, true);
            placeholder.GetComponent<TranslationLiveUpdate>().translationKey = placeHolderName;

            DestroyGameObject(container, "Subscribe");
            DestroyGameObject(container, "ViewPastUpdates");

            GameObject subscription = container.transform.Find("SubscriptionError").gameObject;
            subscription.transform.localPosition = new Vector3(subscription.transform.localPosition.x + 200f, subscription.transform.localPosition.y - 200f, subscription.transform.localPosition.z);
        }

        /**
         *
         * Input metnini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string GetInputText(string inputName)
        {
            return GameObject.Find(inputName).transform.Find("InputField").GetComponent<TMP_InputField>().text.ToString();
        }

        /**
         *
         * Input metnini temizler.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static string ClearInputText(string inputName)
        {
            return GameObject.Find(inputName).transform.Find("InputField").GetComponent<TMP_InputField>().text = "";
        }

        /**
         *
         * Oyun Nesnesini yok eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static void DestroyGameObject(GameObject gameObject, string key)
        {
            UnityEngine.GameObject.Destroy(gameObject.transform.Find(key).gameObject);
        }

        /**
         *
         * Input detayına hata mesajı yazar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void SetInputErrorMessage(string inputName, string errorMessage)
        {
            GameObject gameObject = GameObject.Find(inputName);
            gameObject.transform.Find("SubscriptionError/Text").GetComponent<TextMeshProUGUI>().text = errorMessage;
            gameObject.transform.Find("SubscriptionError/Text").GetComponent<TranslationLiveUpdate>().translationKey = errorMessage;

            GameObject container = gameObject.transform.Find("SubscriptionError").gameObject;
            container.SetActive(true);
        }

        /**
         *
         *  Buton Nesnesi
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static GameObject StartButtonPrefab
        {
            get
            {
                if (_StartButtonPrefab == null)
                {
                    _StartButtonPrefab = GameObject.Find(GameIndex.MAIN_MENU_BUTTON);
                }

                return _StartButtonPrefab;
            }
        }

        /**
         *
         * Kayıtlı oyunlar Şeması 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static GameObject SavedGamesPrefab
        {
            get
            {
                if (_SavedGamesPrefab == null)
                {
                    _SavedGamesPrefab = MainMenuRightSide.main.gameObject.transform.Find("SavedGames").gameObject;
                }

                return _SavedGamesPrefab;
            }
        }

        /**
         *
         * Yeni Oyun Şeması 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static GameObject NewGamePrefab
        {
            get
            {
                if (_NewGamePrefab == null)
                {
                    _NewGamePrefab = MainMenuRightSide.main.gameObject.transform.Find("NewGame").gameObject;
                }

                return _NewGamePrefab;
            }
        }
                
        /**
         *
         * Group İçindeki Buton Şeması 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static GameObject GroupButtonPrefab
        {
            get
            {
                if (_GroupButtonPrefab == null)
                {
                    _GroupButtonPrefab = MainMenuRightSide.main.gameObject.transform.Find(string.Format("SavedGames/{0}", GameIndex.MAIN_MENU_BUTTON_IN_GROUP)).gameObject;
                }

                return _GroupButtonPrefab;
            }
        }

        /**
         *
         * InputBox Şeması 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static GameObject InputBoxPrefab
        {
            get
            {
                if (_InputBoxPrefab == null)
                {
                    _InputBoxPrefab = GameObject.Find(GameIndex.MAIN_MENU_INPUTBOX);
                }

                return _InputBoxPrefab;
            }
        }

        /**
         *
         * SmallButton Şeması 
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static GameObject SmallButtonPrefab
        {
            get
            {
                if (_SmallButtonPrefab == null)
                {
                    _SmallButtonPrefab = GameObject.Find(GameIndex.MAIN_MENU_SMALL_BUTTON);
                }

                return _SmallButtonPrefab;
            }
        }

        /**
         *
         * Tek oyunculu bölümü aktif mi?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsSinglePlayerMenuActive
        {
            get
            {
                return SavedGamesPrefab.activeSelf;
            }
        }

        /**
         *
         * Host
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static bool IsHostGroupActive
        {
            get
            {
                GameObject gameObject = GameObject.Find(MultiplayerMainMenu.MULTIPLAYER_HOST_GROUP_NAME);
                if(gameObject == null)
                {
                    return false;
                }

                return gameObject.activeSelf;
            }
        }

        /**
         *
         * İç Nesneler
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static GameObject _StartButtonPrefab;
        private static GameObject _SavedGamesPrefab;
        private static GameObject _GroupButtonPrefab;
        private static GameObject _InputBoxPrefab;
        private static GameObject _SmallButtonPrefab;
        private static GameObject _NewGamePrefab;
    }
}