using System.Linq;
using NUnit.Framework;
using OSA.Core.Util.Extensions.FractionLongs;
using OSA.Validator.FractionDistribution;

namespace OSA.Validator.Tests.FractionDistributionTests
{
    [TestFixture]
    public class SellerFractionsDistributionAnalyzerTests
    {
        [Test]
        [Timeout(100 /*ms*/)]
        public void ZeroFractionPartFieldsAndFractionPartsAreFilteredOut()
        {
            var sellerFractionsDistributionAnalyzer = new SellerFractionsDistributionAnalyzer();
            var hundrendZeroFields = Enumerable.Range(0, 100).Select(x => x.FL()).ToList();
            var hundrendZeroFractionParts = Enumerable.Repeat(0, 100).Select(x => x.FL()).ToList();
            sellerFractionsDistributionAnalyzer.FractionDistributionIsPossible(hundrendZeroFields, hundrendZeroFractionParts);
        }
    }
}
