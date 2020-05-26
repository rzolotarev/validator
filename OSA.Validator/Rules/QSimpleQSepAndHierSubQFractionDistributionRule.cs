using System;
using System.Collections.Generic;
using System.Linq;
using OSA.Core.Entities.Registration;
using OSA.Editor.ViewModels.BulletinViewModel.Questions;
using OSA.Editor.ViewModels.Fields;
using OSA.Validator.FractionDistribution;
using OSA.Validator.Visitors;

namespace OSA.Validator.Rules
{
    public class QSimpleQSepAndHierSubQFractionDistributionRule:RuleNode
    {
        private readonly SellerFractionsDistributionAnalyzer _sellerFractionsDistributionAnalyzer = new SellerFractionsDistributionAnalyzer();
        private readonly BaseYesNoAbsPoint _point;
        private readonly Func<List<FractionLong>> _allowedVotesAmountFractions;
        private readonly bool _isADR;
        private readonly Func<bool> _hasInstructions;

        public QSimpleQSepAndHierSubQFractionDistributionRule(BaseYesNoAbsPoint point, Func<List<FractionLong>> allowedVotesAmountFractions, bool isADR, Func<bool> hasInstructions)
        {
            _point = point;
            _hasInstructions = hasInstructions;
            _isADR = isADR;
            _allowedVotesAmountFractions = allowedVotesAmountFractions;
        }

        public override bool IsFulfiled
        {
            get
            {
                if (IsADRSpecialCase()) return true;
                var fieldsWithFractionParts = GetActiveVoteFields();
                var fieldValues = fieldsWithFractionParts.Select(x => x.Value).ToList();
                var fractionParts = _allowedVotesAmountFractions().Select(x=>x.FractionPart).ToList();
                return _sellerFractionsDistributionAnalyzer.FractionDistributionIsPossible(fieldValues, fractionParts);
            }
        }

        private bool IsADRSpecialCase()
        {
            return _isADR && _hasInstructions();
        }

        private IEnumerable<VotesDocField> GetActiveVoteFields()
        {
            var votesFieldsWithChecks = new List<VotesDocField>();
            if (Point.YesCheckBoxField.Value) votesFieldsWithChecks.Add(Point.YesVotesField);
            if (Point.NoCheckBoxField.Value) votesFieldsWithChecks.Add(Point.NoVotesField);
            if (Point.AbstainedCheckBoxField.Value) votesFieldsWithChecks.Add(Point.AbstainedVotesField);

            return votesFieldsWithChecks;
        }

        public override bool ShouldGoFurtherDownTheGraph
        {
            get { return IsFulfiled; }
        }

        public override IList<DocField> DependsOn
        {
            get { return GetActiveVoteFields().Where(x=>x.Value.HasFractionPart).Cast<DocField>().ToList(); }
        }

        public BaseYesNoAbsPoint Point
        {
            get { return _point; }
        }

        public override void Accept(IGraphVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string GetErrorText()
        {
            return ErrorTexts.CANNOT_SPLIT_FRACTIONS_THIS_WAY;
        }
    }
}