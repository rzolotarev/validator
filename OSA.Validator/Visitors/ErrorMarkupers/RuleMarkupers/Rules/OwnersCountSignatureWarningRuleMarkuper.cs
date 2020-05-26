using OSA.Editor.ViewModels.Fields;
using OSA.Validator.Rules;
using OSA.Validator.Rules.PageRules;

namespace OSA.Validator.Visitors.ErrorMarkupers.RuleMarkupers.Rules
{
    public class OwnersCountSignatureWarningRuleMarkuper : RuleNodeMarkuperBase
    {
        public override void MarkupFulfilled(RuleNode ruleNode)
        {
            var rule = (OwnersCountSignatureWarningRule) ruleNode;
            rule.Signature.SignatureCheckBox.ErrorLevel = ErrorLevel.Valid;
        }

        public override void MarkupNotFulfilled(RuleNode ruleNode)
        {
            var rule = (OwnersCountSignatureWarningRule)ruleNode;
            rule.Signature.SignatureCheckBox.ErrorLevel = ErrorLevel.Warning;
        }

        public override bool CanMarkupRuleNodeOfType(RuleNode ruleNode)
        {
            return ruleNode is OwnersCountSignatureWarningRule;
        }
    }
}