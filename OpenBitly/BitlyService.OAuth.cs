using System;
using System.Compat.Web;
using System.Text;
using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Web;

namespace OpenBitly
{
    public partial class BitlyService
    {
        private class FunctionArguments
        {
            public string ConsumerKey { get; set; }
            public string ConsumerSecret { get; set; }
            public string Token { get; set; }
            public string TokenSecret { get; set; }
            public string Verifier { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }

        private readonly Func<FunctionArguments, RestRequest> accessTokenQuery
            = args =>
            {
                var request = new RestRequest
                {
                    Credentials = new OAuthCredentials
                    {
                        ClientUsername = args.Username,
                        ClientPassword = args.Password,
                        ConsumerKey = args.ConsumerKey,
                        ConsumerSecret = args.ConsumerSecret,
                        ParameterHandling = OAuthParameterHandling.UrlOrPostParameters,
                        SignatureMethod = OAuthSignatureMethod.HmacSha1,
                        Type = OAuthType.ClientAuthentication
                    },
                    Method = WebMethod.Post,
                    Path = "/oauth/access_token"
                };
                return request;
            };

        private readonly Func<FunctionArguments, RestRequest> protectedResourceQuery
            = args =>
            {
                var request = new RestRequest
                {
                    Credentials = new OAuthCredentials
                    {
                        Type = OAuthType.ProtectedResource,
                        SignatureMethod = OAuthSignatureMethod.HmacSha1,
                        ParameterHandling = OAuthParameterHandling.UrlOrPostParameters,
                        ConsumerKey = args.ConsumerKey,
                        ConsumerSecret = args.ConsumerSecret,
                        Token = args.Token
                    }
                };
                request.AddParameter("access_token", args.Token);
                return request;
            };

        private readonly RestClient oauth;
        private const string AuthorizeUrl = Globals.Authority + "/oauth/authorize";
        private const string AuthenticateUrl = Globals.Authority + "/oauth/authenticate";

        public virtual void AuthenticateWith(string accessToken)
        {
            token = accessToken;
        }

        public virtual OAuthAccessToken GetAccessToken()
        {
            var args = new FunctionArguments
            {
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
                Username = username,
                Password = password
            };

            var request = accessTokenQuery.Invoke(args);
            var header = EncodeUserDetails();
            request.AddHeader("Authorization", header);
            var response = oauth.Request(request);

            SetResponse(response);

            var accessToken = new OAuthAccessToken
            {
                Token = response.Content ?? "?"
            };

            return accessToken;
        }

        private string EncodeUserDetails()
        {
            var textToEncode = string.Concat(username, ":", password);
            var bytesToEncode = Encoding.Default.GetBytes(textToEncode);
            return string.Format("Basic {0}", Convert.ToBase64String(bytesToEncode));
        }
    }
}