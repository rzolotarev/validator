using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OSA.Core.Entities.Registration;
using OSA.Editor.ViewModels.BulletinViewModel.Bulletin;
using OSA.Editor.ViewModels.BulletinViewModel.Questions;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Cumulative;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Separate;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Simple;
using OSA.Validator.GraphBuilding;
using OSA.Validator.Rules;
using OSA.Validator.Rules.PageRules;
using OSA.Validator.Rules.QCumulativeRules;
using OSA.Validator.Rules.QSeparateRules;
using OSA.Validator.Rules.TablesRules;
using OSA.Validator.Rules.TablesRules.QCumulativeTableRules;
using OSA.Validator.Rules.TablesRules.QSimpleQSepAndHierSubQTableRules;

namespace OSA.Validator.Tests.GraphProviderAndRulesTests
{
    [TestFixture]
    public class GraphProviderBuildGraphTests
    {
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            AllowedVotesAmount = FractionLong.Zero;
            InitBulletin();
        }

        [Test]
        public void Test()
        {
            var graph = GraphProvider.BuildGraph(Bulletin);

            var rootRule = (PacketMustBeRegisteredRule)graph;

            Assert.That(rootRule.DependantRule != null);
            Assert.That(rootRule._packetIsRegistered.Value, Is.EqualTo(true));

            var signaturesParallelRule = (ParallelRule)rootRule.DependantRule;


            Assert.That(signaturesParallelRule.ParallelRules.Count, Is.EqualTo(3));
            Assert.That(signaturesParallelRule.ParallelRules.Select(x => ((SignatureMustBePresentRule)x).Signature)
                            .SequenceEqual(new List<Signature> { Signature1, Signature2, Signature3}));

            Assert.That(signaturesParallelRule.DependantRule == null);

            var ruleAfterSignatureRule1 = (ParallelRule)signaturesParallelRule.ParallelRules[0].DependantRule;
            var ruleAfterSignatureRule2 = (ParallelRule)signaturesParallelRule.ParallelRules[1].DependantRule;
            var ruleAfterSignatureRule3 = (ParallelRule)signaturesParallelRule.ParallelRules[2].DependantRule;

            Assert.That(ruleAfterSignatureRule1.ParallelRules.Count, Is.EqualTo(2));
            Assert.That(ruleAfterSignatureRule2.ParallelRules.Count, Is.EqualTo(2));
            Assert.That(ruleAfterSignatureRule3.ParallelRules.Count, Is.EqualTo(2));

            var rulesAfterEverySignatureRule = signaturesParallelRule.ParallelRules.Select(x => (ParallelRule)x.DependantRule).ToList();

            Assert.That(rulesAfterEverySignatureRule.All(r => r.ParallelRules.Count == 2));

            var ownersCountWarningRules = rulesAfterEverySignatureRule.Select(x => x.ParallelRules[0]).ToList();
            Assert.That(ownersCountWarningRules.Select(x => ((OwnersCountSignatureWarningRule)x).Signature)
                .SequenceEqual(new List<Signature> { Signature1, Signature2, Signature3}));
            Assert.That(ownersCountWarningRules.All(x => x.DependantRule == null));


            var signature1QuestionRules = (ParallelRule)rulesAfterEverySignatureRule[0].ParallelRules[1];
            var signature2QuestionRules = (ParallelRule)rulesAfterEverySignatureRule[1].ParallelRules[1];
            var signature3QuestionRules = (ParallelRule)rulesAfterEverySignatureRule[2].ParallelRules[1];

            Assert.That(signature1QuestionRules.ParallelRules.Count, Is.EqualTo(4));
            Assert.That(signature2QuestionRules.ParallelRules.Count, Is.EqualTo(2));
            Assert.That(signature3QuestionRules.ParallelRules.Count, Is.EqualTo(1));


            //////////////QSIMPLE/////////////////

            var qSimple1TopRule = (PointShouldHaveAtLeastOneSelectionRule)signature1QuestionRules.ParallelRules[0];
            var qSimple2TopRule = (PointShouldHaveAtLeastOneSelectionRule)signature1QuestionRules.ParallelRules[1];
            var qSimple3TopRule = (PointShouldHaveAtLeastOneSelectionRule)signature1QuestionRules.ParallelRules[2];

            Assert.That(qSimple1TopRule.Point, Is.EqualTo(QSimple1.Point));
            Assert.That(qSimple2TopRule.Point, Is.EqualTo(QSimple2.Point));
            Assert.That(qSimple3TopRule.Point, Is.EqualTo(QSimple3.Point));



            var qsimple1BelowRule = (InCaseOfSimplePackPointWithNoAdditionalMarksAlwaysPassesRule)qSimple1TopRule.DependantRule;
            var qsimple2BelowRule = (InCaseOfSimplePackPointWithNoAdditionalMarksAlwaysPassesRule)qSimple2TopRule.DependantRule;
            var qsimple3BelowRule = (InCaseOfSimplePackPointWithNoAdditionalMarksAlwaysPassesRule)qSimple3TopRule.DependantRule;

            Assert.That(qsimple1BelowRule.Point, Is.EqualTo(QSimple1.Point));
            Assert.That(qsimple1BelowRule.AdditionalMarks, Is.EqualTo(Bulletin.AdditionalMarks));

            Assert.That(qsimple2BelowRule.Point, Is.EqualTo(QSimple2.Point));
            Assert.That(qsimple2BelowRule.AdditionalMarks, Is.EqualTo(Bulletin.AdditionalMarks));

            Assert.That(qsimple3BelowRule.Point, Is.EqualTo(QSimple3.Point));
            Assert.That(qsimple3BelowRule.AdditionalMarks, Is.EqualTo(Bulletin.AdditionalMarks));



            var qsimple1BelowRule2 = (MultivariantVotingPointRule)qsimple1BelowRule.DependantRule;
            var qsimple2BelowRule2 = (MultivariantVotingPointRule)qsimple2BelowRule.DependantRule;
            var qsimple3BelowRule2 = (MultivariantVotingPointRule)qsimple3BelowRule.DependantRule;

            Assert.That(qsimple1BelowRule2.Point, Is.EqualTo(QSimple1.Point));
            Assert.That(qsimple1BelowRule2.AdditionalMarks, Is.EqualTo(Bulletin.AdditionalMarks));

            Assert.That(qsimple2BelowRule2.Point, Is.EqualTo(QSimple2.Point));
            Assert.That(qsimple2BelowRule2.AdditionalMarks, Is.EqualTo(Bulletin.AdditionalMarks));

            Assert.That(qsimple3BelowRule2.Point, Is.EqualTo(QSimple3.Point));
            Assert.That(qsimple3BelowRule2.AdditionalMarks, Is.EqualTo(Bulletin.AdditionalMarks));



            var qsimple1BelowRule3 = (QSimpleQSepAndHierSubQTableRule)qsimple1BelowRule2.DependantRule;
            var qsimple2BelowRule3 = (QSimpleQSepAndHierSubQTableRule)qsimple2BelowRule2.DependantRule;
            var qsimple3BelowRule3 = (QSimpleQSepAndHierSubQTableRule)qsimple3BelowRule2.DependantRule;

            Assert.That(qsimple1BelowRule3.Point, Is.EqualTo(QSimple1.Point));
            Assert.That(qsimple1BelowRule3.AdditionalMarks, Is.EqualTo(Bulletin.AdditionalMarks));

            Assert.That(qsimple2BelowRule3.Point, Is.EqualTo(QSimple2.Point));
            Assert.That(qsimple2BelowRule3.AdditionalMarks, Is.EqualTo(Bulletin.AdditionalMarks));

            Assert.That(qsimple3BelowRule3.Point, Is.EqualTo(QSimple3.Point));
            Assert.That(qsimple3BelowRule3.AdditionalMarks, Is.EqualTo(Bulletin.AdditionalMarks));


            var qsimple1BelowRule4 = (QSimpleQSepAndHierSubQFractionDistributionRule)qsimple1BelowRule3.DependantRule;
            var qsimple2BelowRule4 = (QSimpleQSepAndHierSubQFractionDistributionRule)qsimple2BelowRule3.DependantRule;
            var qsimple3BelowRule4 = (QSimpleQSepAndHierSubQFractionDistributionRule)qsimple3BelowRule3.DependantRule;

            Assert.That(qsimple1BelowRule4.Point, Is.EqualTo(QSimple1.Point));
            Assert.That(qsimple2BelowRule4.Point, Is.EqualTo(QSimple2.Point));
            Assert.That(qsimple3BelowRule4.Point, Is.EqualTo(QSimple3.Point));


            var qsimple1BelowRule5 = (AdrMustHaveExactVotesSpreadedForQSimple)qsimple1BelowRule4.DependantRule;
            var qsimple2BelowRule5 = (AdrMustHaveExactVotesSpreadedForQSimple)qsimple2BelowRule4.DependantRule;
            var qsimple3BelowRule5 = (AdrMustHaveExactVotesSpreadedForQSimple)qsimple3BelowRule4.DependantRule;

            Assert.That(qsimple1BelowRule5.Point, Is.EqualTo(QSimple1.Point));

            Assert.That(qsimple2BelowRule5.Point, Is.EqualTo(QSimple2.Point));

            Assert.That(qsimple3BelowRule5.Point, Is.EqualTo(QSimple3.Point));

            var qsimple1BelowRule6 = (QSimpleQSepAndHierSubQWarningRule)qsimple1BelowRule5.DependantRule;
            var qsimple2BelowRule6 = (QSimpleQSepAndHierSubQWarningRule)qsimple2BelowRule5.DependantRule;
            var qsimple3BelowRule6 = (QSimpleQSepAndHierSubQWarningRule)qsimple3BelowRule5.DependantRule;

            Assert.That(qsimple1BelowRule6.Point, Is.EqualTo(QSimple1.Point));
            Assert.That(qsimple1BelowRule6.AdditionalMarks, Is.EqualTo(Bulletin.AdditionalMarks));

            Assert.That(qsimple2BelowRule6.Point, Is.EqualTo(QSimple2.Point));
            Assert.That(qsimple2BelowRule6.AdditionalMarks, Is.EqualTo(Bulletin.AdditionalMarks));

            Assert.That(qsimple3BelowRule6.Point, Is.EqualTo(QSimple3.Point));
            Assert.That(qsimple3BelowRule6.AdditionalMarks, Is.EqualTo(Bulletin.AdditionalMarks));


            var qsimple1BelowRule7 = (QSimplePrivilegeDividendsNoAbsWarningRule)qsimple1BelowRule6.DependantRule;
            var qsimple2BelowRule7 = (QSimplePrivilegeDividendsNoAbsWarningRule)qsimple2BelowRule6.DependantRule;
            var qsimple3BelowRule7 = (QSimplePrivilegeDividendsNoAbsWarningRule)qsimple3BelowRule6.DependantRule;

            Assert.That(qsimple1BelowRule7.Question, Is.EqualTo(QSimple1));
            Assert.That(qsimple1BelowRule7.DependantRule, Is.Null);

            Assert.That(qsimple2BelowRule7.Question, Is.EqualTo(QSimple2));
            Assert.That(qsimple2BelowRule7.DependantRule, Is.Null);

            Assert.That(qsimple3BelowRule7.Question, Is.EqualTo(QSimple3));
            Assert.That(qsimple3BelowRule7.DependantRule, Is.Null);
            //////////////QSEPARATE/////////////////

            var qSeparate1ParallelRule = (ParallelRule)signature1QuestionRules.ParallelRules[3];
            var qSeparate2ParallelRule = (ParallelRule)signature2QuestionRules.ParallelRules[0];

            Assert.That(qSeparate1ParallelRule.ParallelRules.Count, Is.EqualTo(10));
            Assert.That(qSeparate1ParallelRule.ParallelRules.All(r => r is PointShouldHaveAtLeastOneSelectionRule));
            Assert.That(qSeparate1ParallelRule.ParallelRules.Select(x => x.DependantRule).All(r => r is InCaseOfSimplePackPointWithNoAdditionalMarksAlwaysPassesRule));
            Assert.That(qSeparate1ParallelRule.ParallelRules.Select(x => x.DependantRule).All(r => r.DependantRule is MultivariantVotingPointRule));
            Assert.That(qSeparate1ParallelRule.ParallelRules.Select(x => x.DependantRule).All(r => r.DependantRule.DependantRule is QSimpleQSepAndHierSubQTableRule));
            Assert.That(qSeparate1ParallelRule.ParallelRules.Select(x => x.DependantRule).All(r => r.DependantRule.DependantRule.DependantRule is QSimpleQSepAndHierSubQFractionDistributionRule));
            Assert.That(qSeparate1ParallelRule.ParallelRules.Select(x => x.DependantRule).All(r => r.DependantRule.DependantRule.DependantRule.DependantRule is QSimpleQSepAndHierSubQWarningRule));
            Assert.That(qSeparate1ParallelRule.ParallelRules.Select(x => x.DependantRule).All(r => r.DependantRule.DependantRule.DependantRule.DependantRule.DependantRule == null));

            Assert.That(qSeparate2ParallelRule.ParallelRules.Count, Is.EqualTo(10));
            Assert.That(qSeparate2ParallelRule.ParallelRules.All(r => r is PointShouldHaveAtLeastOneSelectionRule));
            Assert.That(qSeparate2ParallelRule.ParallelRules.Select(x => x.DependantRule).All(r => r is InCaseOfSimplePackPointWithNoAdditionalMarksAlwaysPassesRule));
            Assert.That(qSeparate2ParallelRule.ParallelRules.Select(x => x.DependantRule).All(r => r.DependantRule is MultivariantVotingPointRule));
            Assert.That(qSeparate2ParallelRule.ParallelRules.Select(x => x.DependantRule).All(r => r.DependantRule.DependantRule is QSimpleQSepAndHierSubQTableRule));
            Assert.That(qSeparate2ParallelRule.ParallelRules.Select(x => x.DependantRule).All(r => r.DependantRule.DependantRule.DependantRule is QSimpleQSepAndHierSubQFractionDistributionRule));
            Assert.That(qSeparate2ParallelRule.ParallelRules.Select(x => x.DependantRule).All(r => r.DependantRule.DependantRule.DependantRule.DependantRule is QSimpleQSepAndHierSubQWarningRule));
            Assert.That(qSeparate2ParallelRule.ParallelRules.Select(x => x.DependantRule).All(r => r.DependantRule.DependantRule.DependantRule.DependantRule.DependantRule == null));


            var qSeparate1NumberOfPlacesRule = (QSeparateYesChecksShouldNotExceedPlacesCountRule)qSeparate1ParallelRule.DependantRule;
            var qSeparate2NumberOfPlacesRule = (QSeparateYesChecksShouldNotExceedPlacesCountRule)qSeparate2ParallelRule.DependantRule; 

            Assert.That(qSeparate1NumberOfPlacesRule.Question, Is.EqualTo(QSeparate1));
            Assert.That(qSeparate2NumberOfPlacesRule.Question, Is.EqualTo(QSeparate2));
            Assert.That(qSeparate1NumberOfPlacesRule.AdditionalMarks, Is.EqualTo(Bulletin.AdditionalMarks));
            Assert.That(qSeparate2NumberOfPlacesRule.AdditionalMarks, Is.EqualTo(Bulletin.AdditionalMarks));



            var qSeparate1AdrRule = (AdrMustHaveExactOrLessVotesSpreadedForQSep)qSeparate1NumberOfPlacesRule.DependantRule;
            var qSeparate2AdrRule = (AdrMustHaveExactOrLessVotesSpreadedForQSep)qSeparate2NumberOfPlacesRule.DependantRule; 

            Assert.That(qSeparate1AdrRule.QSeparate, Is.EqualTo(QSeparate1));
            Assert.That(qSeparate2AdrRule.QSeparate, Is.EqualTo(QSeparate2));

            Assert.That(qSeparate1AdrRule.DependantRule, Is.EqualTo(null));
            Assert.That(qSeparate2AdrRule.DependantRule, Is.EqualTo(null));


            //////////////QCUMULATIVE/////////////////

            var qCumulative1TopRule = (QCumulativeMustHaveAtLeastOneSelectionRule)signature2QuestionRules.ParallelRules[1];
            var qCumulative2TopRule = (QCumulativeMustHaveAtLeastOneSelectionRule)signature3QuestionRules.ParallelRules[0];

            Assert.That(qCumulative1TopRule.Question, Is.EqualTo(QCumulative1));

            Assert.That(qCumulative2TopRule.Question, Is.EqualTo(QCumulative2));

            ///

            var qCumulative1BelowRule = (InCaseOfSimplePackQCumulativeWithNoAdditionalMarksAlwaysPassesRule)qCumulative1TopRule.DependantRule;
            var qCumulative2BelowRule = (InCaseOfSimplePackQCumulativeWithNoAdditionalMarksAlwaysPassesRule)qCumulative2TopRule.DependantRule;

            Assert.That(qCumulative1BelowRule.Question, Is.EqualTo(QCumulative1));
            Assert.That(qCumulative1BelowRule.AdditionalMarks, Is.EqualTo(Bulletin.AdditionalMarks));

            Assert.That(qCumulative2BelowRule.Question, Is.EqualTo(QCumulative2));
            Assert.That(qCumulative2BelowRule.AdditionalMarks, Is.EqualTo(Bulletin.AdditionalMarks));

            ///

            var qCumulative1BelowRule2 = (MultivariantVotingQCumulativeRule)qCumulative1BelowRule.DependantRule;
            var qCumulative2BelowRule2 = (MultivariantVotingQCumulativeRule)qCumulative2BelowRule.DependantRule;

            Assert.That(qCumulative1BelowRule2.Question, Is.EqualTo(QCumulative1));
            Assert.That(qCumulative1BelowRule2.AdditionalMarks, Is.EqualTo(Bulletin.AdditionalMarks));

            Assert.That(qCumulative2BelowRule2.Question, Is.EqualTo(QCumulative2));
            Assert.That(qCumulative2BelowRule2.AdditionalMarks, Is.EqualTo(Bulletin.AdditionalMarks));

            ///

            var qCumulative1BelowRule3 = (QCumulativeTableRule)qCumulative1BelowRule2.DependantRule;
            var qCumulative2BelowRule3 = (QCumulativeTableRule)qCumulative2BelowRule2.DependantRule;

            Assert.That(qCumulative1BelowRule3.Question, Is.EqualTo(QCumulative1));
            Assert.That(qCumulative1BelowRule3.AdditionalMarks, Is.EqualTo(Bulletin.AdditionalMarks));


            Assert.That(qCumulative2BelowRule3.Question, Is.EqualTo(QCumulative2));
            Assert.That(qCumulative2BelowRule3.AdditionalMarks, Is.EqualTo(Bulletin.AdditionalMarks));

            ///

            var qCumulative1BelowRule4 = (QCumulativeFractionDistributionRule)qCumulative1BelowRule3.DependantRule;
            var qCumulative2BelowRule4 = (QCumulativeFractionDistributionRule)qCumulative2BelowRule3.DependantRule;

            Assert.That(qCumulative1BelowRule4.Question, Is.EqualTo(QCumulative1));
            Assert.That(qCumulative2BelowRule4.Question, Is.EqualTo(QCumulative2));

            ///

            var qCumulative1BelowRule5 = (AdrMustHaveExactVotesSpreadedWithMinimalSpredForQCum)qCumulative1BelowRule4.DependantRule;
            var qCumulative2BelowRule5 = (AdrMustHaveExactVotesSpreadedWithMinimalSpredForQCum)qCumulative2BelowRule4.DependantRule;

            Assert.That(qCumulative1BelowRule5.QCumulative, Is.EqualTo(QCumulative1));
            Assert.That(qCumulative2BelowRule5.QCumulative, Is.EqualTo(QCumulative2));

            ///

            var qCumulative1BelowRule6 = (QCumulativeWarningRule)qCumulative1BelowRule5.DependantRule;
            var qCumulative2BelowRule6 = (QCumulativeWarningRule)qCumulative2BelowRule5.DependantRule;

            Assert.That(qCumulative1BelowRule6.Question, Is.EqualTo(QCumulative1));
            Assert.That(qCumulative1BelowRule6.AdditionalMarks, Is.EqualTo(Bulletin.AdditionalMarks));
            Assert.That(qCumulative1BelowRule6.DependantRule, Is.Null);


            Assert.That(qCumulative2BelowRule6.Question, Is.EqualTo(QCumulative2));
            Assert.That(qCumulative2BelowRule6.AdditionalMarks, Is.EqualTo(Bulletin.AdditionalMarks));
            Assert.That(qCumulative2BelowRule6.DependantRule, Is.Null);
        }

