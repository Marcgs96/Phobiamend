using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateHandler : MonoBehaviour
{
    public delegate void RingCompleted();
    public delegate void SecuenceCompleted();
    public delegate void Teleport();
    public delegate void EndLevel(bool win);
    public delegate void RestartLevel();

    public static event RingCompleted ringCompleted;
    public static event SecuenceCompleted secuenceCompleted;
    public static event Teleport teleport;
    public static event EndLevel endlevel;
    public static event RestartLevel restartlevel;

    public void OnRingCompleted()
    {
        ringCompleted();
    }

    public void OnSecuenceCompleted()
    {
        secuenceCompleted();
    }
    
    public void OnTeleport()
    {
        teleport();
    }

    public void OnEndLevel(bool win)
    {
        endlevel(win);
    }
    public void OnRestartLevel()
    {
        restartlevel();
    }
}
