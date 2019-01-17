using System.Linq;
using VirtoCommerce.Rating.Core.Services;

namespace VirtoCommerce.Rating.Data.Calculators
{
    /// <summary>
    /// Calculate rating by geting average value 
    /// </summary>
    public class AverageRatingCalculator : IRatingCalculator
    {
        public string Name => "Average";

        public float Calculate(int[] reviews)
        {
            if (reviews.Length == 0) return 0;
            return (float)reviews.Sum() / reviews.Length;
        }
    }
}
