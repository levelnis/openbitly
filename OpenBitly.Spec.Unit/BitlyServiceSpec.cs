using NBehave.Spec.NUnit;

namespace OpenBitly.Spec.Unit
{
    public class GivenABitlyService : SpecBase<BitlyService>
    {
        protected override BitlyService Establish_context()
        {
            return new BitlyService();
        }
    }
}