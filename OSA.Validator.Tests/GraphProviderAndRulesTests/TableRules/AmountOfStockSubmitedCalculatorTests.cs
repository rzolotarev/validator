using System.Collections.Generic;
using NUnit.Framework;
using OSA.Core.Entities.Registration;
using OSA.Editor.ViewModels.BulletinViewModel.Questions;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Cumulative;
using OSA.Validator.Rules.TablesRules.Enums;

namespace OSA.Validator.Tests.GraphProviderAndRulesTests.TableRules
{
    [TestFixture]
    public class AmountOfStockSubmitedCalculatorTests
    {
        private readonly AmountOfStockSubmitedCalculator _calculator = new AmountOfStockSubmitedCalculator();
        //        Yes       Votes   No      Votes   Abs     Votes   Expected
        [TestCase(true,     1,      true,   1,      true,   1,      AmountOfStockSubmited.LessOrEqualThanThereIsOnPack)]
        [TestCase(false,    1,      true,   100,    false,  1,      AmountOfStockSubmited.LessOrEqualThanThereIsOnPack)]
        [TestCase(false,    1,      true,   100,    true,   1,      AmountOfStockSubmited.MoreThanThereIsOnPack)]
        [TestCase(true,     101,    false,  0,      false,  1,      AmountOfStockSubmited.MoreThanThereIsOnPack)]
        [TestCase(true,     0,      false,  0,      false,  0,      AmountOfStockSubmited.VotesArentSubmited)]
        [TestCase(true,     0,      false,  0,      true,   1,      AmountOfStockSubmited.MoreThanThereIsOnPack)]
        [Test]
        public void BaseYesNoAbsPointTests(bool yesCheck, int yesVotes, 
                             bool noCheck, int noVotes, 
                             bool absCheck, int absVotes,
                             AmountOfStockSubmited expected)
        {
            var allowedVotesAmount = new FractionLong(100);
            var baseYesNoAbstainedPoint = new BaseYesNoAbsPoint(
                    Fields.CheckBoxDocField(yesCheck),
                    Fields.CheckBoxDocField(noCheck),
                    Fields.CheckBoxDocField(absCheck),
                    Fields.VotesDocField(yesVotes),
                    Fields.VotesDocField(noVotes),
                    Fields.VotesDocField(absVotes));
            var result = _calculator.GetAmountOfStockSubmited(baseYesNoAbstainedPoint, allowedVotesAmount);

            Assert.That(result, Is.EqualTo(expected));
        }     
        
        //        Yes       Votes   No      Votes   Abs     Votes   Expected
        [TestCase(true,     1,1,1,  true,   1,      true,   1,      AmountOfStockSubmited.LessOrEqualThanThereIsOnPack)]
        [TestCase(true,     1,1,1,  true,   0,      true,   1,      AmountOfStockSubmited.MoreThanThereIsOnPack)]
        [TestCase(false,    1,1,1,  true,   100,    false,  1,      AmountOfStockSubmited.LessOrEqualThanThereIsOnPack)]
        [TestCase(false,    1,1,1,  true,   100,    true,   1,      AmountOfStockSubmited.MoreThanThereIsOnPack)]
        [TestCase(true,     101,0,0,false,  0,      false,  1,      AmountOfStockSubmited.MoreThanThereIsOnPack)]
        
        [TestCase(true,     0,0,0,  false,  0,      false,  0,      AmountOfStockSubmited.VotesArentSubmited)] //Если голоса "ЗА" не указаны, всегда возвращаем VotesArentSubmited для корректной работы калькулятора
        [TestCase(true,     0,0,0,  false,  0,      true,   1,      AmountOfStockSubmited.VotesArentSubmited)]
        [TestCase(true,     0,0,0,  false,  0,      true,   0,      AmountOfStockSubmited.VotesArentSubmited)] 

        [TestCase(false,    0,0,0,  true,   0,      false,  0,      AmountOfStockSubmited.VotesArentSubmited)] 
        [TestCase(false,    0,0,0,  true,   1,      true,   0,      AmountOfStockSubmited.MoreThanThereIsOnPack)]
        [Test]
        public void QCumulativeTests(bool yesCheck, int firstVote, int secondVote, int thirdVote, 
                             bool noCheck, int noVotes, 
                             bool absCheck, int absVotes,
                             AmountOfStockSubmited expected)
        {
            var allowedVotesAmount = new FractionLong(100);
            var qCum = new QCumulative(1,
                                       new List<QCumCandidatePoint>
                                           {
                                               new QCumCandidatePoint(1, Fields.VotesDocField(firstVote)),
                                               new QCumCandidatePoint(2, Fields.VotesDocField(secondVote)),
                                               new QCumCandidatePoint(3, Fields.VotesDocField(thirdVote)),
                                           },
                                       new QCumulativeAdditionalMarks(
                                           Fields.CheckBoxDocField(yesCheck),
                                           Fields.CheckBoxDocField(noCheck),
                                           Fields.VotesDocField(noVotes),
                                           Fields.CheckBoxDocField(absCheck),
                                           Fields.VotesDocField(absVotes))
                            )
                           {
                               PositionsCount = 10
                           };
            var result = _calculator.GetAmountOfStockSubmited(qCum, allowedVotesAmount);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
