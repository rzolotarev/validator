using System;
using System.Collections.Generic;
using System.Linq;
using OSA.Editor.ViewModels.Fields;
using OSA.Validator.Rules;
using OSA.Validator.Visitors;

namespace OSA.Validator
{
    public class ParallelRule : RuleNode
    {
        private readonly bool _shouldGoFurtherDownTheGraphAnyway;
        public IList<RuleNode> ParallelRules { get; set; }

        public ParallelRule(IList<RuleNode> parallelRules, bool shouldGoFurtherDownTheGraphAnyway = false)
        {
            _shouldGoFurtherDownTheGraphAnyway = shouldGoFurtherDownTheGraphAnyway;
            ParallelRules = parallelRules;
        }

        public override bool IsFulfiled
        {
            get
            {
                return ParallelRules.All(RuleAndItsDependantsAreFulfilled);
            }
        }

        public override bool ShouldGoFurtherDownTheGraph
        {
            get
            {
                if (_shouldGoFurtherDownTheGraphAnyway) return true;
                return IsFulfiled;
            }
        }

        private static bool RuleAndItsDependantsAreFulfilled(RuleNode node)
        {
            if (node.DependantRule == null)
            {
                return node.IsFulfiled;
            }
            else
            {
                return node.IsFulfiled && RuleAndItsDependantsAreFulfilled(node.DependantRule);
            }
        }

        public override IList<DocField> DependsOn
        {
            get
            {
                var rootRulesDependOn = ParallelRules.SelectMany(r => r.DependsOn).ToList();
                var dependantRulesDependOn = ParallelRules.SelectMany(r => r.DependantRulesDependOn).ToList();
                var result = new List<DocField>();
                result.AddRange(rootRulesDependOn);
                result.AddRange(dependantRulesDependOn);

                result = result.Distinct().ToList();
                return result;
            }
        }

        public override void Accept(IGraphVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string GetErrorText()
        {
            throw new NotSupportedException("Shouldnt ever get here");
        }
    }
}