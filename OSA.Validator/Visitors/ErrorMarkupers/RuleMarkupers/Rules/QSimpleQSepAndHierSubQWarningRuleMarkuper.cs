using OSA.Editor.ViewModels.Fields;
using OSA.Validator.Rules;

namespace OSA.Validator.Visitors.ErrorMarkupers.RuleMarkupers.Rules
{
    public class QSimpleQSepAndHierSubQWarningRuleMarkuper : RuleNodeMarkuperBase
    {
        public override void MarkupFulfilled(RuleNode ruleNode)
        {
            var rule = (QSimpleQSepAndHierSubQWarningRule)ruleNode;
            rule.Point.YesCheckBoxField.ErrorLevel = ErrorLevel.Valid;
            rule.Point.NoCheckBoxField.ErrorLevel = ErrorLevel.Valid;
            rule.Point.AbstainedCheckBoxField.ErrorLevel = ErrorLevel.Valid;

            rule.Point.YesVotesField.ErrorLevel = ErrorLevel.Valid;
            rule.Point.NoVotesField.ErrorLevel = ErrorLevel.Valid;
            rule.Point.AbstainedVotesField.ErrorLevel = ErrorLevel.Valid;
        }

        public override void MarkupNotFulfilled(RuleNode ruleNode)
        {
            var rule = (QSimpleQSepAndHierSubQWarningRule)ruleNode;
            rule.Point.YesCheckBoxField.ErrorLevel = ErrorLevel.Warning;
            rule.Point.NoCheckBoxField.ErrorLevel = ErrorLevel.Warning;
            rule.Point.AbstainedCheckBoxField.ErrorLevel = ErrorLevel.Warning;

            rule.Point.YesVotesField.ErrorLevel = ErrorLevel.Warning;
            rule.Point.NoVotesField.ErrorLevel = ErrorLevel.Warning;
            rule.Point.AbstainedVotesField.ErrorLevel = ErrorLevel.Warning;
        }

        public override bool CanMarkupRuleNodeOfType(RuleNode ruleNode)
        {
            return ruleNode is QSimpleQSepAndHierSubQWarningRule;
        }
    }
}