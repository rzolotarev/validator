using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OSA.Core.Util.Extensions.Int;
using OSA.Core.Util.Extensions.Objects;
using OSA.Validator.FractionDistribution;

namespace OSA.Validator.Tests.FractionDistributionTests.CombinationsGeneratorTests
{
    [TestFixture]
    public class GetCombinationsTests
    {
        [Test]
        public void BasicTest()
        {
            var object1 = new object();
            var object2 = new object();
            var object3 = new object();
            var list = new List<object> { object1, object2, object3};

            var combinationsGenerator = new CombinationsGenerator();
            var combinations = combinationsGenerator.GetCombinations(list);
            Assert.That(combinations.Count, Is.EqualTo(8));


            var expectedCombination1 = new List<object>();
            var expectedCombination2 = new List<object> { object1 };
            var expectedCombination3 = new List<object> { object1, object2 };
            var expectedCombination4 = new List<object> { object1, object3 };
            var expectedCombination5 = new List<object> { object1, object2, object3 };
            var expectedCombination6 = new List<object> { object2 };
            var expectedCombination7 = new List<object> { object2, object3 };
            var expectedCombination8 = new List<object> { object3 };

            var expectedCombinationsList = new List<List<object>>
                                               {
                                                   expectedCombination1,
                                                   expectedCombination2,
                                                   expectedCombination3,
                                                   expectedCombination4,
                                                   expectedCombination5,
                                                   expectedCombination6,
                                                   expectedCombination7,
                                                   expectedCombination8,
                                               };

            foreach (var expected in expectedCombinationsList)
            {
                Assert.That(combinations.Any(x => x.Elements.AreElementsAreEqualTo(expected)), "Не найдена комбинация " + GetListString(expected));
            }
        }
        [Test]
        public void BasicTest2()
        {
            var object1 = 1;
            var object2 = 2;
            var object3 = 3;
            var list = new List<object> { object1, object2, object3};

            var combinationsGenerator = new CombinationsGenerator();
            var combinations = combinationsGenerator.GetCombinations(list);
            Assert.That(combinations.Count, Is.EqualTo(8));


            var expectedCombination1 = new List<object>();
            var expectedCombination2 = new List<object> { object1 };
            var expectedCombination3 = new List<object> { object1, object2 };
            var expectedCombination4 = new List<object> { object1, object3 };
            var expectedCombination5 = new List<object> { object1, object2, object3 };
            var expectedCombination6 = new List<object> { object2 };
            var expectedCombination7 = new List<object> { object2, object3 };
            var expectedCombination8 = new List<object> { object3 };

            var expectedCombinationsList = new List<List<object>>
                                               {
                                                   expectedCombination1,
                                                   expectedCombination2,
                                                   expectedCombination3,
                                                   expectedCombination4,
                                                   expectedCombination5,
                                                   expectedCombination6,
                                                   expectedCombination7,
                                                   expectedCombination8,
                                               };

            foreach (var expected in expectedCombinationsList)
            {
                Assert.That(combinations.Any(x => x.Elements.AreElementsAreEqualTo(expected)), "Не найдена комбинация " + GetListString(expected));
            }
        }

        [Test]
        [Timeout(50)]
        public void TimeBasedTest()
        {
            const int numberOfObjects = 5;

            var list = new List<object>();
            numberOfObjects.Times(() => list.Add(new object()));

            var combinationsGenerator = new CombinationsGenerator();
            var combinations = combinationsGenerator.GetCombinations(list);
        }

        [Test]
        [Timeout(12000)]
        public void TimeBasedTest2()
        {
            const int numberOfObjects = 12;

            var list = new List<object>();
            numberOfObjects.Times(() => list.Add(new object()));

            var combinationsGenerator = new CombinationsGenerator();
            var combinations = combinationsGenerator.GetCombinations(list);
        }

        [Test]
        public void EqualObjectsInCollectionTest()
        {
            var object1 = new object();
            var list = new List<object> { object1, object1};

            var combinationsGenerator = new CombinationsGenerator();
            var combinations = combinationsGenerator.GetCombinations(list);
            Assert.That(combinations.Count, Is.EqualTo(3));


            var expectedCombination1 = new List<object>();
            var expectedCombination2 = new List<object> { object1 };
            var expectedCombination3 = new List<object> { object1, object1 };

            var expectedCombinationsList = new List<List<object>>
                                               {
                                                   expectedCombination1,
                                                   expectedCombination2,
                                                   expectedCombination3,
                                               };

            foreach (var expected in expectedCombinationsList)
            {
                Assert.That(combinations.Any(x => x.Elements.AreElementsAreEqualTo(expected)), "Не найдена комбинация " + GetListString(expected));
            }
        }

        [Test]
        public void ValueTypesTest()
        {
            var list = new List<object> { 1, 1, 1};

            var combinationsGenerator = new CombinationsGenerator();
            var combinations = combinationsGenerator.GetCombinations(list);
            Assert.That(combinations.Count, Is.EqualTo(4));


            var expectedCombination1 = new List<object>();
            var expectedCombination2 = new List<object> { 1 };
            var expectedCombination3 = new List<object> { 1, 1 };
            var expectedCombination4 = new List<object> { 1, 1, 1 };

            var expectedCombinationsList = new List<List<object>>
                                               {
                                                   expectedCombination1,
                                                   expectedCombination2,
                                                   expectedCombination3,
                                                   expectedCombination4,
                                               };

            foreach (var expected in expectedCombinationsList)
            {
                Assert.That(combinations.Any(x => x.Elements.AreElementsAreEqualTo(expected)), "Не найдена комбинация " + GetListString(expected));
            }
        }

        private string GetListString<T>(List<T> list)
        {
            return "{" + list.Aggregate("", (r, e) => r + (r==""?"":" ") + e) + "}";
        }

        [Test]
        public void EmptyListTest()
        {
            var list = new List<int> {};
            var combinationsGenerator = new CombinationsGenerator();
            var combinations = combinationsGenerator.GetCombinations(list);
            Assert.That(combinations.Count, Is.EqualTo(1));
            Assert.That(combinations.Single().Elements.Count == 0);
        }
    }
}