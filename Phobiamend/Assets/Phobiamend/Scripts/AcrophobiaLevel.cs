using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcrophobiaLevel : MonoBehaviour
{
    public List<GameSecuence> gameSecuences;

    public int currentSecuence = 0;

    // Start is called before the first frame update
    void Start()
    {
        InitLevel();
    }

    private void OnEnable()
    {
        DelegateHandler.secuenceCompleted += AdvanceSecuence;
    }

    void InitLevel()
    {
        foreach (GameSecuence gs in gameSecuences)
        {
            gs.teleport.SetToOutState();
        }

        gameSecuences[0].teleport.SetToInState();
        currentSecuence = 0;
    }

    void AdvanceSecuence() {
        gameSecuences[currentSecuence].teleport.SetToNoneState();
        if (currentSecuence < gameSecuences.Count - 1)
        {
            currentSecuence++;
            gameSecuences[currentSecuence].teleport.SetToInState();
        }
        else
        {
            Debug.Log("Game Ended");
        }
    }

}
