using System;
using System.Collections.Generic;
using NUnit.Framework;
using OSA.Core.Entities.Registration;
using OSA.Editor.ViewModels.BulletinViewModel.Questions;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Simple;
using OSA.Editor.ViewModels.Fields;
using OSA.Editor.ViewModels.ValidityChecking;

namespace OSA.Validator.Tests.QuestionValidityCheckerTests
{
    [TestFixture]
    public class WhenCheckingThreeQSimplesWhereTwoAreInvalid
    {
        private IList<QuestionWithVotes> _invalidQuestions;
        private QSimple _invalidQSimple1;
        private QSimple _validQSimple;
        private QSimple _invalidQSimple2;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            #region doesnt matter constants
            const bool doesntmatter = true;
            var doesntmatter2 = FractionLong.Zero;
            #endregion

            _invalidQSimple1 = new QSimple(1,
                new BaseYesNoAbsPoint(
                    Fields.CheckBoxDocField(doesntmatter, ErrorLevel.Error),
                    Fields.CheckBoxDocField(doesntmatter, ErrorLevel.Error),
                    Fields.CheckBoxDocField(doesntmatter, ErrorLevel.Valid),
                    Fields.VotesDocField(doesntmatter2),
                    Fields.VotesDocField(doesntmatter2),
                    Fields.VotesDocField(doesntmatter2)
                    ));
            _validQSimple = new QSimple(2,
                new BaseYesNoAbsPoint(
                    Fields.CheckBoxDocField(doesntmatter, ErrorLevel.WasntChecked),
                    Fields.CheckBoxDocField(doesntmatter, ErrorLevel.Valid),
                    Fields.CheckBoxDocField(doesntmatter, ErrorLevel.Valid),
                    Fields.VotesDocField(doesntmatter2, ErrorLevel.WasntChecked),
                    Fields.VotesDocField(doesntmatter2, ErrorLevel.Valid),
                    Fields.VotesDocField(doesntmatter2, ErrorLevel.WasntChecked)
                    ));
            _invalidQSimple2 = new QSimple(3,
                new BaseYesNoAbsPoint(
                    Fields.CheckBoxDocField(doesntmatter, ErrorLevel.Valid),
                    Fields.CheckBoxDocField(doesntmatter, ErrorLevel.WasntChecked),
                    Fields.CheckBoxDocField(doesntmatter, ErrorLevel.Valid),
                    Fields.VotesDocField(doesntmatter2, ErrorLevel.Error),
                    Fields.VotesDocField(doesntmatter2, ErrorLevel.WasntChecked),
                    Fields.VotesDocField(doesntmatter2, ErrorLevel.WasntChecked)
                    ));

            var bulletin = TestBulletinModel.Create(new List<Question>
                                            {
                                                _invalidQSimple1,                                     
                                                _validQSimple,
                                                _invalidQSimple2,
                                            });

            var checker = new QuestionValidityChecker(new CandidateValidityChecker(), new AdrQuestionValidityChecker());
            _invalidQuestions = checker.GetInvalidQuestions(bulletin);
        }

        [Test]
        public void Should_find_2_invalid_qsimples()
        {
            Assert.That(_invalidQuestions.Count, Is.EqualTo(2));
            Assert.That(_invalidQuestions.Contains(_invalidQSimple1));
            Assert.That(_invalidQuestions.Contains(_invalidQSimple2));
        }
    }
}