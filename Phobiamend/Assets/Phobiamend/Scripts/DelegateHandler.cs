using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateHandler : MonoBehaviour
{
    public delegate void RingCompleted();
    public delegate void SecuenceCompleted();
    public delegate void Teleport();

    public static event RingCompleted ringCompleted;
    public static event SecuenceCompleted secuenceCompleted;
    public static event Teleport teleport;

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
}
