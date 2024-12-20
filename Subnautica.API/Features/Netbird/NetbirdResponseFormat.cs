using System.Collections.Generic;

namespace Subnautica.API.Features.Netbird
{
    public class NetbirdResponseFormat
    {
        public NetbirdResponseConnectionFormat Management { get; set; }

        public NetbirdResponseConnectionFormat Signal { get; set; }

        public NetbirdResponseRelayFormat Relays { get; set; }

        public string NetbirdIp { get; set; }

        public string PublicKey { get; set; }
    }

    public class NetbirdResponseConnectionFormat
    {
        public string Url { get; set; }
        
        public string Uri { get; set; }

        public bool Connected { get; set; }

        public bool Available { get; set; }

        public string Error { get; set; }
    }

    public class NetbirdResponseRelayFormat
    {
        public List<NetbirdResponseConnectionFormat> Details;
    }
}
