using System;
using UnityEngine;

public class EventManager
{
    public static Action OnKnifeAllTheWayDown;
    public static void KnifeAllTheWayDown()
    {
        OnKnifeAllTheWayDown?.Invoke();
    }

    public static Action OnKnifeAllTheWayUp;
    public static void KnifeAllTheWayUp()
    {
        OnKnifeAllTheWayUp?.Invoke();
    }

    public static Action OnGameOver;
    public static void GameOver()
    {
        OnGameOver?.Invoke();
    }
}
