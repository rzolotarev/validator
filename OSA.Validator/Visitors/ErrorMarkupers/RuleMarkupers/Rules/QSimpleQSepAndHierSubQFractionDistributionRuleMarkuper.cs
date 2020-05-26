using OSA.Validator.Rules;
using OSA.Validator.Rules.TablesRules;

namespace OSA.Validator.Visitors.ErrorMarkupers.RuleMarkupers.Rules
{
    public class QSimpleQSepAndHierSubQFractionDistributionRuleMarkuper : MarkAllDependantRuleMarkuper
    {
        public override bool CanMarkupRuleNodeOfType(RuleNode ruleNode)
        {
            return ruleNode is QSimpleQSepAndHierSubQFractionDistributionRule;
        }
    }
}