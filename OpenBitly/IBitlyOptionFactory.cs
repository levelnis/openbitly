namespace OpenBitly
{
    public interface IBitlyOptionFactory
    {
        GetClickCountOptions CreateGetClickCountOptions(string link);
        GetClickCountOverTimeOptions CreateGetClickCountOverTimeOptions(string link, bool rollup, string unit, int units);
        ListHighValueLinksOptions CreateListHighValueLinksOptions(int limit);
        ListSearchResultsOptions CreateListSearchResultsOptions(string cities, string domain, string fields, string lang, int limit, int offset, string query);
        ShortenUrlOptions CreateShortenUrlOptions(string longUrl);
    }
}