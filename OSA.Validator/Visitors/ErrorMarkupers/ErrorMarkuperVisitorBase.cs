using System.Collections.Generic;
using System.Linq;
using OSA.Core.Util.Extensions.IEnumerables;
using OSA.Validator.Rules;
using OSA.Validator.Visitors.ErrorMarkupers.RuleMarkupers;

namespace OSA.Validator.Visitors.ErrorMarkupers
{
    public abstract class ErrorMarkuperVisitorBase : IGraphVisitor
    {
        private readonly RuleMarkuperProvider _ruleMarkuperProvider = new RuleMarkuperProvider();

        protected ErrorMarkuperVisitorBase()
        {
            VisitedRules = new List<RuleNode>();
        }

        public IList<RuleNode> VisitedRules { get; private set; }


        public void Visit(RuleNode rule)
        {
            VisitedRules.Add(rule);
            var ruleMarkuper = _ruleMarkuperProvider.GetRuleMarkuperFor(rule);

            if (rule.IsFulfiled)
            {
                ruleMarkuper.MarkupFulfilled(rule);
                rule.DependsOn.ForEach(field => field.ErrorText = null);
            }
            else
            {
                ruleMarkuper.MarkupNotFulfilled(rule);
                rule.DependsOn.ForEach(field => field.ErrorText = rule.GetErrorText());
            }


            if (rule.ShouldGoFurtherDownTheGraph) GoOnFurtherDownTheGraphBeyond(rule);
        }

        public void Visit(ParallelRule rule)
        {
            VisitedRules.Add(rule);

            foreach (var r in rule.ParallelRules)
            {
                r.Accept(this);
            }
            if (rule.ShouldGoFurtherDownTheGraph) GoOnFurtherDownTheGraphBeyond(rule);
        }


        private void GoOnFurtherDownTheGraphBeyond(RuleNode ruleNode)
        {
            NextVisitedRuleNodes(ruleNode).ToList().ForEach(x => x.Accept(this));
        }

        protected abstract IList<RuleNode> NextVisitedRuleNodes(RuleNode ruleNode);
    }
}