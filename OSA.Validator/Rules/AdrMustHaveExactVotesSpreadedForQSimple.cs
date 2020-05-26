using System;
using System.Collections.Generic;
using OSA.Core.Entities.Registration;
using OSA.Editor.ViewModels.BulletinViewModel.Questions;
using OSA.Editor.ViewModels.Fields;
using OSA.Editor.ViewModels.ValidityChecking;
using OSA.Validator.Visitors;

namespace OSA.Validator.Rules
{
    public class AdrMustHaveExactVotesSpreadedForQSimple : RuleNode
    {
        private readonly Func<FractionLong> _allowedVotesAmount;
        private readonly bool _isADRBulletin;
        private readonly AdrQuestionValidityChecker _adrQuestionValidityChecker = new AdrQuestionValidityChecker();
        public BaseYesNoAbsPoint Point { get; private set; }

        public AdrMustHaveExactVotesSpreadedForQSimple(BaseYesNoAbsPoint point, Func<FractionLong> allowedVotesAmount, bool isADRBulletin)
        {
            _allowedVotesAmount = allowedVotesAmount;
            _isADRBulletin = isADRBulletin;
            Point = point;
        }

        public override bool IsFulfiled
        {
            get 
            { 
                if (!_isADRBulletin) return true;
                return _adrQuestionValidityChecker.PointIsValid(Point, _allowedVotesAmount());
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
                return new List<DocField>
                    {
                        Point.YesCheckBoxField,
                        Point.NoCheckBoxField,
                        Point.AbstainedCheckBoxField,
                        Point.YesVotesField,
                        Point.NoVotesField,
                        Point.AbstainedVotesField,
                    };
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