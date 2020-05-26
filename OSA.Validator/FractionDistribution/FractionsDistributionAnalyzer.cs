using System.Collections.Generic;
using System.Linq;
using OSA.Core.Entities.Registration;
using OSA.Core.Util;
using OSA.Core.Util.Extensions.FractionLongs;

namespace OSA.Validator.FractionDistribution
{
    public class FractionsDistributionAnalyzer
    {
        /// <summary>
        /// Проверяет, возможно ли распределить дроби по полям (например, можно ли распределить 1/6, 1/7 и 1/8
        /// и получить поля со значениями 10 + 1/6, 20 и 30 + 15/56
        /// </summary>
        /// <param name="fields">Дробные значения полей</param>
        /// <param name="fractionParts">Варианты дробных</param>
        /// <returns></returns>
        public bool FractionDistributionIsPossible(List<FractionLong> fields, List<FractionLong> fractionParts)
        {
            var combinationsGenerator = new CombinationsGenerator();
            var basketCombinations = combinationsGenerator.GetCombinationsInBaskets(fractionParts, fields.Count);

            return basketCombinations.Any(basketCombination => BasketCombinationMatches(fields, basketCombination));
        }

        private static bool BasketCombinationMatches(List<FractionLong> fields, BasketCombination<FractionLong> basketCombination)
        {
            var nonzeroBasketsValues = GetNonZeroBasketFractionValues(basketCombination);

            var remaningFields = new List<FractionLong>(fields);

            var matchFound = true;
            foreach (var basketValue in nonzeroBasketsValues)
            {
                if (ThereIsMatchingFieldIn(remaningFields, basketValue))
                {
                    RemoveMatchingFieldFrom(remaningFields, basketValue);
                }
                else
                {
                    matchFound = false;
                    break;
                }
            }

            var thereAreNoFractionFieldsRemaining = remaningFields.Where(x => x.HasFractionPart).Count() == 0;
            return matchFound && thereAreNoFractionFieldsRemaining;
        }

        private static bool ThereIsMatchingFieldIn(List<FractionLong> fieldsRemaining, FractionLong basketValue)
        {
            var orderedFields = fieldsRemaining.OrderBy(x => x.Numerator);
            return orderedFields.Where(x => FieldCanHoldBasketValue(x, basketValue)).Count() > 0;
        }

        private static void RemoveMatchingFieldFrom(List<FractionLong> fieldsRemaining, FractionLong basketValue)
        {
            var orderedFields = fieldsRemaining.OrderBy(x => x.Numerator);
            var matchingField = orderedFields.First(x => FieldCanHoldBasketValue(x, basketValue));
            fieldsRemaining.Remove(matchingField);
        }

        private static bool FieldCanHoldBasketValue(FractionLong field, FractionLong basketValue)
        {
            return field.FractionPart == basketValue.FractionPart && field >= basketValue;
        }

        private static List<FractionLong> GetNonZeroBasketFractionValues(BasketCombination<FractionLong> basketCombination)
        {
            var basketFractionValues = basketCombination.Baskets.Select(x => x.BasketElements.Aggregate(0.FL(), (res, n) => res + n)).ToList();
            var nonzeroBasketsValues = basketFractionValues.Where(x => x != 0.FL()).ToList();
            return nonzeroBasketsValues;
        }
    }
}