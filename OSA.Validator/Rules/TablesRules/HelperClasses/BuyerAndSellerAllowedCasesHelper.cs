using OSA.Core.Util.Extensions.Enum;
using OSA.Validator.Rules.TablesRules.Enums;

namespace OSA.Validator.Rules.TablesRules.HelperClasses
{
    public static class BuyerAndSellerAllowedCasesHelper
    {
        /// <summary>
        /// Случай для продавца
        /// </summary>
        public static ValidationResult IsSellerAllowedCase(AdditionalChecks additionalChecks, AmountOfStockSubmited amountOfStockSubmited)
        {
            if (additionalChecks == AdditionalChecks.NotWholeStockWasPassed &&
                amountOfStockSubmited == AmountOfStockSubmited.VotesArentSubmited) return ValidationResult.NotFullfilled(ErrorTexts.SELLER_PACKET_WITH_NOT_WHOLE_STOCK_PASSED_CHECK_MUST_HAVE_VOTES_SUBMITED);

            var sellerCheckedThatHeHasTrust = additionalChecks.IsSet(AdditionalChecks.HasTrust);
            if (sellerCheckedThatHeHasTrust) return ValidationResult.NotFullfilled(ErrorTexts.SELLER_PACK_CANNOT_HAVE_HAVE_TRUST_CHECK);

            var hasInstructionsIsChecked = additionalChecks.IsSet(AdditionalChecks.HasInstructions);
            var notWholeStockPassedIsChecked = additionalChecks.IsSet(AdditionalChecks.NotWholeStockWasPassed);
            if (!hasInstructionsIsChecked && !notWholeStockPassedIsChecked) return ValidationResult.NotFullfilled(ErrorTexts.SELLER_PACKET_MUST_HAVE_NOTWHOLESTOCKWASPASSED_CHECK);

            return ValidationResult.Fullfilled;
        }

        /// <summary>
        /// Случай для покупателя
        /// </summary>
        public static ValidationResult IsBuyerAllowedCase(AdditionalChecks checks, AmountOfStockSubmited amountOfStockSubmited)
        {
            var buyerHasTrust = checks.IsSet(AdditionalChecks.HasTrust);
            var buyerHasSumbitedVotes = amountOfStockSubmited != AmountOfStockSubmited.VotesArentSubmited;
            var buyerHasntCheckedNotWholeStockWasPassed = !checks.IsSet(AdditionalChecks.NotWholeStockWasPassed);

            if (!buyerHasTrust) return ValidationResult.NotFullfilled(ErrorTexts.BUYERS_PACK_MUST_HAVE_HAVE_TRUST_CHECK);
            if (!buyerHasSumbitedVotes) return ValidationResult.NotFullfilled(ErrorTexts.BUYERS_PACK_MUST_HAVE_VOTES_SUBMITED);
            if (!buyerHasntCheckedNotWholeStockWasPassed) return ValidationResult.NotFullfilled(ErrorTexts.BUYERS_PACK_CANNOT_HAVE_NOTWHOLESTOCKPASSED_CHECK);

            return ValidationResult.Fullfilled;
        }
    }
}
