using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Subnautica.API.Extensions;

namespace Subnautica.API.Features.Netbird
{
    public class NetbirdContainerItem
    {
        /**
         *
         * Bağlantı durumunu barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsConnected { get; private set; }

        /**
         *
         * Hata Mesajını barındırır.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public string ErrorMessage { get; private set; }

        /**
         *
         * Bağlantı durumunu değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetConnected(bool isConnected)
        {
            this.IsConnected = isConnected;
        }

        /**
         *
         * Hata Mesajını değiştirir.
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public void SetErrorMessage(string errorMessage)
        {
            if (errorMessage.IsNotNull())
            {
                this.ErrorMessage = errorMessage;
            }
        }

        /**
         *
         * Hata var mı?
         *
         * @author Ismail <ismaiil_0234@hotmail.com>
         *
         */
        public bool IsAnyError()
        {
            return this.ErrorMessage.IsNotNull();
        }
    }
}
