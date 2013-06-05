using System;
using System.Runtime.Serialization;

namespace OpenBitly.Serialization
{
    [Serializable]
    public class BitlyShortenData
    {
        [DataMember]
        public string GlobalHash { get; set; }

        [DataMember]
        public string Hash { get; set; }

        [DataMember]
        public string LongUrl { get; set; }

        [DataMember]
        public string Url { get; set; }

        [DataMember]
        public int NewHash { get; set; }
    }
}