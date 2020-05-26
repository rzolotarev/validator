using System;
using System.Collections.Generic;
using System.Linq;
using OSA.Validator.Rules;

namespace OSA.Validator.GraphBuilding
{
    public class RuleBuilder
    {
        private RuleNode _lastRule = null;
        private RuleNode _rootRule = null;

        public static RuleBuilder RootRule(RuleNode rule)
        {
            return new RuleBuilder { _rootRule = rule, _lastRule = rule };
        }

        public static RuleBuilder RootRuleChain(RuleNode rule)
        {
            return new RuleBuilder { _rootRule = rule, _lastRule = FindLastRuleInChain(rule) };
        }

        public static RuleBuilder ParallelRootRule<T>(IEnumerable<T> ruleArguments, Func<T, RuleNode> ruleCreationFunc)
        {
            var parallelRule = new ParallelRule(ruleArguments.Select(ruleCreationFunc).ToList());
            return new RuleBuilder { _rootRule = parallelRule, _lastRule = parallelRule };
        }

        public static RuleBuilder ParallelRootRule(params RuleNode[] rules)
        {
            var parallelRule = new ParallelRule(rules);
            return new RuleBuilder { _rootRule = parallelRule, _lastRule = parallelRule };
        }

        public RuleBuilder AddParallelRule<T>(IEnumerable<T> ruleArguments, Func<T, RuleNode> ruleCreationFunc)
        {
            var parallelRule = new ParallelRule(ruleArguments.Select(ruleCreationFunc).ToList());
            _lastRule.DependantRule = parallelRule;
            _lastRule = parallelRule;
            return this;
        }

        public RuleBuilder AddParallelRule(params RuleNode[] rules)
        {
            var parallelRule = new ParallelRule(rules.ToList());
            _lastRule.DependantRule = parallelRule;
            _lastRule = parallelRule;
            return this;
        }

        public RuleBuilder AddRule(RuleNode rule)
        {
            _lastRule.DependantRule = rule;
            _lastRule = rule;
            return this;
        }

        public RuleBuilder AddRuleChain(RuleNode rule)
        {
            _lastRule.DependantRule = rule;
            _lastRule = FindLastRuleInChain(rule);
            return this;
        }

        public RuleNode GetRoot()
        {
            return _rootRule;
        }

        public RuleBuilder InsertRuleChain(RuleNode rootRuleOfRuleChain)
        {
            var lastRuleOfChain = FindLastRuleInChain(rootRuleOfRuleChain);
            _lastRule.DependantRule = rootRuleOfRuleChain;
            _lastRule = lastRuleOfChain;
            return this;
        }

        private static RuleNode FindLastRuleInChain(RuleNode rule)
        {
            if (rule.DependantRule == null) return rule;
            else return FindLastRuleInChain(rule.DependantRule);
        }

    }
}