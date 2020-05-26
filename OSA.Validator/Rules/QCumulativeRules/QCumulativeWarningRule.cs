using System;
using System.Collections.Generic;
using OSA.Core.Entities.Registration;
using OSA.Core.Util.Extensions.IEnumerables;
using OSA.Editor.ViewModels.AmountOfVotesCalculators;
using OSA.Editor.ViewModels.BulletinViewModel.Bulletin;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Cumulative;
using OSA.Editor.ViewModels.Fields;
using OSA.Validator.Visitors;

namespace OSA.Validator.Rules.QCumulativeRules
{
    public class QCumulativeWarningRule :RuleNode
    {
        private static readonly ExactAmountOfStockSubmitedCalulator _exactAmountOfStockSubmitedCalulator = new ExactAmountOfStockSubmitedCalulator();
        
        public QCumulative Question { get; private set;}
        public BulletinAdditionalMarks AdditionalMarks { get; private set; }
        private readonly Func<FractionLong> _allowedVotesAmount;

        public QCumulativeWarningRule(QCumulative question, BulletinAdditionalMarks additionalMarks, Func<FractionLong> allowedVotesAmount)
        {
            _allowedVotesAmount = allowedVotesAmount;
            Question = question;
            AdditionalMarks = additionalMarks;
        }

        public override bool IsFulfiled
        {
            get 
            {
                var candidatesSum = Question.CandidatePoints.Sum(c => _exactAmountOfStockSubmitedCalulator.GetVotesForCandidateField(Question, c));
                var noVotes = _exactAmountOfStockSubmitedCalulator.GetVotesForNoField(Question, _allowedVotesAmount(), AdditionalMarks);
                var absVotes = _exactAmountOfStockSubmitedCalulator.GetVotesForAbstainedField(Question, _allowedVotesAmount(), AdditionalMarks);

                return _allowedVotesAmount() == candidatesSum + noVotes + absVotes;
            }
        }


        public override bool ShouldGoFurtherDownTheGraph
        {
            get { return IsFulfiled; }
        }

        public override IList<DocField> DependsOn
        {
            get 
            {
                var result = new List<DocField>();
                Question.CandidatePoints.ForEach(c=> result.Add(c.VotesDocField));
                result.Add(Question.AdditionalMarks.NoVotesField);
                result.Add(Question.AdditionalMarks.AbstainedVotesField);

                result.Add(Question.AdditionalMarks.YesCheckBoxField);
                result.Add(Question.AdditionalMarks.NoCheckBoxField);
                result.Add(Question.AdditionalMarks.AbstainedCheckBoxField);
                return result;
            }
        }

        public override void Accept(IGraphVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string GetErrorText()
        {
            return ErrorTexts.NOT_ALL_VOTES_WERE_DIVIDED_AMONG_FIELDS;
        }
    }
}