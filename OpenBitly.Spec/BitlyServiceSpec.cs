using System;
using System.Configuration;
using System.Net;
using System.Web;
using NBehave.Spec.NUnit;
using NUnit.Framework;
using OpenBitly.Serialization;

namespace OpenBitly.Spec
{
    public abstract class GivenABitlyService : SpecBase<BitlyService>
    {
        protected string ConsumerKey;
        protected string ConsumerSecret;
        protected string AccessToken;
        protected string Username;
        protected string Password;

        protected override BitlyService Establish_context()
        {
            ConsumerKey = ConfigurationManager.AppSettings["ConsumerKey"];
            ConsumerSecret = ConfigurationManager.AppSettings["ConsumerSecret"];
            AccessToken = ConfigurationManager.AppSettings["AccessToken"];
            Username = ConfigurationManager.AppSettings["Username"];
            Password = ConfigurationManager.AppSettings["Password"];
            return new BitlyService(ConsumerKey, ConsumerSecret, Username, Password);
        }

        protected static void AssertResultWas(BitlyService service, HttpStatusCode statusCode)
        {
            service.Response.ShouldNotBeNull();
            service.Response.StatusCode.ShouldEqual(statusCode);
        }
    }

    public abstract class GivenAnAuthenticatedBitlyService : GivenABitlyService
    {
        protected override BitlyService Establish_context()
        {
            var context = base.Establish_context();
            context.AuthenticateWith(AccessToken);
            return context;
        }
    }

    public class WhenGettingAccessToken : GivenABitlyService
    {
        private OAuthAccessToken result;

        protected override void Because_of()
        {
            result = Sut.GetAccessToken();
        }

        [Test]
        public void ThenAccessTokenShouldNotBeNull()
        {
            result.ShouldNotBeNull();
        }

        [Test]
        public void ThenResponseStatusShouldBeOk()
        {
            AssertResultWas(Sut, HttpStatusCode.OK);
        }
    }

    public class WhenListingHighValueLinks : GivenAnAuthenticatedBitlyService
    {
        private ListHighValueLinksOptions input;
        private const int InputLimit = 2;
        private BitlyResult result;

        protected override BitlyService Establish_context()
        {
            input = new ListHighValueLinksOptions {Limit = InputLimit};
            return base.Establish_context();
        }

        protected override void Because_of()
        {
            result = Sut.ListHighValueLinks(input);
        }

        [Test]
        public void ThenItShouldReturn2Links()
        {
            result.Data.Values.Count.ShouldEqual(InputLimit);
        }
    }

    public class WhenGettingLinkClicks : GivenAnAuthenticatedBitlyService
    {
        private GetClickCountOptions input;
        private const string Link = "http://bit.ly/13aeJYW";
        private const int ClickCount = 4; // this is very flaky - it's a live link and only equals 4 until someone pastes the link into their browser
        private BitlyLinkResult result;

        protected override BitlyService Establish_context()
        {
            input = new GetClickCountOptions {Link = HttpUtility.UrlEncode(Link)};
            return base.Establish_context();
        }

        protected override void Because_of()
        {
            result = Sut.GetClickCount(input);
        }

        [Test]
        public void ThenItShouldReturn4SearchResults()
        {
            result.Data.LinkClicks.ShouldEqual(ClickCount);
        }

        [Test]
        public void ThenItShouldReturnStatusCode200()
        {
            result.StatusCode.ShouldEqual(200);
        }

        [Test]
        public void ThenItShouldReturnStatusTxtOk()
        {
            result.StatusTxt.ShouldEqual("OK");
        }
    }

    public class WhenGettingLinkClicksOverTime : GivenAnAuthenticatedBitlyService
    {
        private GetClickCountOverTimeOptions input;
        private const string Link = "http://bit.ly/13aeJYW";
        private const int DayCount = 2; // this is very flaky - it's a live link and only equals 2 until someone pastes the link into their browser
        private const int FirstDayCount = 1; 
        private const int SecondDayCount = 3; 
        private DateTime firstDay = new DateTime(2013, 6, 8);
        private DateTime secondDay = new DateTime(2013, 6, 5);
        private BitlyLinkCollectionResult result;

        protected override BitlyService Establish_context()
        {
            input = new GetClickCountOverTimeOptions { Link = HttpUtility.UrlEncode(Link), Unit = "day", Units = -1, Rollup = false };
            return base.Establish_context();
        }

        protected override void Because_of()
        {
            result = Sut.GetClickCountOverTime(input);
        }

        [Test]
        public void ThenItShouldReturn2DaysOfResults()
        {
            result.Data.LinkClicks.Count.ShouldEqual(DayCount);
        }

        [Test]
        public void ThenItShouldReturn3SearchResultsOnTheFirstDay()
        {
            var linkClick = result.Data.LinkClicks[0];
            var dateTime = linkClick.Dt.AsDateTime();
            linkClick.Clicks.ShouldEqual(FirstDayCount);
            dateTime.Date.ShouldEqual(firstDay.Date);
        }

        [Test]
        public void ThenItShouldReturn1SearchResultOnTheSecondDay()
        {
            var linkClick = result.Data.LinkClicks[1];
            var dateTime = linkClick.Dt.AsDateTime();
            linkClick.Clicks.ShouldEqual(SecondDayCount);
            dateTime.Date.ShouldEqual(secondDay.Date);
        }

        [Test]
        public void ThenItShouldReturnStatusCode200()
        {
            result.StatusCode.ShouldEqual(200);
        }

        [Test]
        public void ThenItShouldReturnStatusTxtOk()
        {
            result.StatusTxt.ShouldEqual("OK");
        }
    }

    public class WhenSearchingForLinks : GivenAnAuthenticatedBitlyService
    {
        private ListSearchResultsOptions input;
        private const int InputLimit = 3;
        private const string Query = "obama";
        private BitlyResult result;

        protected override BitlyService Establish_context()
        {
            input = new ListSearchResultsOptions {Limit = InputLimit, Query = Query};
            return base.Establish_context();
        }

        protected override void Because_of()
        {
            result = Sut.ListSearchResults(input);
        }

        [Test]
        public void ThenItShouldReturn3SearchResults()
        {
            result.Data.Results.Count.ShouldEqual(InputLimit);
        }

        [Test]
        public void ThenItShouldReturnStatusCode200()
        {
            result.StatusCode.ShouldEqual(200);
        }

        [Test]
        public void ThenItShouldReturnStatusTxtOk()
        {
            result.StatusTxt.ShouldEqual("OK");
        }
    }

    public class WhenShorteningAUrl : GivenAnAuthenticatedBitlyService
    {
        private ShortenUrlOptions input;
        private const string LongUrl = "http://levelnis.co.uk/";
        private BitlyShortenResult result;

        protected override BitlyService Establish_context()
        {
            input = new ShortenUrlOptions { Longurl = HttpUtility.UrlEncode(LongUrl) };
            return base.Establish_context();
        }

        protected override void Because_of()
        {
            result = Sut.ShortenUrl(input); 
        }

        [Test]
        public void ThenItShouldReturn3SearchResults()
        {
            result.Data.Url.StartsWith("http://bit.ly/").ShouldBeTrue();
        }

        [Test]
        public void ThenItShouldReturnStatusCode200()
        {
            result.StatusCode.ShouldEqual(200);
        }

        [Test]
        public void ThenItShouldReturnStatusTxtOk()
        {
            result.StatusTxt.ShouldEqual("OK");
        }
    }
}