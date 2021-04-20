using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct LevelData
{
    public int platformsSize;
    public int platformsHeight;
    public float platformsTransparency;
}
public class AcrophobiaLevel : MonoBehaviour
{
    public List<GameSecuence> gameSecuences;

    public LevelData levelData;

    public int currentSecuence = 0;

    // Start is called before the first frame update
    void Start()
    {
        InitLevel();
        levelData.platformsHeight = 10;
        levelData.platformsSize = 1;
        levelData.platformsTransparency = 0.2f;
        SetLevelData();
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
            gs.enabled = false;
        }

        gameSecuences[0].enabled = true;
        gameSecuences[0].teleport.SetToInState();
        currentSecuence = 0;
    }

    void AdvanceSecuence() {
        gameSecuences[currentSecuence].enabled = false;
        if (currentSecuence < gameSecuences.Count - 1)
        {
            currentSecuence++;
            gameSecuences[currentSecuence].enabled = true;
            gameSecuences[currentSecuence].teleport.SetToInState();
        }
        else
        {
            Debug.Log("Game Ended");
        }
    }

    void SetLevelData()
    {
        foreach (var item in gameSecuences)
        {
            item.platform.transform.localScale = new Vector3(levelData.platformsSize, 1.0f, levelData.platformsSize);
            item.transform.position = new Vector3(item.transform.position.x, levelData.platformsHeight, item.transform.position.z);
            item.platform.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_BaseColor", new Color(1.0f, 1.0f, 1.0f, levelData.platformsTransparency));
        }
    }

}
