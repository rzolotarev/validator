using System;
using System.Collections.Generic;
using OSA.Core.Util.Extensions.IEnumerables;
using OSA.Editor.ViewModels.BulletinViewModel.Bulletin;
using OSA.Editor.ViewModels.BulletinViewModel.Questions;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Cumulative;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Separate;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Simple;
using OSA.Editor.ViewModels.Fields;

namespace OSA.Validator
{
    public class VotesFieldsWasntCheckedSetter
    {
        public void SetVoteFieldsThatDontHaveCorrespondingChecksSetAsWasntChecked(BulletinScreenModelBase bulletin)
        {
            bulletin
                .Questions
                .ForEach(SetVoteFieldsThatDontHaveCorrespondingChecksSetAsWasntChecked);
        }

        private void SetVoteFieldsThatDontHaveCorrespondingChecksSetAsWasntChecked(Question question)
        {
            if (question is QSimple)
            {
                SetVoteFieldsThatDontHaveCorrespondingChecksSetAsWasntChecked((QSimple)question);
            }
            else if (question is QCumulative)
            {
                SetVoteFieldsThatDontHaveCorrespondingChecksSetAsWasntChecked((QCumulative)question);
            }
            else if (question is QSeparate)
            {
                SetVoteFieldsThatDontHaveCorrespondingChecksSetAsWasntChecked((QSeparate) question);
            }
            else
            {
                throw new NotSupportedException("Тип " + question.GetType() + " не поддерживается.");
            }
        }


        private static void SetVoteFieldsThatDontHaveCorrespondingChecksSetAsWasntChecked(BaseYesNoAbsPoint point)
        {
            var additionalChecks = ((BulletinScreenModelBase)point.ParentRootModel).AdditionalMarks;
            var additionalChecksAreUsed = additionalChecks.HasInstructions.Value || 
                                                          additionalChecks.NotWholeStockWasPassed.Value ||
                                                          additionalChecks.HasTrust.Value;

            var pairs = new Dictionary<CheckBoxDocField, VotesDocField>
                {
                    {point.YesCheckBoxField, point.YesVotesField},
                    {point.NoCheckBoxField, point.NoVotesField},
                    {point.AbstainedCheckBoxField, point.AbstainedVotesField},
                };

            pairs.ForEach(p =>
                {
                    if (p.Key.Value == false || !additionalChecksAreUsed) p.Value.ErrorLevel = ErrorLevel.WasntChecked;
                });
        }

        private static void SetVoteFieldsThatDontHaveCorrespondingChecksSetAsWasntChecked(QCumulative question)
        {
            if (question.AdditionalMarks.YesCheckBoxField.Value == false)
            {
                question.CandidatePoints.ForEach(cp => cp.VotesDocField.ErrorLevel = ErrorLevel.WasntChecked);
            }

            if (question.AdditionalMarks.NoCheckBoxField.Value == false)
                question.AdditionalMarks.NoVotesField.ErrorLevel = ErrorLevel.WasntChecked;

            if (question.AdditionalMarks.AbstainedCheckBoxField.Value == false)
                question.AdditionalMarks.AbstainedVotesField.ErrorLevel = ErrorLevel.WasntChecked;
        }

        private static void SetVoteFieldsThatDontHaveCorrespondingChecksSetAsWasntChecked(QSeparate question)
        {
            question.CandidatePoints.ForEach(SetVoteFieldsThatDontHaveCorrespondingChecksSetAsWasntChecked);
        }
        private static void SetVoteFieldsThatDontHaveCorrespondingChecksSetAsWasntChecked(QSimple question)
        {
            SetVoteFieldsThatDontHaveCorrespondingChecksSetAsWasntChecked(question.Point);
        }
    }
}