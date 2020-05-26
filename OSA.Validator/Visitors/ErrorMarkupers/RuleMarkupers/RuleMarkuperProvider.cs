using System;
using System.Collections.Generic;
using OSA.Validator.Rules;
using OSA.Validator.Visitors.ErrorMarkupers.RuleMarkupers.Rules;

namespace OSA.Validator.Visitors.ErrorMarkupers.RuleMarkupers
{
    public class RuleMarkuperProvider
    {
        private  readonly IList<RuleNodeMarkuperBase> _markupers;
        public RuleMarkuperProvider()
        {
            _markupers = new List<RuleNodeMarkuperBase>
                            {
                                new PointShouldHaveAtLeastOneSelectionRuleMarkuper(),
                                new SignatureMustBePresentRuleMarkuper(),
                                new QSimpleQSepAndHierSubQTableRuleMarkuper(),
                                new QSeparateYesChecksShouldNotExceedPlacesCountRuleMarkuper(),
                                new QCumulativeMustHaveAtLeastOneSelectionRuleMarkuper(),
                                new QCumulativeTableRuleMarkuper(),
                                new InCaseOfSimplePackPointWithNoAdditionalMarksAlwaysPassesRuleMarkuper(),
                                new MultivariantVotingPointRuleMarkuper(),
                                new InCaseOfSimplePackQCumulativeWithNoAdditionalMarksAlwaysPassesRuleMarkuper(),
                                new MultivariantVotingQCumulativeRuleMarkuper(),
                                new QSimpleQSepAndHierSubQWarningRuleMarkuper(),
                                new QCumulativeWarningRuleMarkuper(),
                                new OwnersCountSignatureWarningRuleMarkuper(),
                                new PacketMustBeRegisteredRuleMarkuper(),
                                new QSimpleQSepAndHierSubQFractionDistributionRuleMarkuper(),
                                new QCumulativeFractionDistributionRuleMarkuper(),
                                new AdrMustHaveExactVotesSpreadedForQSimpleMarkuper(),
                                new AdrMustHaveExactVotesSpreadedForQSepMarkuper(),
                                new AdrMustHaveExactVotesSpreadedForQCumMarkuper(),
                                new QSimplePrivilegeDividendsNoAbsWarningRuleMarkuper(),
                            };
        }

        public  RuleNodeMarkuperBase GetRuleMarkuperFor(RuleNode ruleNode)
        {
            foreach (var markuper in _markupers)
            {
                if (markuper.CanMarkupRuleNodeOfType(ruleNode))
                    return markuper;
            }

            throw new NotSupportedException("��� ������������� ������ ��� ������� ���� " + ruleNode.GetType());
        }
    }
}