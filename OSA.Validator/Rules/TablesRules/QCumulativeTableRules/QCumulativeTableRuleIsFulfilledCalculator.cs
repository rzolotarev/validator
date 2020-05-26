using System;
using OSA.Core.Entities.Registration;
using OSA.Core.Util.Extensions.Enum;
using OSA.Validator.Rules.TablesRules.Enums;
using OSA.Validator.Rules.TablesRules.HelperClasses;

namespace OSA.Validator.Rules.TablesRules.QCumulativeTableRules
{
    public class QCumulativeTableRuleIsFulfilledCalculator
    {
        // Важно! Для корректной работы, если не указаны голоса для варианта "ЗА" всегда amountOfStockSubmited должно
        // быть равно VotesArentSubmited, иначе могут случиться неприятности в случае NumberOfChecks.Multiple, когда
        // выбран вариант "ЗА" без указания голосов и "ПРОТИВ" с указанием ("ЗА" всегда должен иметь количество голосов
        // указанным).
        public bool GetIsFulfilledFor(PackStatus packStatus, AdditionalChecks additionalChecks, AmountOfStockSubmited amountOfStockSubmited, NumberOfChecks numberOfChecks, CumYesIs cumChecks, bool trustExists)
        {
            return
                GetIsValidationResultFor(packStatus,
                                         additionalChecks,
                                         amountOfStockSubmited,
                                         numberOfChecks,
                                         cumChecks,
                                         trustExists)
                    .IsFullfilled;
        }

        public string GetErrorTextFor(PackStatus packStatus, AdditionalChecks additionalChecks, AmountOfStockSubmited amountOfStockSubmited, NumberOfChecks numberOfChecks, CumYesIs cumChecks, bool trustExists)
        {
            return
                GetIsValidationResultFor(packStatus,
                                         additionalChecks,
                                         amountOfStockSubmited,
                                         numberOfChecks,
                                         cumChecks,
                                         trustExists)
                    .ErrorText;
        }



        public ValidationResult GetIsValidationResultFor(PackStatus packStatus, AdditionalChecks additionalChecks, AmountOfStockSubmited amountOfStockSubmited, NumberOfChecks numberOfChecks, CumYesIs cumChecks, bool trustExists)
        {
            var ammountOfStockSubmitedIsNoMorethatThereIsOnPack = AmmountOfStockSubmitedIsNoMorethatThereIsOnPack(amountOfStockSubmited);
            if (!ammountOfStockSubmitedIsNoMorethatThereIsOnPack.IsFullfilled) return ammountOfStockSubmitedIsNoMorethatThereIsOnPack;

            var ifYesIsCheckedVotesAreSubmited = IfYesIsCheckedVotesAreSubmited(amountOfStockSubmited, cumChecks);
            if (!ifYesIsCheckedVotesAreSubmited.IsFullfilled) return ifYesIsCheckedVotesAreSubmited;

            var multipleChecksArentSelectedIfVotesArentSubmited = MultipleChecksArentSelectedIfVotesArentSubmited(amountOfStockSubmited, numberOfChecks);
            if (!multipleChecksArentSelectedIfVotesArentSubmited.IsFullfilled) return multipleChecksArentSelectedIfVotesArentSubmited;

            if (IsTrustSpecialCase(packStatus, additionalChecks, amountOfStockSubmited, numberOfChecks, cumChecks))
            {
                return IsAllowedTrustSpecialCase(trustExists);
            }

            if (packStatus == PackStatus.Simple) return IsSimpleAllowedCase(additionalChecks, amountOfStockSubmited, cumChecks, numberOfChecks);
            if (packStatus == PackStatus.Seller) return BuyerAndSellerAllowedCasesHelper.IsSellerAllowedCase(additionalChecks, amountOfStockSubmited);
            if (packStatus == PackStatus.Buyer) return BuyerAndSellerAllowedCasesHelper.IsBuyerAllowedCase(additionalChecks, amountOfStockSubmited);

            throw new NotSupportedException("Тип " + packStatus + " не поддерживается.");
        }

