using System;
using System.Collections.Specialized;
using System.Net;
using Hammock;
using OpenBitly.Serialization;

namespace OpenBitly
{
    public class BitlyResponse
    {
        private readonly RestResponseBase response;
        private readonly Exception exception;

        internal BitlyResponse(RestResponseBase response, Exception exception = null)
        {
            this.response = response;
            this.exception = exception;
        }

        public virtual NameValueCollection Headers
        {
            get { return response.Headers; }
        }

        public virtual HttpStatusCode StatusCode
        {
            get { return response.StatusCode; }
            set { response.StatusCode = value; }
        }

        public virtual bool SkippedDueToRateLimitingRule
        {
            get { return response.SkippedDueToRateLimitingRule; }
            set { response.SkippedDueToRateLimitingRule = value; }
        }

        public virtual string StatusDescription
        {
            get { return response.StatusDescription; }
            set { response.StatusDescription = value; }
        }

        public virtual string Response
        {
            get { return response.Content; }
        }

        public virtual string RequestMethod
        {
            get { return response.RequestMethod; }
            set { response.RequestMethod = value; }
        }

        public virtual Uri RequestUri
        {
            get { return response.RequestUri; }
            set { response.RequestUri = value; }
        }

        public virtual DateTime? ResponseDate
        {
            get { return response.ResponseDate; }
            set { response.ResponseDate = value; }
        }

        public virtual DateTime? RequestDate
        {
            get { return response.RequestDate; }
            set { response.RequestDate = value; }
        }

        public virtual Exception InnerException
        {
            get { return exception ?? response.InnerException; }
        }
    }
}