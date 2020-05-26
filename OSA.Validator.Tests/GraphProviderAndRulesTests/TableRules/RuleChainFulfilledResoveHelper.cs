using OSA.Validator.Rules;

namespace OSA.Validator.Tests.GraphProviderAndRulesTests.TableRules
{
    public static class RuleChainFulfilledResoveHelper
    {
        public static bool RuleChainIsFulfilled(RuleNode chain)
        {
            if (!chain.IsFulfiled) return false;
            if (chain.DependantRule == null || chain.ShouldGoFurtherDownTheGraph == false) return true;
            return (RuleChainIsFulfilled(chain.DependantRule));
        }
    }
}