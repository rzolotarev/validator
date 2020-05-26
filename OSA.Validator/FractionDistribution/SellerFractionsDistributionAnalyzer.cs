using System.Collections.Generic;
using System.Linq;
using OSA.Core.Entities.Registration;
using OSA.Core.Util;
using OSA.Core.Util.Extensions.FractionLongs;

namespace OSA.Validator.FractionDistribution
{
    public class SellerFractionsDistributionAnalyzer
    {
        private readonly FractionsDistributionAnalyzer _fractionDistributionAnalyzer = new FractionsDistributionAnalyzer();
        private readonly CombinationsGenerator _combinationsGenerator = new CombinationsGenerator();

        /// <summary>
        /// Проверяет, возможно ли распределить дроби по полям, когда неизвестно, сколько и с каких счетов акционер
        /// продал акции
        /// (например, у акционера было три счета с дробными акциями 1/6, 1/7 и 1/8, можно ли при этом получить распределение 10 + 1/6, 20 и 30 + 1/7
        /// </summary>
        /// <param name="fields">Дробные значения полей</param>
        /// <param name="fractionParts">Варианты дробных</param>
        /// <returns></returns>
        public bool FractionDistributionIsPossible(List<FractionLong> fields, List<FractionLong> fractionParts)
        {
            #region Assertions
            Check.That(fractionParts.All(x => x < 1.FL()), "Все дробные части должны быть меньше одного.");
            #endregion

            var nonZeroFractionParts = fractionParts.Where(x => x.FractionPart != 0.FL()).ToList();
            if (nonZeroFractionParts.Count == 0) return true;

            var nonZeroFields = fields.Where(x => x != 0.FL()).ToList();

            var fractionPartsCombinations = _combinationsGenerator.GetCombinations(nonZeroFractionParts);

            return fractionPartsCombinations.Any(fractionPartsCombination => _fractionDistributionAnalyzer.FractionDistributionIsPossible(nonZeroFields, fractionPartsCombination.Elements));
        }
    }
}