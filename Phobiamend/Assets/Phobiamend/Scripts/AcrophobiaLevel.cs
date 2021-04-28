using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


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
    private GameObject scoreUI;
    public TeleportZone endTeleport;

    public int playerScore = 0;
    public int ringScore = 0;
    public int timeScore = 0;
    public int heightMultiplier = 0;

    // Start is called before the first frame update
    void Start()
    {
        startPlatform = GameObject.Find("StartingPlatform");
        finalPlatform = GameObject.Find("FinalPlatform");
        endTeleport = GameObject.Find("EndTeleport").GetComponent<TeleportZone>();
        scoreUI = finalPlatform.transform.GetChild(0).gameObject;
        SetLevelData();
        InitLevel();
    }

    private void OnEnable()
    {
        DelegateHandler.secuenceCompleted += AdvanceSecuence;
        DelegateHandler.endlevel += EndLevel;
        DelegateHandler.restartlevel += RestartLevel;
    }

    void InitLevel()
    {
        GameManager.instance.player.transform.position = startPlatform.transform.position;
        foreach (GameSecuence gs in gameSecuences)
        {
            gs.teleport.SetToOutState();
            gs.enabled = false;
        }

        endTeleport.SetToOutState();

        gameSecuences[0].enabled = true;
        gameSecuences[0].teleport.SetToInState();
        currentSecuence = 0;

        playerScore = 0;
        ringScore = 0;
        timeScore = 0;
        heightMultiplier = 0;
    }

    public void RestartLevel()
    {
        foreach (var item in gameSecuences)
        {
            item.enabled = true;
            item.Restart();
        }
        scoreUI.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "0000";
        scoreUI.transform.GetChild(1).GetChild(3).GetComponent<Text>().text = "0000";
        scoreUI.transform.GetChild(1).GetChild(5).GetComponent<Text>().text = "0000";
        scoreUI.transform.GetChild(1).GetChild(7).GetComponent<Text>().text = "0000";
        InitLevel();
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
            endTeleport.SetToInState();
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
            endTeleport.SetToNoneState();
            endTeleport.gameObject.SetActive(false);
            CalculateScore();
        }
    }

    public void CalculateScore()
    {
        heightMultiplier = levelData.platformsHeight / 5;
        playerScore = (ringScore + timeScore) * heightMultiplier;

        scoreUI.transform.GetChild(1).GetChild(1).GetComponent<Text>().DOText(ringScore.ToString(), 1.5f, true, ScrambleMode.Numerals);
        scoreUI.transform.GetChild(1).GetChild(3).GetComponent<Text>().DOText(timeScore.ToString(), 2.5f, true, ScrambleMode.Numerals);
        scoreUI.transform.GetChild(1).GetChild(5).GetComponent<Text>().DOText(heightMultiplier.ToString(), 3.5f, true, ScrambleMode.Numerals);
        scoreUI.transform.GetChild(1).GetChild(7).GetComponent<Text>().DOText(playerScore.ToString(), 4.5f, true, ScrambleMode.Numerals);
    }

}
