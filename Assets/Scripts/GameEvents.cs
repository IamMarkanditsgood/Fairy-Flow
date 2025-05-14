using System;

public static class GameEvents
{
    public static event Action<int> OnNewCoins;
    public static event Action<int> OnNewReward;
    public static event Action<int> OnNewTime;

    public static void NewCoins(int coins) => OnNewCoins?.Invoke(coins);
    public static void NewReward(int reward) => OnNewReward?.Invoke(reward);
    public static void NewTime(int time) => OnNewTime?.Invoke(time);
}
