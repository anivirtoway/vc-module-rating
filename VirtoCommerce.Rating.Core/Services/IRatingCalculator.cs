namespace VirtoCommerce.Rating.Core.Services
{
    public interface IRatingCalculator
    {
        // todo: make translation
        string Name { get; }
        float Calculate(int[] ratings);
    }
}
