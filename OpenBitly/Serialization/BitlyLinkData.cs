using System;
using System.Runtime.Serialization;

namespace OpenBitly.Serialization
{
    [Serializable]
    public class BitlyLinkData
    {
        [DataMember]
        public virtual int LinkClicks { get; set; }
    }
}