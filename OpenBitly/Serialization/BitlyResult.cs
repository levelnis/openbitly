using System;
using System.Runtime.Serialization;

namespace OpenBitly.Serialization
{
    [Serializable]
    public class BitlyResult : IBitlyEntity
    {
        [DataMember]
        public BitlyData Data { get; set; }

        [DataMember]
        public virtual string RawSource { get; set; }
    }
}