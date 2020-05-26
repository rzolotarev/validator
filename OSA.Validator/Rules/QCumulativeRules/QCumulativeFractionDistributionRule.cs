using System;
using System.Collections.Generic;
using System.Linq;
using OSA.Core.Entities.Registration;
using OSA.Editor.ViewModels.BulletinViewModel.Questions.Cumulative;
using OSA.Editor.ViewModels.Fields;
using OSA.Validator.FractionDistribution;
using OSA.Validator.Rules.TablesRules.Enums;
using OSA.Validator.Visitors;

namespace OSA.Validator.Rules.QCumulativeRules
{
    public class QCumulativeFractionDistributionRule : RuleNode
    {
        private readonly SellerFractionsDistributionAnalyzer _sellerFractionsDistributionAnalyzer = new SellerFractionsDistributionAnalyzer();
        private readonly Func<List<FractionLong>> _allowedVotesAmountFractions;
        private readonly Func<AdditionalChecks> _additionalChecks;
        private readonly bool _isADR;
        private readonly Func<bool> _hasInstructions;


        public QCumulativeFractionDistributionRule(QCumulative question, Func<List<FractionLong>> allowedVotesAmountFractions, Func<AdditionalChecks> additionalChecks, bool isADR, Func<bool> hasInstructions)
        {
            _allowedVotesAmountFractions = allowedVotesAmountFractions;
            _hasInstructions = hasInstructions;
            _isADR = isADR;
            _additionalChecks = additionalChecks;
            Question = question;
        }

        public QCumulative Question { get; private set; }

        public override bool IsFulfiled
        {
            get
            {
                if (IsADRSpecialCase()) return true;
                if (MultivariantVotingIsntAllowedAndFractionsAreSpreadAmongMoreThanOneCandidate()) return false;


                var fieldsWithFractionParts = GetActiveFields();
                var fieldValues = fieldsWithFractionParts.Select(x => x.Value).ToList();
                var fractionParts = _allowedVotesAmountFractions().Select(x => x.FractionPart).ToList();
                return _sellerFractionsDistributionAnalyzer.FractionDistributionIsPossible(fieldValues, fractionParts);
            }
        }

        private bool IsADRSpecialCase()
        {
            return _isADR && _hasInstructions();
        }

        private bool MultivariantVotingIsntAllowedAndFractionsAreSpreadAmongMoreThanOneCandidate()
        {
            var candidateFieldsWithFractions = GetCandidateFields().Where(x => x.Value.HasFractionPart).ToList();
            var multivariantVotingIsntAllowed = !MultiVariantVotingAllowanceInterpreter.IsMultiVariantVotingAllowed(_additionalChecks());
            return candidateFieldsWithFractions.Count > 1 && multivariantVotingIsntAllowed;
        }

        private IEnumerable<VotesDocField> GetActiveFields()
        {
            var fields = new List<VotesDocField>();
            if (Question.AdditionalMarks.YesCheckBoxField.Value)
            {
                fields.AddRange(GetCandidateFields());
            }
            if (Question.AdditionalMarks.NoCheckBoxField.Value)
            {
                fields.Add(Question.AdditionalMarks.NoVotesField);
            }
            if (Question.AdditionalMarks.AbstainedCheckBoxField.Value)
            {
                fields.Add(Question.AdditionalMarks.AbstainedVotesField);
            }
            return fields;
        }

        private List<VotesDocField> GetCandidateFields()
        {
            return Question.CandidatePoints.Select(x => x.VotesDocField).ToList();
        }

        public override bool ShouldGoFurtherDownTheGraph
        {
            get { return IsFulfiled; }
        }

        public override IList<DocField> DependsOn
        {
            get { return GetActiveFields().Cast<DocField>().ToList(); }
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