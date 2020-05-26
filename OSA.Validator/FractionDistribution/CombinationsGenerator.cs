using System;
using System.Collections.Generic;
using System.Linq;
using OSA.Core.Util;
using OSA.Core.Util.Extensions.Objects;

namespace OSA.Validator.FractionDistribution
{
    public class CombinationsGenerator
    {
        /// <summary>
        /// Выдает комбинации выборки элементов из переданного множества объектов. При передаче
        /// в метод одинаковых объектов, комбинации, получающиеся их перестановкой выдаваться не будут.
        /// </summary>
        /// <typeparam name="T">Тип передаваемых объектов</typeparam>
        /// <param name="elements">Элементы для получения комбинаций их сочитаний</param>
        /// <returns></returns>
        public List<Combination<T>> GetCombinations<T>(List<T> elements)
        {
            if (elements.Count == 0) return new List<Combination<T>> { new Combination<T>(new List<T>())};
            if (elements.Count == 1) return new List<Combination<T>> { new Combination<T>(new List<T>()), new Combination<T>(elements)};

            var tailCombinations = GetCombinations(elements.Skip(1).ToList());
            var head = elements.First();
            var combinationsWithHead = tailCombinations
                .Select(comb =>
                            {
                                var elementsWithHead = new List<T>();
                                elementsWithHead.AddRange(comb.Elements);
                                elementsWithHead.Add(head);
                                return new Combination<T>(elementsWithHead);
                            });
            var combinationsWithoutHead = tailCombinations;
            var resultCombinations = combinationsWithHead.Union(combinationsWithoutHead).ToList();
            return Distinct(resultCombinations.ToList());
        }

        // TODO: переделать этот метод или метод, его использующий
        // Очень медленный алгоритм сравнения (для 15+ элементов).
        // Его возможно сильно ускорить за счет
        // форсирования where T:IComparable, сортировки всех массивов и применение
        // SequenceEqual
        private static List<Combination<T>> Distinct<T>(List<Combination<T>> list)
        {
            var result = new List<Combination<T>>();
            foreach (var combination in list)
            {
                if (!result.Any(r=> r.Elements.AreElementsAreEqualTo(combination.Elements)))
                {
                    result.Add(combination);
                }
            }
            return result;
        }


        /// <summary>
        /// Получает все варианты раскладки элементов по корзинам. (Например, массив {1, 2, 3} по двум
        /// корзинам можно разложить так: первая корзина {1, 2}, вторая корзина {3}. При передаче в метод
        /// одинаковых объектов, комбинации, получающиеся их перестановкой выдаваться не будут.
        /// </summary>
        /// <typeparam name="T">Тип передаваемых объектов</typeparam>
        /// <param name="elements">Элементы для раскладки по корзинам</param>
        /// <param name="numberOfBaskets">Число корзин, по которым раскладываем</param>
        /// <returns></returns>
        public List<BasketCombination<T>> GetCombinationsInBaskets<T>(List<T> elements, int numberOfBaskets)
        {
            #region Assertions
            Check.That(numberOfBaskets >= 0, "Минимальное число корзин - ноль.");
            #endregion

            if (numberOfBaskets == 0)
            {
                return new List<BasketCombination<T>>();
            }

            if (numberOfBaskets == 1)
            {
                var basket = new Basket<T>(elements);
                var basketCombination = new BasketCombination<T>(new List<Basket<T>>{basket});
                return new List<BasketCombination<T>>{basketCombination};
            }


            var result = new List<BasketCombination<T>>();

            var combinations = GetCombinations(elements);
            foreach (var combination in combinations)
            {
                var remainingElements = GetRemainingObjects(elements, combination.Elements);
                var innerCombinations = GetCombinationsInBaskets(remainingElements, numberOfBaskets - 1);

                foreach (var innerCombination in innerCombinations)
                {
                    var currentBasket = new Basket<T>(combination.Elements);
                    var otherBaskets = innerCombination.Baskets;
                    var resultBasketList = new List<Basket<T>>();
                    resultBasketList.Add(currentBasket);
                    resultBasketList.AddRange(otherBaskets);
                    var basketCombination = new BasketCombination<T>(resultBasketList);
                    result.Add(basketCombination);
                }
            }

            return result;
        }

        private List<T> GetRemainingObjects<T>(List<T> elements, List<T> elementsToRemove)
        {
            var result = new List<T>(elements);
            for (int i = 0; i < elementsToRemove.Count; i++)
            {
                var elementToRemove = elementsToRemove[i];
                var elementWasDeleted = result.Remove(elementToRemove);
                if (!elementWasDeleted) throw new ArgumentOutOfRangeException("Элемент "+ elementsToRemove +" не найден.");
            }
            return result;
        }
    }

    public class BasketCombination<T>
    {
        public BasketCombination(List<Basket<T>> baskets)
        {
            Baskets = baskets;
        }

        public List<Basket<T>> Baskets { get; private set; }
    }

    public class Basket<T>
    {
        public Basket(List<T> basketElements)
        {
            BasketElements = basketElements;
        }

        public List<T> BasketElements { get; private set; }
    }
}