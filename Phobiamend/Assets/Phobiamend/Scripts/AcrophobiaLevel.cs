using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;


public struct AcrophobiaLevelData
{
    public int platformsSize;
    public int platformsHeight;
    public float platformsTransparency;
}

public struct AcrophobiaScore
{
    public int objectivesScore;
    public float timeScore;
    public float dificultySMultiplier;
    public float totalScore;
}

public class AcrophobiaLevel : MonoBehaviour
{
    public List<GameSecuence> gameSecuences;

    public int currentSecuence = 0;

    public AcrophobiaLevelData levelData;
    public AcrophobiaScore scores;

    public GameObject startPlatform;
    public GameObject finalPlatform;
    private GameObject scoreUI;
    public TeleportZone endTeleport;
    public GameObject congratulations;

    public int playerScore = 0;
    public int ringScore = 0;
    public int timeScore = 0;
    public int heightMultiplier = 0;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        InitLevel();
        SetLevelData();
        scoreUI = finalPlatform.transform.GetChild(0).gameObject;
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
        GameObject[] gameSecuencesGameObjects = GameObject.FindGameObjectsWithTag("GameSecuence");
        foreach (GameObject gs in gameSecuencesGameObjects)
        {
            gameSecuences.Add(gs.GetComponent<GameSecuence>());
            gs.GetComponent<GameSecuence>().teleport.SetToOutState();
            gs.GetComponent<GameSecuence>().enabled = false;
        }

        //endTeleport.SetToOutState();

        gameSecuences[0].enabled = true;
        gameSecuences[0].teleport.SetToInState();
        currentSecuence = 0;

        playerScore = 0;
        ringScore = 0;
        timeScore = 0;
        heightMultiplier = 0;
        congratulations.SetActive(false);
    }

    public void RestartLevel()
    {
        foreach (var item in gameSecuences)
        {
            item.enabled = true;
            item.Restart();
        }
        scoreUI.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = "0000";
        scoreUI.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>().text = "0000";
        scoreUI.transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<Text>().text = "0000";
        scoreUI.transform.GetChild(2).GetChild(3).GetChild(0).GetComponent<Text>().text = "0000";
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
            congratulations.SetActive(true);
        }
    }

    public void CalculateScore()
    {
        scores.timeScore = timeScore;
        scores.objectivesScore = ringScore;
        scores.dificultySMultiplier = (levelData.platformsHeight * (5 - levelData.platformsSize) * (1.0f - levelData.platformsTransparency)) / 3.0f;
        scores.totalScore = scores.objectivesScore + scores.timeScore * scores.dificultySMultiplier;

        scoreUI.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = scores.objectivesScore.ToString();
        scoreUI.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>().text = scores.timeScore.ToString("F1");
        scoreUI.transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<Text>().text = scores.dificultySMultiplier.ToString("F1");
        scoreUI.transform.GetChild(2).GetChild(3).GetChild(0).GetComponent<Text>().text = scores.totalScore.ToString("F1");
    }

}
