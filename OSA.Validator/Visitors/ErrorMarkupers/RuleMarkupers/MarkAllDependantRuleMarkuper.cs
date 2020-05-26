using System.Linq;
using OSA.Editor.ViewModels.Fields;
using OSA.Validator.Rules;

namespace OSA.Validator.Visitors.ErrorMarkupers.RuleMarkupers
{
    public abstract class MarkAllDependantRuleMarkuper:RuleNodeMarkuperBase
    {
        public override void MarkupFulfilled(RuleNode ruleNode)
        {
            ruleNode.DependsOn.ToList().ForEach(d=>d.ErrorLevel = ErrorLevel.Valid);
        }

        public override void MarkupNotFulfilled(RuleNode ruleNode)
        {
            ruleNode.DependsOn.ToList().ForEach(d => d.ErrorLevel = ErrorLevel.Error);
        }
    }
}