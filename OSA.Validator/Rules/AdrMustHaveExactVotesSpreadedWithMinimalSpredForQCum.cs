using System;
using System.Collections.Generic;
using OSA.Core.Entities.Registration;
using OSA.Core.Util.Extensions.IEnumerables;
using OSA.Editor.ViewModels.AmountOfVotesCalculators;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Cumulative;
using OSA.Editor.ViewModels.Fields;
using OSA.Editor.ViewModels.ValidityChecking;
using OSA.Validator.Visitors;

namespace OSA.Validator.Rules
{
    public class AdrMustHaveExactVotesSpreadedWithMinimalSpredForQCum : RuleNode
    {
        private static readonly ExactAmountOfStockSubmitedCalulator _exactAmountOfStockSubmitedCalulator = new ExactAmountOfStockSubmitedCalulator();
        private static readonly AdrQuestionValidityChecker _adrQuestionValidityChecker = new AdrQuestionValidityChecker();

        private readonly Func<FractionLong> _allowedVotesAmount;
        private readonly bool _isADRBulletin;
        public QCumulative QCumulative { get; private set; }

        public AdrMustHaveExactVotesSpreadedWithMinimalSpredForQCum(QCumulative qCumulative, Func<FractionLong> allowedVotesAmount, bool isADRBulletin)
        {
            _allowedVotesAmount = allowedVotesAmount;
            _isADRBulletin = isADRBulletin;
            QCumulative = qCumulative;
        }

        public override bool IsFulfiled
        {
            get
            {
                if (!_isADRBulletin) return true;

                return _adrQuestionValidityChecker.QCumulativeIsValid(QCumulative, _allowedVotesAmount());
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
                QCumulative.CandidatePoints.ForEach(c => result.Add(c.VotesDocField));
                result.Add(QCumulative.AdditionalMarks.NoVotesField);
                result.Add(QCumulative.AdditionalMarks.AbstainedVotesField);

                result.Add(QCumulative.AdditionalMarks.YesCheckBoxField);
                result.Add(QCumulative.AdditionalMarks.NoCheckBoxField);
                result.Add(QCumulative.AdditionalMarks.AbstainedCheckBoxField);
                return result;
            }
        }

        public override void Accept(IGraphVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string GetErrorText()
        {
            return ErrorTexts.ADR_MUST_HAVE_EXACT_VOTES_AMOUNT_SPREADED;
        }
    }
}