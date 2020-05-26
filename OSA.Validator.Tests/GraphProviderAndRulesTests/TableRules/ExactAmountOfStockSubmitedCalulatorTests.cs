using System.Collections.Generic;
using NUnit.Framework;
using OSA.Core.Entities.Registration;
using OSA.Core.Util.Extensions.FractionLongs;
using OSA.Editor.ViewModels.AmountOfVotesCalculators;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Cumulative;

namespace OSA.Validator.Tests.GraphProviderAndRulesTests.TableRules
{
    [TestFixture]
    public class ExactAmountOfStockSubmitedCalulatorTests
    {
        private readonly ExactAmountOfStockSubmitedCalulator _calculator = new ExactAmountOfStockSubmitedCalulator();

        //        Yes       Votes   HasInstr    HasTrust     NotWholeStockWasPassed   ExpectedVotes
        [TestCase(true,     0,      false,      false,       false,                   100)]
        [TestCase(true,     1,      false,      false,       false,                   100)]
        [TestCase(true,     101,    false,      false,       false,                   100)]

        [TestCase(false,     101,   false,      false,       false,                   0)]
        [TestCase(false,     1,     false,      true,        false,                   0)]

        [TestCase(true,     1,      true,       false,       false,                   1)]
        [TestCase(true,     0,      true,       false,       false,                   100)]
        [Test]
        public void BaseYesNoAbsPointTests(bool yesCheck, long yesVotes, bool hasInstructions, bool hasTrust, bool notWholeStockWasPassed, long expectedResultLong)
        {
            var allowedVotesAmount = new FractionLong(100);

            var point = TestBulletinModel.GetFreshBaseYesNoAbsPoint(yesVotes: yesVotes.FL(), yesCheck: yesCheck);

            var additionalMarks = TestBulletinModel.GetAdditionalMarks(hasInstructions, hasTrust, notWholeStockWasPassed);

            var result = _calculator.GetVotesForYesField(point, allowedVotesAmount, additionalMarks);
            var expected = new FractionLong(expectedResultLong);
            Assert.That(result, Is.EqualTo(expected));
        }     
        
        [TestCase(false, 100, 0)]
        [TestCase(false, 0, 0)]
        [TestCase(true, 1, 1)]
        [TestCase(true, 0, 0)]
        [Test]
        public void QCumulativeCandidateVoteTests(bool yesCheck, long yesVotes, long expectedResultLong)
        {
            const bool doesntMatter2 = false;
            const long doesntMatter3 = 0;
            const int doesntMatter4 = 0;
            const int doesntMatter5 = 1;

            var candidatePoint= new QCumCandidatePoint(doesntMatter5, Fields.VotesDocField(yesVotes.FL()));


            var qCum = new QCumulative(doesntMatter5, new List<QCumCandidatePoint>{candidatePoint},
                            new QCumulativeAdditionalMarks(Fields.CheckBoxDocField(yesCheck),
                                                    Fields.CheckBoxDocField(doesntMatter2),
                                                    Fields.VotesDocField(doesntMatter3.FL()),
                                                    Fields.CheckBoxDocField(doesntMatter2),
                                                    Fields.VotesDocField(doesntMatter3.FL(),
                                                    doesntMatter4)));

            var result = _calculator.GetVotesForCandidateField(qCum, candidatePoint);
            var expected = new FractionLong(expectedResultLong);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}