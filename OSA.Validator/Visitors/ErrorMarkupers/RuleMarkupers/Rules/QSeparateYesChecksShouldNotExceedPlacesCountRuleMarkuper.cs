using OSA.Validator.Rules;
using OSA.Validator.Rules.QSeparateRules;

namespace OSA.Validator.Visitors.ErrorMarkupers.RuleMarkupers.Rules
{
    public class QSeparateYesChecksShouldNotExceedPlacesCountRuleMarkuper : MarkAllDependantRuleMarkuper
    {
        public override bool CanMarkupRuleNodeOfType(RuleNode ruleNode)
        {
            return ruleNode is QSeparateYesChecksShouldNotExceedPlacesCountRule;
        }

        public override void MarkupFulfilled(RuleNode ruleNode)
        {
            //do nothing
        }
    }
}