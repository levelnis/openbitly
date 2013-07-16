Open Bitly
==========

A Bitly API Wrapper service that uses OAuth 2.0 authentication and T4 templates to make adding new endpoints easy.

Massive kudos to Daniel Crenna and Tweetsharp, upon which most of this was based.

Usage
-----

Here's a quick example of shortening a URL using Open Bitly within a class named `LinkService`:

* Inject an instance of `IBitlyService` and `IBitlyOptionFactory` into `LinkService`
* Instantiate a `ShortenUrlOptions` object with the long url
* Pass the `ShortenUrlOptions` to the service
* Return the short url from the response


    <pre>
    public LinkService(IBitlyService bitlyService, IBitlyOptionFactory bitlyOptionFactory)
    {
      this.bitlyService = bitlyService;
      this.bitlyOptionFactory = bitlyOptionFactory;
    }

    public string ShortenUrl(string fullUrl)
    {
      var options = bitlyOptionFactory.CreateShortenUrlOptions(HttpUtility.UrlEncode(fullUrl));
      var response = bitlyService.ShortenUrl(options);
      return response.Data.Url;
    }
    </pre>