        private static ValidationResult AmmountOfStockSubmitedIsNoMorethatThereIsOnPack(AmountOfStockSubmited amountOfStockSubmited)
        {
            if (amountOfStockSubmited == AmountOfStockSubmited.MoreThanThereIsOnPack) return ValidationResult.NotFullfilled(ErrorTexts.AMOUNT_OF_STOCK_SUBMITED_IS_GREATER_THAN_THERE_IS_ON_PACK);
            return ValidationResult.Fullfilled;
        }

        private static ValidationResult IfYesIsCheckedVotesAreSubmited(AmountOfStockSubmited amountOfStockSubmited, CumYesIs cumYesCheck)
        {
            if (cumYesCheck == CumYesIs.Checked && amountOfStockSubmited == AmountOfStockSubmited.VotesArentSubmited) 
                return ValidationResult.NotFullfilled(ErrorTexts.CUMULATIVE_QUESTION_MUST_HAVE_VOTES_SUBMITED_IN_CASE_OF_YES_CHECK);
            return ValidationResult.Fullfilled;
        }

        private static ValidationResult MultipleChecksArentSelectedIfVotesArentSubmited(AmountOfStockSubmited amountOfStockSubmited, NumberOfChecks numberOfChecks)
        {
            if (numberOfChecks == NumberOfChecks.Multiple && amountOfStockSubmited == AmountOfStockSubmited.VotesArentSubmited)
                return ValidationResult.NotFullfilled(ErrorTexts.WITH_MULTIPLE_CHECKS_VOTES_MUST_BE_SUBMITED);
            return ValidationResult.Fullfilled;
        }

        private static ValidationResult IsAllowedTrustSpecialCase(bool trustExists)
        {
            if (trustExists) return ValidationResult.Fullfilled;
            else return ValidationResult.NotFullfilled(ErrorTexts.NO_TRUST_IN_DATABASE);
        }


        /// <summary>
        /// Случай для обычного пакета
        /// </summary>
        private static ValidationResult IsSimpleAllowedCase(AdditionalChecks checks, AmountOfStockSubmited amountOfStockSubmited, CumYesIs cumChecks, NumberOfChecks numberOfChecks)
        {
            if (amountOfStockSubmited == AmountOfStockSubmited.VotesArentSubmited)
            {
                if (cumChecks == CumYesIs.NotChecked) return ValidationResult.Fullfilled;
                else return ValidationResult.NotFullfilled(ErrorTexts.CUMULATIVE_QUESTION_MUST_HAVE_VOTES_SUBMITED_IN_CASE_OF_YES_CHECK);
            }

            if (checks.IsSet(AdditionalChecks.HasTrust))
            {
                if (checks.IsSet(AdditionalChecks.HasInstructions) ||
                    checks.IsSet(AdditionalChecks.NotWholeStockWasPassed))
                {
                    //Установлен только вариант "ЗА"
                    if (cumChecks == CumYesIs.Checked && numberOfChecks == NumberOfChecks.Single) return ValidationResult.Fullfilled;
                    else return ValidationResult.NotFullfilled(ErrorTexts.INCORRECT_FILLING);
                }
            }
            return ValidationResult.Fullfilled;
        }

        private bool IsTrustSpecialCase(PackStatus packStatus, AdditionalChecks additionalChecks, AmountOfStockSubmited amountOfStockSubmited, NumberOfChecks numberOfChecks, CumYesIs cumChecks)
        {
            return packStatus == PackStatus.Simple &&
                   additionalChecks == AdditionalChecks.HasTrust &&
                   amountOfStockSubmited == AmountOfStockSubmited.LessOrEqualThanThereIsOnPack &&
                   cumChecks == CumYesIs.NotChecked;
        }
    }
}