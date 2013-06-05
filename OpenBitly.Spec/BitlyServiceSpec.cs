using System.Configuration;
using System.Net;
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
            result.Data.Values.Count.ShouldEqual(2);
        }
    }
}