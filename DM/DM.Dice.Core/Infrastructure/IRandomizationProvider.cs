namespace DM.Dice.Core.Infrastructure
{
    public interface IRandomizationProvider
    {
        int Next(int maxValue);
    }
}