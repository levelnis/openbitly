/// <summary>
/// This code is generated. Any changes to it will be destroyed on subsequent generations. Use the DSL instead.
/// </summary>

using System;
using System.IO;
using System.Collections.Generic;
using OpenBitly.Serialization;

namespace OpenBitly 
{
		
	public class ListHighValueLinksOptions
	{ 
		public int Limit { get; set; } 			
	}			

	public interface IBitlyService
	{
		BitlyResult ListHighValueLinks(ListHighValueLinksOptions options);	
	}

	public partial class BitlyService : IBitlyService
	{
		public virtual BitlyResult ListHighValueLinks(ListHighValueLinksOptions options)
		{
			var limit = options.Limit;	
			
			return WithHammock<BitlyResult>("highvalue", FormatAsString, "?limit=", limit);
		}

	}
}
