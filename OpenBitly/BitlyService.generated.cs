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
		
	public class ListSearchResultsOptions
	{ 
		public int Limit { get; set; }  
		public int Offset { get; set; }  
		public string Query { get; set; }  
		public string Lang { get; set; }  
		public string Cities { get; set; }  
		public string Domain { get; set; }  
		public string Fields { get; set; } 			
	}			
		
	public class ShortenUrlOptions
	{ 
		public string Longurl { get; set; } 			
	}			
		
	public class GetClickCountOptions
	{ 
		public string Link { get; set; } 			
	}			
		
	public class GetClickCountOverTimeOptions
	{ 
		public int Units { get; set; }  
		public bool Rollup { get; set; }  
		public string Link { get; set; }  
		public string Unit { get; set; } 			
	}			

	public interface IBitlyService
	{
		BitlyResult ListHighValueLinks(ListHighValueLinksOptions options);	

		BitlyResult ListSearchResults(ListSearchResultsOptions options);	

		BitlyShortenResult ShortenUrl(ShortenUrlOptions options);	

		BitlyLinkResult GetClickCount(GetClickCountOptions options);	

		BitlyLinkCollectionResult GetClickCountOverTime(GetClickCountOverTimeOptions options);	
	}

	public partial class BitlyService : IBitlyService
	{
		public virtual BitlyResult ListHighValueLinks(ListHighValueLinksOptions options)
		{
			 
			var limit = options.Limit;	
			
			return WithHammock<BitlyResult>("highvalue", FormatAsString, "?limit=", limit);
		}


		public virtual BitlyResult ListSearchResults(ListSearchResultsOptions options)
		{
			 
			var limit = options.Limit; 
			var offset = options.Offset; 
			var query = options.Query; 
			var lang = options.Lang; 
			var cities = options.Cities; 
			var domain = options.Domain; 
			var fields = options.Fields;	
			
			return WithHammock<BitlyResult>("search", FormatAsString, "?limit=", limit, "&offset=", offset, "&query=", query, "&lang=", lang, "&cities=", cities, "&domain=", domain, "&fields=", fields);
		}


		public virtual BitlyShortenResult ShortenUrl(ShortenUrlOptions options)
		{
			 
			var longUrl = options.Longurl;	
			
			return WithHammock<BitlyShortenResult>("shorten", FormatAsString, "?longUrl=", longUrl);
		}


		public virtual BitlyLinkResult GetClickCount(GetClickCountOptions options)
		{
			 
			var link = options.Link;	
			
			return WithHammock<BitlyLinkResult>("link/clicks", FormatAsString, "?link=", link);
		}


		public virtual BitlyLinkCollectionResult GetClickCountOverTime(GetClickCountOverTimeOptions options)
		{
			 
			var units = options.Units; 
			var rollup = options.Rollup; 
			var link = options.Link; 
			var unit = options.Unit;	
			
			return WithHammock<BitlyLinkCollectionResult>("link/clicks", FormatAsString, "?units=", units, "&rollup=", rollup, "&link=", link, "&unit=", unit);
		}

	}
}
