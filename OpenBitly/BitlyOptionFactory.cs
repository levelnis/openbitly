namespace OpenBitly
{
    public class BitlyOptionFactory : IBitlyOptionFactory
    {
        public GetClickCountOptions CreateGetClickCountOptions(string link)
        {
            return new GetClickCountOptions { Link = link };
        }

        public GetClickCountOverTimeOptions CreateGetClickCountOverTimeOptions(string link, bool rollup, string unit, int units)
        {
            return new GetClickCountOverTimeOptions { Link = link, Rollup = rollup, Unit = unit, Units = units };
        }

        public ListHighValueLinksOptions CreateListHighValueLinksOptions(int limit)
        {
            return new ListHighValueLinksOptions { Limit = limit };
        }

        public ListSearchResultsOptions CreateListSearchResultsOptions(string cities, string domain, string fields, string lang, int limit, int offset, string query)
        {
            return new ListSearchResultsOptions { Cities = cities, Domain = domain, Fields = fields, Lang = lang, Limit = limit, Offset = offset, Query = query };
        }

        public ShortenUrlOptions CreateShortenUrlOptions(string longUrl)
        {
            return new ShortenUrlOptions { Longurl = longUrl };
        }
    }
}