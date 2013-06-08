using System;
using System.Runtime.Serialization;

namespace OpenBitly.Serialization
{
    [Serializable]
    public class BitlyLinkCollectionResult : BitlyEntity
    {
        [DataMember]
        public virtual BitlyLinkCollectionData Data { get; set; }
    }
}