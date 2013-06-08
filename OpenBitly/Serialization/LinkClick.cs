using System;
using System.Runtime.Serialization;

namespace OpenBitly.Serialization
{
    [Serializable]
    public class LinkClick
    {
        [DataMember]
        public int Clicks { get; set; }

        [DataMember]
        public int Dt { get; set; }
    }
}