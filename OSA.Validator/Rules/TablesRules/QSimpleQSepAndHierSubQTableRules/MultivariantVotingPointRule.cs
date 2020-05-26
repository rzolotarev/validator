using System;
using System.Collections.Generic;
using OSA.Editor.ViewModels.BulletinViewModel.Bulletin;
using OSA.Editor.ViewModels.BulletinViewModel.Questions;
using OSA.Editor.ViewModels.Fields;
using OSA.Validator.Rules.TablesRules.Enums;
using OSA.Validator.Visitors;

namespace OSA.Validator.Rules.TablesRules.QSimpleQSepAndHierSubQTableRules
{
    public class MultivariantVotingPointRule:RuleNode
    {
        private readonly Func<NumberOfChecks> _numberOfChecks;
        private readonly Func<AdditionalChecks> _additionalChecks;
        public BaseYesNoAbsPoint Point { get; set; }
        public BulletinAdditionalMarks AdditionalMarks { get; set; }

        public MultivariantVotingPointRule(BaseYesNoAbsPoint point, BulletinAdditionalMarks additionalMarks, Func<NumberOfChecks> numberOfChecks, Func<AdditionalChecks> additionalChecks)
        {
            _numberOfChecks = numberOfChecks;
            _additionalChecks = additionalChecks;
            Point = point;
            AdditionalMarks = additionalMarks;
        }

        public override bool IsFulfiled
        {
            get
            {
                
                if (_numberOfChecks() == NumberOfChecks.Multiple && !MultiVariantVotingAllowanceInterpreter.IsMultiVariantVotingAllowed(_additionalChecks()))
                {
                    return false;
                }
                return true;
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

                               AdditionalMarks.HasInstructions,
                               AdditionalMarks.HasTrust,
                               AdditionalMarks.NotWholeStockWasPassed
                           };
            }
        }

        public override void Accept(IGraphVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string GetErrorText()
        {
            return ErrorTexts.MULTIVARIANT_VOTING_IS_PROHIBITED;
        }
    }
}