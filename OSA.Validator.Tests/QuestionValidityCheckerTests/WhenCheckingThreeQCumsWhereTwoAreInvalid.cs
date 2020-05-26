using System.Collections.Generic;
using NUnit.Framework;
using OSA.Core.Entities.Registration;
using OSA.Editor.ViewModels.BulletinViewModel.Questions;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Cumulative;
using OSA.Editor.ViewModels.Fields;
using OSA.Editor.ViewModels.ValidityChecking;

namespace OSA.Validator.Tests.QuestionValidityCheckerTests
{
    [TestFixture]
    public class WhenCheckingThreeQCumsWhereTwoAreInvalid
    {
        private IList<QuestionWithVotes> _invalidQuestions;
        private QCumulative _invalidQCum1;
        private QCumulative _validQCum;
        private QCumulative _invalidQCum2;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _invalidQCum1 = GetQCumulative(candidate1ErrorLevel: ErrorLevel.WasntChecked, 
                           candidate2ErrorLevel:ErrorLevel.Error, 
                           candidate3ErrorLevel:ErrorLevel.Valid, 
                           candidate4ErrorLevel:ErrorLevel.Valid, 
                           candidate5ErrorLevel:ErrorLevel.WasntChecked, 

                           yesCheckBoxErrorLevel:ErrorLevel.Valid, 
                           noCheckBoxErrorLevel:ErrorLevel.WasntChecked, 
                           noVotesErrorLevel:ErrorLevel.Valid, 
                           abstainedCheckBoxErrorLevel:ErrorLevel.WasntChecked, 
                           abstainedVotesErrorLevel:ErrorLevel.Valid);


            _validQCum = GetQCumulative(candidate1ErrorLevel: ErrorLevel.WasntChecked,
                           candidate2ErrorLevel: ErrorLevel.WasntChecked,
                           candidate3ErrorLevel: ErrorLevel.Valid,
                           candidate4ErrorLevel: ErrorLevel.Valid,
                           candidate5ErrorLevel: ErrorLevel.WasntChecked,

                           yesCheckBoxErrorLevel: ErrorLevel.Valid,
                           noCheckBoxErrorLevel: ErrorLevel.WasntChecked,
                           noVotesErrorLevel: ErrorLevel.Valid,
                           abstainedCheckBoxErrorLevel: ErrorLevel.WasntChecked,
                           abstainedVotesErrorLevel: ErrorLevel.Valid);



            _invalidQCum2 = GetQCumulative(candidate1ErrorLevel: ErrorLevel.WasntChecked,
                           candidate2ErrorLevel: ErrorLevel.WasntChecked,
                           candidate3ErrorLevel: ErrorLevel.Valid,
                           candidate4ErrorLevel: ErrorLevel.Valid,
                           candidate5ErrorLevel: ErrorLevel.WasntChecked,

                           yesCheckBoxErrorLevel: ErrorLevel.Valid,
                           noCheckBoxErrorLevel: ErrorLevel.WasntChecked,
                           noVotesErrorLevel: ErrorLevel.Valid,
                           abstainedCheckBoxErrorLevel: ErrorLevel.Error,
                           abstainedVotesErrorLevel: ErrorLevel.Valid);

            var questions = new List<Question>
                                {
                                    _invalidQCum1, _validQCum, _invalidQCum2
                                };
            var bulletin = TestBulletinModel.Create(questions);

            var checker = new QuestionValidityChecker(new CandidateValidityChecker(), new AdrQuestionValidityChecker());
            _invalidQuestions = checker.GetInvalidQuestions(bulletin);
        }

        private QCumulative GetQCumulative(ErrorLevel candidate1ErrorLevel, ErrorLevel candidate2ErrorLevel, ErrorLevel candidate3ErrorLevel, ErrorLevel candidate4ErrorLevel, ErrorLevel candidate5ErrorLevel, ErrorLevel yesCheckBoxErrorLevel, ErrorLevel noCheckBoxErrorLevel, ErrorLevel noVotesErrorLevel, ErrorLevel abstainedCheckBoxErrorLevel, ErrorLevel abstainedVotesErrorLevel)
        {
            #region doesnt matter constants

            const bool doesntmatter = true;
            var doesntmatter2 = FractionLong.Zero;
            const int doesntMatter5 = 1;
            #endregion


            return new QCumulative(
                doesntMatter5,
                new List<QCumCandidatePoint>
                    {
                        new QCumCandidatePoint(doesntMatter5, Fields.VotesDocField(doesntmatter2, candidate1ErrorLevel)),
                        new QCumCandidatePoint(doesntMatter5, Fields.VotesDocField(doesntmatter2, candidate2ErrorLevel)),
                        new QCumCandidatePoint(doesntMatter5, Fields.VotesDocField(doesntmatter2, candidate3ErrorLevel)),
                        new QCumCandidatePoint(doesntMatter5, Fields.VotesDocField(doesntmatter2, candidate4ErrorLevel)),
                        new QCumCandidatePoint(doesntMatter5, Fields.VotesDocField(doesntmatter2, candidate5ErrorLevel))
                    },
                new QCumulativeAdditionalMarks
                    (
                    Fields.CheckBoxDocField(doesntmatter, yesCheckBoxErrorLevel),
                    Fields.CheckBoxDocField(doesntmatter, noCheckBoxErrorLevel),
                    Fields.VotesDocField(doesntmatter2, abstainedCheckBoxErrorLevel),
                    Fields.CheckBoxDocField(doesntmatter, noVotesErrorLevel),
                    Fields.VotesDocField(doesntmatter2, abstainedVotesErrorLevel))
                );
        }

        [Test]
        public void Should_find_2_invalid_qCums()
        {
            Assert.That(_invalidQuestions.Count, Is.EqualTo(2));
            Assert.That(_invalidQuestions.Contains(_invalidQCum1));
            Assert.That(_invalidQuestions.Contains(_invalidQCum2));
        }
    }
}