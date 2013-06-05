using System;
using System.Runtime.Serialization;

namespace OpenBitly.Serialization
{
    [Serializable]
    public class BitlyShortenResult : BitlyEntity
    {
        [DataMember]
        public virtual BitlyShortenData Data { get; set; }
    }
}