using OSA.Validator.Rules;

namespace OSA.Validator.Visitors.ErrorMarkupers.RuleMarkupers.Rules
{
    public class AdrMustHaveExactVotesSpreadedForQSepMarkuper : MarkAllDependantRuleMarkuper
    {
        public override bool CanMarkupRuleNodeOfType(RuleNode ruleNode)
        {
            return ruleNode is AdrMustHaveExactOrLessVotesSpreadedForQSep;
        }

        public override void MarkupFulfilled(RuleNode ruleNode)
        {
            //do nothing
        }
    }
}