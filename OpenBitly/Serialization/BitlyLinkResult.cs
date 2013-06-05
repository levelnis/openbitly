using System;
using System.Runtime.Serialization;

namespace OpenBitly.Serialization
{
    [Serializable]
    public class BitlyLinkResult : BitlyEntity
    {
        [DataMember]
        public virtual BitlyLinkData Data { get; set; }
    }
}