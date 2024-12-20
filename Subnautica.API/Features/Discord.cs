namespace Subnautica.API.Features
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using DiscordRPC;

    using Subnautica.API.Features.DiscordManager;

    using UnityEngine;

    public class Discord : MonoBehaviour
    {
        /**
         *
         * Başlangıç zamanını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static DateTime StartedTime { get; set; }

        /**
         *
         * Başlangıç zamanını döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static Queue<RichPresence> Queue { get; set; } = new Queue<RichPresence>();

        /**
         *
         * Client Sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static DiscordRpcClient Client { get; set; }

        /**
         *
         * discordManager Sınıfını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private static GameObject DiscordManager { get; set; }

        /**
         *
         * Sınıf ayarlarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Awake()
        {
            this.transform.parent = null;

            DontDestroyOnLoad(this.gameObject);

            if (Client == null)
            {
                Client = new DiscordRpcClient(Settings.DiscordClientId, pipe: -1, logger: null, autoEvents: true, client: new NamedPipeUnity());
                
                Client.OnError += (sender, e) =>
                {
                    Log.Error("Discord.Integration: " + e.Message);
                };

                Client.OnReady += (sender, e) =>
                {
                    Log.Info("Received Ready from user " + e.User.Username + "#" + e.User.Discriminator);
                };

                Client.OnPresenceUpdate += (sender, e) =>
                {

                };

                Client.Initialize();
            }
        }

        /**
         *
         * Discord durumunu günceller
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public static void UpdateRichPresence(string message, string subMessage = null, bool resetTime = false)
        {
            if (Client == null)
            {
                if (UnityEngine.Object.FindObjectOfType<Discord>() == null)
                {
                    DiscordManager = new GameObject("DiscordManager");
                    DiscordManager.hideFlags = HideFlags.HideAndDontSave;
                    DiscordManager.AddComponent<Discord>();
                    DiscordManager.SetActive(true);

                    resetTime = true;
                }
            }

            if (resetTime)
            {
                StartedTime = DateTime.UtcNow;
            }

            RichPresence rich = new RichPresence()
            {
                Details    = message,
                State      = subMessage,
                Timestamps = new Timestamps(StartedTime),
                Assets     = new Assets()
                {
                    LargeImageKey  = "subnauticabelowzero",
                    LargeImageText = "Subnautica BZ Multiplayer",
                }
            };

            Queue.Enqueue(rich);
        }

        /**
         *
         * Her karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void Update()
        {
            if (Client != null)
            {
                Client.Invoke();
            }
        }

        /**
         *
         * Her sabit karede tetiklenir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void FixedUpdate()
        {
            if (Client != null)
            {
                while (Queue.Count > 0)
                {
                    var item = Queue.Dequeue();

                    Client.SetPresence(item);
                }
            }
        }
    }
}
