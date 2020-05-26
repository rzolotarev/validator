using System.Collections.Generic;
using OSA.Validator.Rules;

namespace OSA.Validator.Visitors.ErrorMarkupers
{
    public class GeneralErrorMarkerVisitor : ErrorMarkuperVisitorBase
    {
        protected override IList<RuleNode> NextVisitedRuleNodes(RuleNode ruleNode)
        {
            var result = new List<RuleNode>();
            if (ruleNode.DependantRule != null)
                result.Add(ruleNode.DependantRule);
            return result;
        }
    }
}