using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct AcrophobiaLevelData
{
    public int platformsSize;
    public int platformsHeight;
    public float platformsTransparency;
}
public class AcrophobiaLevel : MonoBehaviour
{
    public List<GameSecuence> gameSecuences;

    public int currentSecuence = 0;

    public AcrophobiaLevelData levelData;

    public GameObject startPlatform;
    public GameObject finalPlatform;

    public int playerScore = 0;
    public int ringScore = 0;
    public int timeScore = 0;
    public int heightMultiplier = 0;

    // Start is called before the first frame update
    void Start()
    {
        startPlatform = GameObject.Find("StartingPlatform");
        finalPlatform = GameObject.Find("FinalPlatform");
        levelData.platformsHeight = 10;
        SetLevelData();
        InitLevel();
    }

    private void OnEnable()
    {
        DelegateHandler.secuenceCompleted += AdvanceSecuence;
    }

    void InitLevel()
    {
        GameManager.instance.player.transform.position = startPlatform.transform.position;
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

        startPlatform.transform.position = new Vector3(startPlatform.transform.position.x, levelData.platformsHeight, startPlatform.transform.position.z);
        finalPlatform.transform.position = new Vector3(finalPlatform.transform.position.x, levelData.platformsHeight, finalPlatform.transform.position.z);
    }

    public void EndLevel(bool win)
    {
        if (!win)
        {

        }
        else
        {

        }
    }
}
