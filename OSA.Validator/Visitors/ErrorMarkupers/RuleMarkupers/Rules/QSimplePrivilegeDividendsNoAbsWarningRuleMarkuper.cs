using OSA.Editor.ViewModels.Fields;
using OSA.Validator.Rules;

namespace OSA.Validator.Visitors.ErrorMarkupers.RuleMarkupers.Rules
{
    public class QSimplePrivilegeDividendsNoAbsWarningRuleMarkuper : RuleNodeMarkuperBase
    {
        public override void MarkupFulfilled(RuleNode ruleNode)
        {
            var rule = (QSimplePrivilegeDividendsNoAbsWarningRule)ruleNode;
            rule.Question.Point.YesCheckBoxField.ErrorLevel = ErrorLevel.Valid;
            rule.Question.Point.NoCheckBoxField.ErrorLevel = ErrorLevel.Valid;
            rule.Question.Point.AbstainedCheckBoxField.ErrorLevel = ErrorLevel.Valid;

            rule.Question.Point.YesVotesField.ErrorLevel = ErrorLevel.Valid;
            rule.Question.Point.NoVotesField.ErrorLevel = ErrorLevel.Valid;
            rule.Question.Point.AbstainedVotesField.ErrorLevel = ErrorLevel.Valid;
        }

        public override void MarkupNotFulfilled(RuleNode ruleNode)
        {
            var rule = (QSimplePrivilegeDividendsNoAbsWarningRule)ruleNode;
            rule.Question.Point.YesCheckBoxField.ErrorLevel = ErrorLevel.Valid;
            rule.Question.Point.NoCheckBoxField.ErrorLevel = ErrorLevel.Warning;
            rule.Question.Point.AbstainedCheckBoxField.ErrorLevel = ErrorLevel.Warning;

            rule.Question.Point.YesVotesField.ErrorLevel = ErrorLevel.Valid;
            rule.Question.Point.NoVotesField.ErrorLevel = ErrorLevel.Warning;
            rule.Question.Point.AbstainedVotesField.ErrorLevel = ErrorLevel.Warning;
        }

        public override bool CanMarkupRuleNodeOfType(RuleNode ruleNode)
        {
            return ruleNode is QSimplePrivilegeDividendsNoAbsWarningRule;
        }
    }
}
