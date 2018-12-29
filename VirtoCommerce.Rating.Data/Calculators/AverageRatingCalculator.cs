using System.Linq;
using VirtoCommerce.Rating.Core.Services;

namespace VirtoCommerce.Rating.Data.Calculators
{
    /// <summary>
    /// Calculate rating by geting average value 
    /// </summary>
    public class AverageRatingCalculator : IRatingCalculator
    {
        public string Name => "AverageRating";

        public float Calculate(int[] ratings)
        {
            if (ratings.Length == 0) return 0;
            return (float)ratings.Sum() / ratings.Length;
        }
    }
}
