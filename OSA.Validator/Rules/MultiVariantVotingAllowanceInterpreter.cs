using System.Collections.Generic;
using OSA.Validator.Rules.TablesRules.Enums;

namespace OSA.Validator.Rules
{
    public class MultiVariantVotingAllowanceInterpreter
    {
        public static bool IsMultiVariantVotingAllowed(AdditionalChecks additionalChecks)
        {
            var allowedVariants = new List<AdditionalChecks>
                                      {
                                          AdditionalChecks.HasInstructions,
                                          AdditionalChecks.HasInstructions | AdditionalChecks.NotWholeStockWasPassed,
                                          AdditionalChecks.HasInstructions | AdditionalChecks.HasTrust
                                      };
            return allowedVariants.Contains(additionalChecks);
        }
    }
}