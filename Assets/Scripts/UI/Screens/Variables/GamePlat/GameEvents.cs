using System;

public static class GameEvents 
{
    public static event Action OnCoinsUpdate;
    public static event Action OnEnergyUpdate;

    public static void UpdateCoins() => OnCoinsUpdate?.Invoke();
    public static void UpdateEnergy () => OnEnergyUpdate?.Invoke();
}
