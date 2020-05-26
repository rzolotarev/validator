using System.Collections.Generic;

namespace OSA.Validator.FractionDistribution
{
    public class Combination<T>
    {
        public Combination(List<T> elements)
        {
            Elements = elements;
        }

        public List<T> Elements { get; private set; }
    }
}