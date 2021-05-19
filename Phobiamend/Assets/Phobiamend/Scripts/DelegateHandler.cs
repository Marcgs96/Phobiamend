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
    public delegate void TargetedSpider(GameObject spider);
    public delegate void DeTargetedSpider();
    public delegate void ObjectiveComplete(int id);

    public static event RingCompleted ringCompleted;
    public static event SecuenceCompleted secuenceCompleted;
    public static event Teleport teleport;
    public static event EndLevel endlevel;
    public static event RestartLevel restartlevel;
    public static event TargetedSpider targetObjective;
    public static event DeTargetedSpider deTargetObjective;
    public static event ObjectiveComplete objective;

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
    public void OnTargetObjective(GameObject objective)
    {
        targetObjective(objective);
    }
    public void OnDeTargetObjective()
    {
        deTargetObjective();
    }
    public void OnObjectiveComplete(int id)
    {
        objective(id);
    }
}
