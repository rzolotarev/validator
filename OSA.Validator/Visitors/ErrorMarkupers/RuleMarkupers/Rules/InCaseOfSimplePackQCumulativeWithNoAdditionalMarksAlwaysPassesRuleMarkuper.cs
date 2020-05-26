using System;
using OSA.Validator.Rules;
using OSA.Validator.Rules.TablesRules.QCumulativeTableRules;

namespace OSA.Validator.Visitors.ErrorMarkupers.RuleMarkupers.Rules
{
    public class InCaseOfSimplePackQCumulativeWithNoAdditionalMarksAlwaysPassesRuleMarkuper : MarkAllDependantRuleMarkuper
    {
        public override bool CanMarkupRuleNodeOfType(RuleNode ruleNode)
        {
            return ruleNode is InCaseOfSimplePackQCumulativeWithNoAdditionalMarksAlwaysPassesRule;
        }
    }
}