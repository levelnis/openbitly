using System;
using System.Runtime.Serialization;

namespace OpenBitly.Serialization
{
    [Serializable]
    public class BitlyData
    {
        [DataMember]
        public virtual BitlyList<string> Values { get; set; }

        [DataMember]
        public virtual BitlyList<BitlySearch> Results { get; set; } 
    }
}