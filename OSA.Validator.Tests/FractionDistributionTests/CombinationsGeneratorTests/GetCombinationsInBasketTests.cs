using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OSA.Core.Util;
using OSA.Core.Util.Extensions.Objects;
using OSA.Validator.FractionDistribution;

namespace OSA.Validator.Tests.FractionDistributionTests.CombinationsGeneratorTests
{
    [TestFixture]
    public class GetCombinationsInBasketTests
    {
        [Test]
        public void BasicTest()
        {
            var o1 = new object();
            var o2 = new object();
            var o3 = new object();
            var list = new List<object> { o1, o2, o3 };

            var combinationsGenerator = new CombinationsGenerator();
            const int numberOfBaskets = 3;
            var basketCombinations = combinationsGenerator.GetCombinationsInBaskets(list, numberOfBaskets);

            var expectedBasketCombinations = new List<BasketCombination<object>>
                                                 {
                                                     // null - разделитель между корзинами
                                                     BasketCombinationWith<object>(o1, o2, o3, null, null), 
                                                     BasketCombinationWith<object>(null, o1, o2, o3, null),
                                                     BasketCombinationWith<object>(null, null, o1, o2, o3),

                                                     BasketCombinationWith<object>(o1, null, o2, null, o3), // первый в первой, второй во второй, третий в третьей
                                                     BasketCombinationWith<object>(o1, null, o3, null, o2),
                                                     BasketCombinationWith<object>(o2, null, o1, null, o3),
                                                     BasketCombinationWith<object>(o2, null, o3, null, o1),
                                                     BasketCombinationWith<object>(o3, null, o1, null, o2),
                                                     BasketCombinationWith<object>(o3, null, o2, null, o1),

                                                     BasketCombinationWith<object>(o1, null, o2, o3, null),
                                                     BasketCombinationWith<object>(o1, null, null, o2, o3),
                                                     BasketCombinationWith<object>(o1, null, o2, null, o3),
                                                     BasketCombinationWith<object>(o1, null, o3, null, o2),

                                                     BasketCombinationWith<object>(o2, null, o1, o3, null),
                                                     BasketCombinationWith<object>(o2, null, null, o1, o3),
                                                     BasketCombinationWith<object>(o2, null, o1, null, o3),
                                                     BasketCombinationWith<object>(o2, null, o3, null, o1),

                                                     BasketCombinationWith<object>(o3, null, o2, o1, null),
                                                     BasketCombinationWith<object>(o3, null, null, o2, o1),
                                                     BasketCombinationWith<object>(o3, null, o2, null, o1),
                                                     BasketCombinationWith<object>(o3, null, o1, null, o2),

                                                     BasketCombinationWith<object>(o1, o2, null, o3, null),
                                                     BasketCombinationWith<object>(o1, o2, null, null, o3),

                                                     BasketCombinationWith<object>(o1, o3, null, o2, null),
                                                     BasketCombinationWith<object>(o1, o3, null, null, o2),

                                                     BasketCombinationWith<object>(o2, o3, null, o1, null),
                                                     BasketCombinationWith<object>(o2, o3, null, null, o1),
                                                 };

            foreach (var expected in expectedBasketCombinations)
            {
                var matchingResult = basketCombinations.SingleOrDefault(x => BasketsAreEqual(expected, x));
                Assert.NotNull(matchingResult, "Ожидаемый вариант {0} не найден", GetVariantText(expected));
            }

            Assert.That(basketCombinations.Count, Is.EqualTo(expectedBasketCombinations.Count));
        }

