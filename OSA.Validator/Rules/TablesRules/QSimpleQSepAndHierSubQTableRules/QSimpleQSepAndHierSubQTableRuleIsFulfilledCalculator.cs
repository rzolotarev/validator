using System;
using OSA.Core.Entities.Registration;
using OSA.Core.Util.Extensions.Enum;
using OSA.Validator.Rules.TablesRules.Enums;
using OSA.Validator.Rules.TablesRules.HelperClasses;

namespace OSA.Validator.Rules.TablesRules
{
    public class QSimpleQSepAndHierSubQTableRuleIsFulfilledCalculator
    {
        public bool GetIsFulfilledFor(PackStatus packStatus,
                                      AdditionalChecks additionalChecks,
                                      AmountOfStockSubmited amountOfStockSubmited,
                                      NumberOfChecks numberOfChecks,
                                      bool trustExists)
        {
            return GetValidationResultFor(packStatus,
                                          additionalChecks,
                                          amountOfStockSubmited,
                                          numberOfChecks,
                                          trustExists)
                .IsFullfilled;
        }

        public string GetErrorTextFor(PackStatus packStatus,
                      AdditionalChecks additionalChecks,
                      AmountOfStockSubmited amountOfStockSubmited,
                      NumberOfChecks numberOfChecks,
                      bool trustExists)
        {
            return GetValidationResultFor(packStatus,
                              additionalChecks,
                              amountOfStockSubmited,
                              numberOfChecks,
                              trustExists)
                .ErrorText;
        }



        private ValidationResult GetValidationResultFor(PackStatus packStatus,
                                      AdditionalChecks additionalChecks,
                                      AmountOfStockSubmited amountOfStockSubmited,
                                      NumberOfChecks numberOfChecks,
                                      bool trustExists)
        {
            var ammountOfStockSubmitedIsNoMorethatThereIsOnPack = AmmountOfStockSubmitedIsNoMorethatThereIsOnPack(amountOfStockSubmited);
            if (!ammountOfStockSubmitedIsNoMorethatThereIsOnPack.IsFullfilled) return ammountOfStockSubmitedIsNoMorethatThereIsOnPack;


            var multipleChecksArentSelectedIfVotesArentSubmited = MultipleChecksArentSelectedIfVotesArentSubmited(amountOfStockSubmited, numberOfChecks);
            if (!multipleChecksArentSelectedIfVotesArentSubmited.IsFullfilled) return multipleChecksArentSelectedIfVotesArentSubmited;

            if (IsTrustSpecialCase(packStatus, additionalChecks, amountOfStockSubmited, numberOfChecks))
            {
                return IsAllowedTrustSpecialCase(trustExists);
            }

            if (packStatus == PackStatus.Simple) return IsSimpleAllowedCase(additionalChecks, amountOfStockSubmited);

            if (packStatus == PackStatus.Seller) return BuyerAndSellerAllowedCasesHelper.IsSellerAllowedCase(additionalChecks, amountOfStockSubmited);

            if (packStatus == PackStatus.Buyer) return BuyerAndSellerAllowedCasesHelper.IsBuyerAllowedCase(additionalChecks, amountOfStockSubmited);

            throw new NotSupportedException("Тип " +packStatus +" не поддерживается.");
        }

        private static ValidationResult AmmountOfStockSubmitedIsNoMorethatThereIsOnPack(AmountOfStockSubmited amountOfStockSubmited)
        {
            if (amountOfStockSubmited == AmountOfStockSubmited.MoreThanThereIsOnPack)
            {
                return ValidationResult.NotFullfilled(ErrorTexts.AMOUNT_OF_STOCK_SUBMITED_IS_GREATER_THAN_THERE_IS_ON_PACK);
            }
            return ValidationResult.Fullfilled;
        }

        private static ValidationResult MultipleChecksArentSelectedIfVotesArentSubmited(AmountOfStockSubmited amountOfStockSubmited, NumberOfChecks numberOfChecks)
        {
            if (numberOfChecks == NumberOfChecks.Multiple && amountOfStockSubmited == AmountOfStockSubmited.VotesArentSubmited)
            {
                return ValidationResult.NotFullfilled(ErrorTexts.WITH_MULTIPLE_CHECKS_VOTES_MUST_BE_SUBMITED);
            }
            return ValidationResult.Fullfilled;
        }

        private static ValidationResult IsAllowedTrustSpecialCase(bool trustExists)
        {
            if (trustExists) return ValidationResult.Fullfilled;
            else return ValidationResult.NotFullfilled(ErrorTexts.NO_TRUST_IN_DATABASE);
        }

        private static bool IsTrustSpecialCase(PackStatus packStatus, AdditionalChecks additionalChecks, AmountOfStockSubmited amountOfStockSubmited, NumberOfChecks numberOfChecks)
        {
            //Специальный случай для проверки наличия доверенности
            return packStatus == PackStatus.Simple &&
                   additionalChecks == AdditionalChecks.HasTrust &&
                   amountOfStockSubmited == AmountOfStockSubmited.LessOrEqualThanThereIsOnPack &&
                   numberOfChecks == NumberOfChecks.Single;
        }


        /// <summary>
        /// Случай для обычного пакета
        /// </summary>
        private static ValidationResult IsSimpleAllowedCase(AdditionalChecks checks, AmountOfStockSubmited amountOfStockSubmited)
        {
            if (amountOfStockSubmited == AmountOfStockSubmited.VotesArentSubmited)
            {
                return ValidationResult.Fullfilled;
            }

            if (checks.IsSet(AdditionalChecks.HasTrust))
            {
                if (checks.IsSet(AdditionalChecks.HasInstructions) ||
                    checks.IsSet(AdditionalChecks.NotWholeStockWasPassed))
                {
                    return ValidationResult.NotFullfilled(ErrorTexts.CANNOT_USE_OTHER_CHECKS_WITH_HAVE_TRUST_CHECK);
                }
            }
            return ValidationResult.Fullfilled;
        }
    }
}