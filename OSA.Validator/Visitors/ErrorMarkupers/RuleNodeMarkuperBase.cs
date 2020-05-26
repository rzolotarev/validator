using OSA.Validator.Rules;

namespace OSA.Validator.Visitors.ErrorMarkupers
{
    public abstract class RuleNodeMarkuperBase
    {
        public abstract void MarkupFulfilled(RuleNode ruleNode);
        public abstract void MarkupNotFulfilled(RuleNode ruleNode);
        public abstract bool CanMarkupRuleNodeOfType(RuleNode ruleNode);
    }
}