        [Test]
        public void BasicTest2()
        {
            var o1 = 1;
            var o2 = 2;
            var o3 = 3;
            var list = new List<object> { o1, o2, o3 };

            var combinationsGenerator = new CombinationsGenerator();
            const int numberOfBaskets = 3;
            var basketCombinations = combinationsGenerator.GetCombinationsInBaskets(list, numberOfBaskets);

            var expectedBasketCombinations = new List<BasketCombination<object>>
                                                 {
                                                     // null - разделитель между корзинами
                                                     BasketCombinationWith<object>(o1, o2, o3, null, null), 
                                                     BasketCombinationWith<object>(null, o1, o2, o3, null),
                                                     BasketCombinationWith<object>(null, null, o1, o2, o3),

                                                     BasketCombinationWith<object>(o1, null, o2, null, o3), // первый в первой, второй во второй, третий в третьей
                                                     BasketCombinationWith<object>(o1, null, o3, null, o2),
                                                     BasketCombinationWith<object>(o2, null, o1, null, o3),
                                                     BasketCombinationWith<object>(o2, null, o3, null, o1),
                                                     BasketCombinationWith<object>(o3, null, o1, null, o2),
                                                     BasketCombinationWith<object>(o3, null, o2, null, o1),

                                                     BasketCombinationWith<object>(o1, null, o2, o3, null),
                                                     BasketCombinationWith<object>(o1, null, null, o2, o3),
                                                     BasketCombinationWith<object>(o1, null, o2, null, o3),
                                                     BasketCombinationWith<object>(o1, null, o3, null, o2),

                                                     BasketCombinationWith<object>(o2, null, o1, o3, null),
                                                     BasketCombinationWith<object>(o2, null, null, o1, o3),
                                                     BasketCombinationWith<object>(o2, null, o1, null, o3),
                                                     BasketCombinationWith<object>(o2, null, o3, null, o1),

                                                     BasketCombinationWith<object>(o3, null, o2, o1, null),
                                                     BasketCombinationWith<object>(o3, null, null, o2, o1),
                                                     BasketCombinationWith<object>(o3, null, o2, null, o1),
                                                     BasketCombinationWith<object>(o3, null, o1, null, o2),

                                                     BasketCombinationWith<object>(o1, o2, null, o3, null),
                                                     BasketCombinationWith<object>(o1, o2, null, null, o3),

                                                     BasketCombinationWith<object>(o1, o3, null, o2, null),
                                                     BasketCombinationWith<object>(o1, o3, null, null, o2),

                                                     BasketCombinationWith<object>(o2, o3, null, o1, null),
                                                     BasketCombinationWith<object>(o2, o3, null, null, o1),
                                                 };

            foreach (var expected in expectedBasketCombinations)
            {
                var matchingResult = basketCombinations.SingleOrDefault(x => BasketsAreEqual(expected, x));
                Assert.NotNull(matchingResult, "Ожидаемый вариант {0} не найден", GetVariantText(expected));
            }

            Assert.That(basketCombinations.Count, Is.EqualTo(expectedBasketCombinations.Count));
        }

        [Test]
        public void EqualObjectsInCollectionTests()
        {
            var list = new List<int> { 1, 1 };

            var combinationsGenerator = new CombinationsGenerator();
            const int numberOfBaskets = 2;
            var basketCombinations = combinationsGenerator.GetCombinationsInBaskets(list, numberOfBaskets);
            var expectedBasketCombinations = new List<BasketCombination<int>>
                                                 {
                                                     // null - разделитель между корзинами
                                                     BasketCombinationWith<int>(1, 1, null), 
                                                     BasketCombinationWith<int>(1, null, 1),
                                                     BasketCombinationWith<int>(null, 1, 1),
                                                 };

            foreach (var expected in expectedBasketCombinations)
            {
                var matchingResult = basketCombinations.SingleOrDefault(x => BasketsAreEqual(expected, x));
                Assert.NotNull(matchingResult, "Ожидаемый вариант {0} не найден", GetVariantText(expected));
            }

            Assert.That(basketCombinations.Count, Is.EqualTo(expectedBasketCombinations.Count));
        }

        private static string GetVariantText<T>(BasketCombination<T> expected)
        {
            Func<Basket<T>, string> getBasketText = b => b.BasketElements.Aggregate("", (r, e) => r + " " + e);
            return expected.Baskets.Aggregate("", (r, b) => r + "{" + getBasketText(b) + "} ");
        }

        private static bool BasketsAreEqual<T>(BasketCombination<T> basketCombination, BasketCombination<T> expected)
        {
            if (basketCombination.Baskets.Count != expected.Baskets.Count) return false;

            for (var i = 0; i < basketCombination.Baskets.Count; i++)
            {
                if (!basketCombination.Baskets[i].BasketElements.AreElementsAreEqualTo(expected.Baskets[i].BasketElements))
                    return false;
            }
            return true;
        }


        private static BasketCombination<T> BasketCombinationWith<T>(params object[] parameters)
        {
            object delimiter = null;
            Func<object, bool> isNotDilimeter = x => x != delimiter;

            var baskets = new List<Basket<T>>();

            int numberToSkip = 0;

            while(numberToSkip <= parameters.Count())
            {
                var group = parameters.Skip(numberToSkip).TakeWhile(isNotDilimeter);
                baskets.Add(new Basket<T>(group.Cast<T>().ToList()));
                numberToSkip += group.Count() + 1;
            }

            return new BasketCombination<T>(new List<Basket<T>>(baskets));
        }

        [Test]
        public void NegativeNumberOfBasketsTest()
        {
            var combinationsGenerator = new CombinationsGenerator();
            var list = new List<int> { 1, 2, 3 };
            const int numberOfBaskets = -10;
            Assert.Throws<Check.CheckException>(() => combinationsGenerator.GetCombinationsInBaskets(list, numberOfBaskets), "Минимальное число корзин - ноль.");
        }
        [Test]
        public void ZeroNumberOfBasketsTest()
        {
            var combinationsGenerator = new CombinationsGenerator();
            var list = new List<int> { 1, 2, 3 };
            const int numberOfBaskets = 0;
            var combinations = combinationsGenerator.GetCombinationsInBaskets(list, numberOfBaskets);
            Assert.That(combinations.Count, Is.EqualTo(0));
        }
    }
}
