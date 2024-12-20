namespace Subnautica.API.Features
{
    public class GameIndex
    {
        /**
         *
         * Oyuncu gövde anahtarları
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const string PLAYER_MODEL_BODY_DEFAULT         = "female_geo/base/female_base_body_geo";
        public const string PLAYER_MODEL_BODY_COLD_SUIT       = "female_geo/coldProtective/female_coldProtectiveSuit_body_geo";
        public const string PLAYER_MODEL_BODY_REINFORCED_SUIT = "female_geo/reinforced/female_reinforced_body_geo";
        public const string PLAYER_MODEL_BODY_WATER_SUIT      = "female_geo/stillSuit/female_stillSuit_body_geo";

        /**
         *
         * Oyuncu el/eldiven anahtarları
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const string PLAYER_MODEL_HAND_DEFAULT      = "female_geo/base/female_base_hand_geo";
        public const string PLAYER_MODEL_GLOVES_DEFAULT    = "female_geo/base/female_base_gloves_geo";
        public const string PLAYER_MODEL_GLOVES_REINFORCED = "female_geo/reinforced/female_reinforced_hands_geo";
        public const string PLAYER_MODEL_GLOVES_COLD_SUIT  = "female_geo/coldProtective/female_coldProtectiveSuit_hands_geo";

        /**
         *
         * Oyuncu kafa/maske anahtarları
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const string PLAYER_MODEL_MASK_REBREATHER  = "female_geo/base/female_base_mask_geo";
        public const string PLAYER_MODEL_HEAD_DEFAULT     = "female_geo/base/female_base_head_geo";
        public const string PLAYER_MODEL_HEAD_COLD_SUIT   = "female_geo/coldProtective/female_coldProtectiveSuit_head_geo";
        public const string PLAYER_MODEL_MASK_COLD_SUIT   = "female_geo/coldProtective/female_coldProtectiveSuit_mask_geo";
        public const string PLAYER_MODEL_HEAD_FLASH_LIGHT = "export_skeleton/head_rig/FlashlightHelmet";

        /**
         *
         * Oyuncu ayak/yüzgeç anahtarları
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const string PLAYER_MODEL_FLIPPER_FINS = "female_geo/base/female_base_flipper_geo";

        /**
         *
         * Oyuncu sağ ve sol el yapısı
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const string PLAYER_ATTACH_IN_RIGHT_HAND = "export_skeleton/head_rig/neck/chest/clav_R/clav_R_aim/shoulder_R/elbow_R/hand_R/attach1";
        public const string PLAYER_ATTACH_IN_LEFT_HAND  = "export_skeleton/head_rig/neck/chest/clav_L/clav_L_aim/shoulder_L/elbow_L/hand_L/attachL/PlayerPDA";

        /**
         *
         * Menu şema yolları
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public const string MAIN_MENU_BUTTON          = "Menu canvas/Panel/MainMenu/PrimaryOptions/MenuButtons/ButtonPlay";
        public const string MAIN_MENU_BUTTON_CIRCLE   = "Circle/Bar/Text";
        public const string MAIN_MENU_HEADER_IN_GROUP = "Header";
        public const string MAIN_MENU_BUTTON_IN_GROUP = "Scroll View/Viewport/SavedGameAreaContent/NewGame/NewGameButton";
        public const string MAIN_MENU_INPUTBOX        = "Menu canvas/Panel/MainMenu/RightSide/Home/EmailBox";
        public const string MAIN_MENU_SMALL_BUTTON    = "Menu canvas/Panel/Options/Bottom";
    }
}
