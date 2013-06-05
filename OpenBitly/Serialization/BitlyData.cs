using System;
using System.Runtime.Serialization;

namespace OpenBitly.Serialization
{
    [Serializable]
    public class BitlyData
    {
        [DataMember]
        public BitlyList<string> Values { get; set; }
    }
}