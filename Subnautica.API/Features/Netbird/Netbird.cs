using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using Subnautica.API.Extensions;

using UnityEngine;

namespace Subnautica.API.Features.Netbird
{
    public class Netbird
    {
        /**
         *
         * Kurulum bekleme durumunu depolar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool isWaitingInstallation;

        /**
         *
         * Giriş bekleme durumunu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool isWaitingLogin;

        /**
         *
         * JSON geçerlilik durumunu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool isValidJson;

        /**
         *
         * Hata Var mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool isHasError;

        /**
         *
         * peerId değerini durumunu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private string peerId;

        /**
         *
         * peerIp değerini durumunu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private string peerIp;

        /**
         *
         * TextContent değerini durumunu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private string TextContent;

        /**
         *
         * ErrorContent değerini durumunu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private string ErrorContent;

        /**
         *
         * JSON Dynamic verisini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private NetbirdResponseFormat OutputData;

        /**
         *
         * Management Servisini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public NetbirdContainerItem Management { get; set; } = new NetbirdContainerItem();

        /**
         *
         * Signal Servisini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public NetbirdContainerItem Signal { get; set; } = new NetbirdContainerItem();

        /**
         *
         * Relay Servisini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public NetbirdContainerItem Relay { get; set; } = new NetbirdContainerItem();

        /**
         *
         * Stun Servisini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public NetbirdContainerItem Stun { get; set; } = new NetbirdContainerItem();

        /**
         *
         * Turn Servisini barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public NetbirdContainerItem Turn { get; set; } = new NetbirdContainerItem();

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Netbird()
        {

        }

        /**
         *
         * Sınıf ayarlamalarını yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public Netbird(string textContent, string errorContent)
        {
            this.TextContent  = textContent.Trim();
            this.ErrorContent = errorContent.Trim();

            try
            {
                this.Initialize();
            }
            catch (Exception ex)
            {
                Log.Error("DATA:::: err: " + ex);
            }
        }

        /**
         *
         * Ayarlamaları yapar.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool Initialize()
        {
            this.isWaitingInstallation = this.ErrorContent.Contains("service install");
            this.isWaitingLogin        = this.TextContent.Contains("Daemon status: NeedsLogin");
            this.isValidJson           = this.CheckValidJson(this.TextContent);
            this.isHasError            = this.ErrorContent.IsNotNull();

            if (this.TextContent.IsNull())
            {
                return false;
            }

            if (this.IsWaitingInstallation() || this.IsWaitingLogin() || !this.IsValidJson())
            {
                return false;
            }

            try
            {
                this.OutputData = Newtonsoft.Json.JsonConvert.DeserializeObject<NetbirdResponseFormat>(this.TextContent);
                if (this.OutputData == null)
                {
                    this.isValidJson = false;
                    return false;
                }

                if (this.OutputData.NetbirdIp.IsNotNull())
                {
                    this.SetPeerIp(this.OutputData.NetbirdIp);
                }

                if (this.OutputData.PublicKey.IsNotNull())
                {
                    this.SetPeerId(this.OutputData.PublicKey);
                }

                if (this.OutputData.Management != null)
                {
                    this.Management.SetConnected(this.OutputData.Management.Connected);
                    this.Management.SetErrorMessage(this.OutputData.Management.Error);
                }

                if (this.OutputData.Signal != null)
                {
                    this.Signal.SetConnected(this.OutputData.Signal.Connected);
                    this.Signal.SetErrorMessage(this.OutputData.Signal.Error);
                }

                if (this.OutputData.Relays != null && this.OutputData.Relays.Details != null)
                {
                    foreach (var relay in this.OutputData.Relays.Details)
                    {
                        if (relay.Uri.Contains("rels://") || relay.Uri.Contains("rel://") || relay.Uri == "")
                        {
                            this.Relay.SetConnected(relay.Available);
                            this.Relay.SetErrorMessage(relay.Error);
                        }
                        else if (relay.Uri.Contains("turn:"))
                        {
                            this.Turn.SetConnected(relay.Available);
                            this.Turn.SetErrorMessage(relay.Error);
                        }
                        else if (relay.Uri.Contains("stun:"))
                        {
                            this.Stun.SetConnected(relay.Available);
                            this.Stun.SetErrorMessage(relay.Error);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("ex: " + ex);
                this.isValidJson = false;
                return false;
            }
        }

        /**
         *
         * Kurulum bekleniyor mu?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsWaitingInstallation()
        {
            return this.isWaitingInstallation;
        }

        /**
         *
         * Giriş yapılmış mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsWaitingLogin()
        {
            return this.isWaitingLogin;
        }

        /**
         *
         * JSON Geçerliliğini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsValidJson()
        {
            return this.isValidJson;
        }

        /**
         *
         * Hata var mı durumunu döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAnyError()
        {
            return this.isHasError;
        }

        /**
         *
         * PeerIP Adresini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string GetPeerIp()
        {
            return this.peerIp;
        }

        /**
         *
         * PeerID değerini döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string GetPeerId()
        {
            return this.peerId;
        }

        /**
         *
         * PeerIP Değerini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SetPeerIp(string ipAddress)
        {
            this.peerIp = ipAddress.Replace("/16", "").Replace("/24", "").Replace("/32", "").Trim();
            if (this.peerIp.Contains("N/A"))
            {
                this.peerIp = "";
            }
        }

        /**
         *
         * PeerID Değerini değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private void SetPeerId(string peerId)
        {
            this.peerId = peerId;
        }

        /**
         *
         * OutputData Content'i döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string GetOutputDataContent()
        {
            return this.TextContent;
        }

        /**
         *
         * Error Content'i döner.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string GetErrorContent()
        {
            return this.ErrorContent;
        }

        /**
         *
         * JSON Geçerliliğini kopntrol eder.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        private bool CheckValidJson(string content)
        {
            return content.IsNotNull() && content.StartsWith("{") && content.EndsWith("}");
        }
    }
}
