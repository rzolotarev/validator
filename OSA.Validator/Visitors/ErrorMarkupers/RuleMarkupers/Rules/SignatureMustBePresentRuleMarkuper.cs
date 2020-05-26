using OSA.Validator.Rules;
using OSA.Validator.Rules.PageRules;

namespace OSA.Validator.Visitors.ErrorMarkupers.RuleMarkupers.Rules
{
    public class SignatureMustBePresentRuleMarkuper : MarkAllDependantRuleMarkuper
    {
        public override bool CanMarkupRuleNodeOfType(RuleNode ruleNode)
        {
            return ruleNode is SignatureMustBePresentRule;
        }
    }
}