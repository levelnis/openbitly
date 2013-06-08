using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OpenBitly.Serialization
{
    [Serializable]
    public class BitlyLinkCollectionData
    {
        [DataMember]
        public List<LinkClick> LinkClicks { get; set; }
    }
}