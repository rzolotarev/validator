using OSA.Validator.Rules;
using OSA.Validator.Rules.TablesRules.QSimpleQSepAndHierSubQTableRules;

namespace OSA.Validator.Visitors.ErrorMarkupers.RuleMarkupers.Rules
{
    public class MultivariantVotingPointRuleMarkuper : MarkAllDependantRuleMarkuper
    {
        public override bool CanMarkupRuleNodeOfType(RuleNode ruleNode)
        {
            return ruleNode is MultivariantVotingPointRule;
        }
    }
}