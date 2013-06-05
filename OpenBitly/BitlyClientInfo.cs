using Hammock.Web;

namespace OpenBitly
{
    public class BitlyClientInfo : IWebQueryInfo
    {
        public virtual string ClientName { get; set; }
        public virtual string ClientVersion { get; set; }
        public virtual string ClientUrl { get; set; }
        public virtual string ConsumerKey { get; set; }
        public virtual string ConsumerSecret { get; set; }
    }
}