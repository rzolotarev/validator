using System.Collections.Generic;
using NUnit.Framework;
using OSA.Core.Util.Extensions.FractionLongs;
using OSA.Editor.ViewModels.BulletinViewModel.Questions;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Separate;
using OSA.Editor.ViewModels.Fields;
using OSA.Editor.ViewModels.ValidityChecking;

namespace OSA.Validator.Tests.QuestionValidityCheckerTests
{
    [TestFixture]
    public class WhenCheckingThreeQSeparatesWhereTwoAreInvalid
    {
        private IList<QuestionWithVotes> _invalidQuestions;
        private QSeparate _invalidQSep;
        private QSeparate _validQSep;
        private QSeparate _invalidQSep2;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _invalidQSep = GetQSeparate(candidate1YesCheckErrorLevel: ErrorLevel.Valid,
                                        candidate1NoCheckErrorLevel: ErrorLevel.Valid,
                                        candidate1AbsCheckErrorLevel: ErrorLevel.Error,
                                        candidate1YesVotesErrorLevel: ErrorLevel.WasntChecked,
                                        candidate1NoVotesErrorLevel: ErrorLevel.WasntChecked,
                                        candidate1AbsVotesErrorLevel: ErrorLevel.WasntChecked,

                                        candidate2YesCheckErrorLevel: ErrorLevel.Error,
                                        candidate2NoCheckErrorLevel: ErrorLevel.Error,
                                        candidate2AbsCheckErrorLevel: ErrorLevel.Valid,
                                        candidate2YesVotesErrorLevel: ErrorLevel.WasntChecked,
                                        candidate2NoVotesErrorLevel: ErrorLevel.WasntChecked,
                                        candidate2AbsVotesErrorLevel: ErrorLevel.WasntChecked);

            _validQSep = GetQSeparate(candidate1YesCheckErrorLevel: ErrorLevel.Valid,
                                        candidate1NoCheckErrorLevel: ErrorLevel.Valid,
                                        candidate1AbsCheckErrorLevel: ErrorLevel.Valid,
                                        candidate1YesVotesErrorLevel: ErrorLevel.WasntChecked,
                                        candidate1NoVotesErrorLevel: ErrorLevel.WasntChecked,
                                        candidate1AbsVotesErrorLevel: ErrorLevel.WasntChecked,

                                        candidate2YesCheckErrorLevel: ErrorLevel.Valid,
                                        candidate2NoCheckErrorLevel: ErrorLevel.WasntChecked,
                                        candidate2AbsCheckErrorLevel: ErrorLevel.Valid,
                                        candidate2YesVotesErrorLevel: ErrorLevel.WasntChecked,
                                        candidate2NoVotesErrorLevel: ErrorLevel.WasntChecked,
                                        candidate2AbsVotesErrorLevel: ErrorLevel.WasntChecked);

            _invalidQSep2 = GetQSeparate(candidate1YesCheckErrorLevel: ErrorLevel.Valid,
                                        candidate1NoCheckErrorLevel: ErrorLevel.Valid,
                                        candidate1AbsCheckErrorLevel: ErrorLevel.Error,
                                        candidate1YesVotesErrorLevel: ErrorLevel.WasntChecked,
                                        candidate1NoVotesErrorLevel: ErrorLevel.WasntChecked,
                                        candidate1AbsVotesErrorLevel: ErrorLevel.WasntChecked,

                                        candidate2YesCheckErrorLevel: ErrorLevel.Valid,
                                        candidate2NoCheckErrorLevel: ErrorLevel.WasntChecked,
                                        candidate2AbsCheckErrorLevel: ErrorLevel.Valid,
                                        candidate2YesVotesErrorLevel: ErrorLevel.WasntChecked,
                                        candidate2NoVotesErrorLevel: ErrorLevel.Error,
                                        candidate2AbsVotesErrorLevel: ErrorLevel.WasntChecked);

            var questions = new List<Question> { _invalidQSep, _validQSep, _invalidQSep2 };

            var bulletin = TestBulletinModel.Create(questions);

            var checker = new QuestionValidityChecker(new CandidateValidityChecker(), new AdrQuestionValidityChecker());
            _invalidQuestions = checker.GetInvalidQuestions(bulletin);
        }

        private QSeparate GetQSeparate(ErrorLevel candidate1YesCheckErrorLevel,
                                       ErrorLevel candidate1NoCheckErrorLevel,
                                       ErrorLevel candidate1AbsCheckErrorLevel,
                                       ErrorLevel candidate1YesVotesErrorLevel,
                                       ErrorLevel candidate1NoVotesErrorLevel,
                                       ErrorLevel candidate1AbsVotesErrorLevel,
                                       ErrorLevel candidate2YesCheckErrorLevel,
                                       ErrorLevel candidate2NoCheckErrorLevel,
                                       ErrorLevel candidate2AbsCheckErrorLevel,
                                       ErrorLevel candidate2YesVotesErrorLevel,
                                       ErrorLevel candidate2NoVotesErrorLevel,
                                       ErrorLevel candidate2AbsVotesErrorLevel)
        {
            var candidatePoint1 = GetCandidatePoint(candidate1YesCheckErrorLevel, candidate1NoCheckErrorLevel, candidate1AbsCheckErrorLevel, candidate1YesVotesErrorLevel, candidate1NoVotesErrorLevel, candidate1AbsVotesErrorLevel);
            var candidatePoint2 = GetCandidatePoint(candidate2YesCheckErrorLevel, candidate2NoCheckErrorLevel, candidate2AbsCheckErrorLevel, candidate2YesVotesErrorLevel, candidate2NoVotesErrorLevel, candidate2AbsVotesErrorLevel);
            var candidatePoints = new List<QSepCandidatePoint> { candidatePoint1, candidatePoint2 };
            return new QSeparate(1, candidatePoints);
        }

        private static QSepCandidatePoint GetCandidatePoint(ErrorLevel candidateYesCheckErrorLevel, ErrorLevel candidateNoCheckErrorLevel, ErrorLevel candidateAbsCheckErrorLevel, ErrorLevel candidateYesVotesErrorLevel, ErrorLevel candidateNoVotesErrorLevel, ErrorLevel candidateAbsVotesErrorLevel)
        {
            const bool doesntMatter = false;
            var doesntMatter2 = 0.FL();

            return new QSepCandidatePoint(1,
                                          new BaseYesNoAbsPoint(
                                              Fields.CheckBoxDocField(doesntMatter, candidateYesCheckErrorLevel),
                                              Fields.CheckBoxDocField(doesntMatter, candidateNoCheckErrorLevel),
                                              Fields.CheckBoxDocField(doesntMatter, candidateAbsCheckErrorLevel),
                                              Fields.VotesDocField(doesntMatter2, candidateYesVotesErrorLevel),
                                              Fields.VotesDocField(doesntMatter2, candidateNoVotesErrorLevel),
                                              Fields.VotesDocField(doesntMatter2, candidateAbsVotesErrorLevel)));
        }

        [Test]
        public void Should_find_2_invalid_qSeps()
        {
            Assert.That(_invalidQuestions.Count, Is.EqualTo(2));
            Assert.That(_invalidQuestions.Contains(_invalidQSep));
            Assert.That(_invalidQuestions.Contains(_invalidQSep2));
        }
    }
}