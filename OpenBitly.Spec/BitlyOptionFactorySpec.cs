using NBehave.Spec.NUnit;
using NUnit.Framework;

namespace OpenBitly.Spec
{
    public abstract class GivenABitlyOptionFactory : SpecBase<BitlyOptionFactory>
    {
        protected override BitlyOptionFactory Establish_context()
        {
            return new BitlyOptionFactory();
        }
    }

    public class WhenCreatingGetClickCountOptions : GivenABitlyOptionFactory
    {
        private const string Link = "http://bity.ly/54she7f";

        private GetClickCountOptions result;

        protected override void Because_of()
        {
            result = Sut.CreateGetClickCountOptions(Link);
        }

        [Test]
        public void ThenTheLinkShouldBeSetCorrectly()
        {
            result.Link.ShouldEqual(Link);
        }
    }

    public class WhenCreatingGetClickCountOverTimeOptions : GivenABitlyOptionFactory
    {
        private const string Link = "http://bity.ly/54she7f";
        private const bool Rollup = true;
        private const string Unit = "day";
        private const int Units = 7;

        private GetClickCountOverTimeOptions result;

        protected override void Because_of()
        {
            result = Sut.CreateGetClickCountOverTimeOptions(Link, Rollup, Unit, Units);
        }

        [Test]
        public void ThenTheLinkShouldBeSetCorrectly()
        {
            result.Link.ShouldEqual(Link);
        }

        [Test]
        public void ThenTheRollupShouldBeSetCorrectly()
        {
            result.Rollup.ShouldEqual(Rollup);
        }

        [Test]
        public void ThenTheUnitShouldBeSetCorrectly()
        {
            result.Unit.ShouldEqual(Unit);
        }

        [Test]
        public void ThenTheUnitsShouldBeSetCorrectly()
        {
            result.Units.ShouldEqual(Units);
        }
    }

    public class WhenCreatingListHighValueLinksOptions : GivenABitlyOptionFactory
    {
        private const int Limit = 20;

        private ListHighValueLinksOptions result;

        protected override void Because_of()
        {
            result = Sut.CreateListHighValueLinksOptions(Limit);
        }

        [Test]
        public void ThenTheLimitShouldBeSetCorrectly()
        {
            result.Limit.ShouldEqual(Limit);
        }
    }

    public class WhenCreatingListSearchResultsOptions : GivenABitlyOptionFactory
    {
        private const string Cities = "us-il-chicago";
        private const string Lang = "en";
        private const string Domain = "friendorsement.co.uk";
        private const string Fields = "url,referrer,epoch,title";
        private const int Limit = 20;
        private const int Offset = 10;
        private const string Query = "Offers";

        private ListSearchResultsOptions result;

        protected override void Because_of()
        {
            result = Sut.CreateListSearchResultsOptions(Cities, Domain, Fields, Lang, Limit, Offset, Query);
        }

        [Test]
        public void ThenTheCitiesShouldBeSetCorrectly()
        {
            result.Cities.ShouldEqual(Cities);
        }

        [Test]
        public void ThenTheDomainShouldBeSetCorrectly()
        {
            result.Domain.ShouldEqual(Domain);
        }

        [Test]
        public void ThenTheFieldsShouldBeSetCorrectly()
        {
            result.Fields.ShouldEqual(Fields);
        }

        [Test]
        public void ThenTheLangShouldBeSetCorrectly()
        {
            result.Lang.ShouldEqual(Lang);
        }

        [Test]
        public void ThenTheLimitShouldBeSetCorrectly()
        {
            result.Limit.ShouldEqual(Limit);
        }

        [Test]
        public void ThenTheOffsetShouldBeSetCorrectly()
        {
            result.Offset.ShouldEqual(Offset);
        }

        [Test]
        public void ThenTheQueryShouldBeSetCorrectly()
        {
            result.Query.ShouldEqual(Query);
        }
    }

    public class WhenCreatingShortenUrlOptions : GivenABitlyOptionFactory
    {
        private const string LongUrl = "http://levelnis.co.uk/blog/";

        private ShortenUrlOptions result;

        protected override void Because_of()
        {
            result = Sut.CreateShortenUrlOptions(LongUrl);
        }

        [Test]
        public void ThenTheLongUrlShouldBeSetCorrectly()
        {
            result.Longurl.ShouldEqual(LongUrl);
        }
    }
}