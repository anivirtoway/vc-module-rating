namespace VirtoCommerce.Rating.Core.Services
{
    public interface IRatingCalculator
    {
        string Name { get; }
        float Calculate(int[] reviews);
    }
}
