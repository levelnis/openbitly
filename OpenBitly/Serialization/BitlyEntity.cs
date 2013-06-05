using System;
using System.Runtime.Serialization;

namespace OpenBitly.Serialization
{
    [Serializable]
    public abstract class BitlyEntity : IBitlyEntity
    {
        [DataMember]
        public virtual string RawSource { get; set; }

        [DataMember]
        public virtual int StatusCode { get; set; }

        [DataMember]
        public virtual string StatusTxt { get; set; }
    }
}