        protected QSimple QSimple1 { get; set; }
        protected QSimple QSimple2 { get; set; }
        protected QSimple QSimple3 { get; set; }
        protected QCumulative QCumulative1 { get; set; }
        protected QCumulative QCumulative2 { get; set; }
        protected QSeparate QSeparate1 { get; set; }
        protected QSeparate QSeparate2 { get; set; }
        protected Page Page1 { get; set; }
        protected Page Page2 { get; set; }
        protected Page Page3 { get; set; }
        protected Signature Signature1 { get; set; }
        protected Signature Signature2 { get; set; }
        protected Signature Signature3 { get; set; }
        protected BulletinScreenModelBase Bulletin { get; private set; }
        protected bool TrustExists { get; private set; }
        protected FractionLong AllowedVotesAmount { get; private set; }
        protected PackStatus PackStatus { get; private set; }

        public void InitBulletin()
        {
            Page1 = new Page(1);
            Page2 = new Page(2);
            Page3 = new Page(3);


            var questions = new List<Question>();
            QSimple1 = TestBulletinModel.GetFreshQSimple(Page1);
            QSimple2 = TestBulletinModel.GetFreshQSimple(Page1);
            QSimple3 = TestBulletinModel.GetFreshQSimple(Page1);
            questions.Add(QSimple1);
            questions.Add(QSimple2);
            questions.Add(QSimple3);


            QCumulative1 = TestBulletinModel.GetFreshQCumulative(Page2);
            QCumulative2 = TestBulletinModel.GetFreshQCumulative(Page2);
            questions.Add(QCumulative1);
            questions.Add(QCumulative2);

            QSeparate1 = TestBulletinModel.GetFreshQSeparate(Page3);
            QSeparate2 = TestBulletinModel.GetFreshQSeparate(Page3);
            questions.Add(QSeparate1);
            questions.Add(QSeparate2);

            const bool packetIsRegistered = true;

            Signature1 = new Signature(Fields.CheckBoxDocField(true, Page1), 1, 1, new List<Question> { QSimple1, QSimple2, QSimple3, QSeparate1 });
            Signature2 = new Signature(Fields.CheckBoxDocField(true, Page2), 2, 2, new List<Question> { QSeparate2, QCumulative1 });
            Signature3 = new Signature(Fields.CheckBoxDocField(true, Page3), 3, 3, new List<Question> { QCumulative2 });

            var signatures = new List<Signature> {Signature1, Signature2, Signature3};

            Bulletin = TestBulletinModel.Create(questions, signatures, AllowedVotesAmount, PackStatus, TrustExists, packetIsRegistered);

            var graph = GraphProvider.BuildGraph(Bulletin);
            new GraphWalker().WalkThrough(graph, Bulletin);
        }

    }
}