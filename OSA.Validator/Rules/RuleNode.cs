using System.Collections.Generic;
using System.Linq;
using OSA.Editor.ViewModels.Fields;
using OSA.Validator.Visitors;

namespace OSA.Validator.Rules
{
    public abstract class RuleNode
    {
        public abstract bool IsFulfiled { get; }

        public abstract bool ShouldGoFurtherDownTheGraph { get; }

        public RuleNode DependantRule { get; set; }

        public abstract IList<DocField> DependsOn { get; }

        public IList<DocField> DependantRulesDependOn
        {
            get
            {
                var result = new List<DocField>();
                if (DependantRule != null)
                {
                    result.AddRange(DependantRule.DependsOn);
                    result.AddRange(DependantRule.DependantRulesDependOn);
                }
                return result.Distinct().ToList();
            }
        }

        public abstract void Accept(IGraphVisitor visitor);

        public abstract string GetErrorText();
    }
}