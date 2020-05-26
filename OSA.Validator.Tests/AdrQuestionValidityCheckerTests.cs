using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OSA.Core.Entities.Registration;
using OSA.Editor.ViewModels.BulletinViewModel.Bulletin;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Cumulative;
using OSA.Editor.ViewModels.Fields;
using OSA.Editor.ViewModels.ValidityChecking;

namespace OSA.Validator.Tests
{
    [TestFixture]
    public class AdrQuestionValidityCheckerTests
    {
        [Test]
        [TestCase(102, false)]
        [TestCase(101, false)]
        [TestCase(100, true)]
        [TestCase(99, true)]
        [TestCase(98, true)]
        [TestCase(97, false)]
        [TestCase(96, false)]
        public void Test(int assigned, bool expectedResult)
        {
            var checker = new AdrQuestionValidityChecker();
            int positionsCount = 3;
            var result = checker.QCumulativeIsValid(GetQCumulative(positionsCount, assigned: new FractionLong(assigned)), new FractionLong(100));
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(8, 0, 1, false)]
        [TestCase(7, 2, 3, false)]
        [TestCase(7, 1, 2, true)]
        [TestCase(7, 0, 1, true)]
        [TestCase(6, 1, 2, true)]
        [TestCase(6, 0, 2, true)]
        [TestCase(5, 1, 2, true)]
        [TestCase(5, 5, 11, false)]
        [TestCase(5, 0, 1, false)]
        public void Test2(int assignedIntegral, int assignedNumerator, int assignedDenominator, bool expectedResult)
        {
            var checker = new AdrQuestionValidityChecker();
            const int positionsCount = 3;
            var assigned = new FractionLong(assignedIntegral, assignedNumerator, assignedDenominator);
            var result = checker.QCumulativeIsValid(GetQCumulative(positionsCount, assigned), new FractionLong(7, 1, 2));
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        private static QCumulative GetQCumulative(int positionsCount, FractionLong assigned)
        {
            var qCumulative = new QCumulative(1, GetCandidatePoints(assigned), GetAdditionalMarks());
            qCumulative.AdditionalMarks.YesCheckBoxField.Value = true;
            qCumulative.PositionsCount = positionsCount;
            return qCumulative;
        }

        private static List<QCumCandidatePoint> GetCandidatePoints(FractionLong assigned)
        {
            return Enumerable.Range(1, 10).Select(x => new QCumCandidatePoint(x, VotesDocField(x == 1 ? assigned : (FractionLong?)null))).ToList();
        }

        private static QCumulativeAdditionalMarks GetAdditionalMarks()
        {
            return new QCumulativeAdditionalMarks(CheckBoxField(), CheckBoxField(), VotesDocField(), CheckBoxField(), VotesDocField());
        }

        private static CheckBoxDocField CheckBoxField()
        {
            return new CheckBoxDocField(null, new Page(1), false);
        }

        private static VotesDocField VotesDocField(FractionLong? value = null)
        {

            return new VotesDocField(null, new Page(1), value ?? new FractionLong(0));
        }
    }
}
