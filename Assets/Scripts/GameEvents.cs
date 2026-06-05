using System;

public static class GameEvents
{
    public static event Action OnStartGame;
    public static event Action OnTutorialCompleted;
    public static event Action OnLevelCompleted;
    public static event Action OnPlayerWon;
    public static event Action OnPlayerDied;

    public static void RaiseStartGame() => OnStartGame?.Invoke();
    public static void RaiseTutorialCompleted() => OnTutorialCompleted?.Invoke();
    public static void RaiseLevelCompleted() => OnLevelCompleted?.Invoke();
    public static void RaisePlayerWon() => OnPlayerWon?.Invoke();
    public static void RaisePlayerDied() => OnPlayerDied?.Invoke();
}
