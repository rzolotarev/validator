using System;

namespace OSA.Validator.Rules.TablesRules.Enums
{
    [Flags]
    public enum AdditionalChecks
    {
        None = 0,
        HasInstructions = 1,
        HasTrust = 2,
        NotWholeStockWasPassed = 4
    }
}