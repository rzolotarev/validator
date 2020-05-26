using System;
using System.Collections.Generic;
using System.Linq;
using OSA.Core.Entities.Registration;
using OSA.Editor.ViewModels.BulletinViewModel.Bulletin;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Cumulative;
using OSA.Editor.ViewModels.Fields;
using OSA.Validator.Rules.TablesRules.Enums;
using OSA.Validator.Visitors;

namespace OSA.Validator.Rules.TablesRules.QCumulativeTableRules
{
    public class QCumulativeTableRule : RuleNode
    {
        private readonly Func<PackStatus> _packStatus;
        private readonly Func<bool> _trustExists;
        private readonly Func<AmountOfStockSubmited> _amountOfStockSubmited;
        private readonly Func<NumberOfChecks> _numberOfChecks;
        private readonly Func<AdditionalChecks> _additionalChecks;
        private readonly Func<CumYesIs> _cumChecks;

        public QCumulative Question { get; set; }
        public BulletinAdditionalMarks AdditionalMarks { get; set; }

        public QCumulativeTableRule(QCumulative qCumulative, BulletinAdditionalMarks additionalMarks, Func<PackStatus> packStatus, Func<bool> trustExists, Func<AmountOfStockSubmited> amountOfStockSubmited, Func<NumberOfChecks> numberOfChecks, Func<AdditionalChecks> additionalChecks, Func<CumYesIs> cumChecks)
        {
            _packStatus = packStatus;
            _trustExists = trustExists;
            _amountOfStockSubmited = amountOfStockSubmited;
            _numberOfChecks = numberOfChecks;
            _additionalChecks = additionalChecks;
            _cumChecks = cumChecks;
            Question = qCumulative;
            AdditionalMarks = additionalMarks;
        }

        private readonly QCumulativeTableRuleIsFulfilledCalculator _calculator = new QCumulativeTableRuleIsFulfilledCalculator();



        public override bool IsFulfiled
        {
            get
            {
                return _calculator.GetIsFulfilledFor(_packStatus(), _additionalChecks(), _amountOfStockSubmited(),
                                                     _numberOfChecks(), _cumChecks(), _trustExists());
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
                var questionFields = new List<DocField>
                                         {
                                             Question.AdditionalMarks.YesCheckBoxField,
                                             Question.AdditionalMarks.NoCheckBoxField,
                                             Question.AdditionalMarks.NoVotesField,
                                             Question.AdditionalMarks.AbstainedCheckBoxField,
                                             Question.AdditionalMarks.AbstainedVotesField
                                         }
                    .Union(Question.CandidatePoints.Select(p => p.VotesDocField))
                    .ToList();
                var additionalMarksFields = new List<DocField>
                                                {
                                                    AdditionalMarks.HasInstructions,
                                                    AdditionalMarks.HasTrust,
                                                    AdditionalMarks.NotWholeStockWasPassed
                                                };
                return questionFields.Union(additionalMarksFields).ToList();
            }
        }

        public override void Accept(IGraphVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string GetErrorText()
        {
            return _calculator.GetErrorTextFor(_packStatus(), _additionalChecks(), _amountOfStockSubmited(),
                                                 _numberOfChecks(), _cumChecks(), _trustExists());
        }
    }
}