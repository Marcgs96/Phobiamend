using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AracnophobiaScore
{
    public int objectivesScore;
    public float timeScore;
    public float dificultySMultiplier;
    public float totalScore;
}

public struct AracnophobiaLevelData
{
    public int numberOfSpiders;
    public int speedOfSpiders;
    public float sizeOfSpiders;
    public int timeToObserveSpider;
    public int timeToGrabSpider;
}

public class AracnophobiaLevel : MonoBehaviour
{
    public AracnophobiaLevelData levelData;
    public Transform spiderMovePoints;
    public List<Spider> spiders;
    public GameObject spiderPrefab;
    public Transform spawnPosition;
    public ObjectiveManager aracnophobiaObjectives;
    public GameObject visionObjectivePrefab;
    public Transform playerSpawnPosition;
    public Transform cameraUIOffset;
    public GameObject congratulations;
    public GameObject ray;

    //scores
    AracnophobiaScore scores;
    float highestTimeScore = 520.0f;
    float timeScoreTimer = 520.0f;

    void Start()
    {
        InitLevel();
        SetSpidersData();
    }

    private void FixedUpdate()
    {
        if(timeScoreTimer > 0.0f)
            timeScoreTimer -= Time.deltaTime;
    }

    private void OnEnable()
    {
        DelegateHandler.objective += OnObjectiveCompleted;
        DelegateHandler.objectives += EndLevel;
    }

    void OnObjectiveCompleted(int id)
    {
        switch (id)
        {
            case 1: FindSpiderObjective();
                break;
            case 2: EnableGrabSpiders();
                break;
            default:
                break;
        }

        scores.objectivesScore += 500;
    }

    void FindSpiderObjective()
    {
        int rand = Random.Range(0, spiders.Count - 1);
        GameObject target = Instantiate(visionObjectivePrefab, cameraUIOffset.position, cameraUIOffset.rotation);
        target.GetComponent<VisionObjective>().target = spiders[rand].transform;
    }

    void EnableGrabSpiders()
    {
        foreach (var item in spiders)
        {
            item.GetComponent<BoxCollider>().enabled = true;
        }
    }

    void InitLevel()
    {
        playerSpawnPosition = GameObject.Find("PlayerSpawn").transform;
        GameManager.instance.player.transform.position = playerSpawnPosition.transform.position;
        aracnophobiaObjectives = GameObject.Find("Level").GetComponent<ObjectiveManager>();
        cameraUIOffset = Camera.main.transform.GetChild(1);
        spiderMovePoints = GameObject.Find("SpidersMovePoints").transform;
        spawnPosition = GameObject.Find("SpidersSpawnPoint").transform;
        congratulations.SetActive(false);

        for(int i = 0; i < levelData.numberOfSpiders; ++i){
            spiders.Add(Instantiate(spiderPrefab, spawnPosition.position + new Vector3(Random.Range(-0.05f, 0.05f), 0.0f, Random.Range(-0.05f, 0.05f)), Quaternion.Euler(0.0f, Random.rotation.eulerAngles.y, 0.0f)).GetComponent<Spider>());
            spiders[i].movePosition = spiderMovePoints.GetChild(i);
            spiders[i].GetComponent<BoxCollider>().enabled = false;
        }

        ray = GameManager.instance.player.transform.GetChild(0).GetChild(3).gameObject;
        ray.SetActive(false);
    }

    void SetSpidersData()
    {
        foreach (var spider in spiders)
        {
            spider.maxSpeed = levelData.speedOfSpiders;
            spider.transform.localScale = new Vector3(levelData.sizeOfSpiders, levelData.sizeOfSpiders, levelData.sizeOfSpiders);        
        }
    }

    public void MoveSpiders()
    {
        foreach (var spider in spiders)
        {
            spider.SetState(Spider.SPIDER_STATE.MOVEMENT);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveSpiders();
        }
    }

    private void OnDisable()
    {
        DelegateHandler.objective -= OnObjectiveCompleted;
        DelegateHandler.objectives -= EndLevel;
    }

    public void EndLevel()
    {
        scores.timeScore = timeScoreTimer;
        scores.dificultySMultiplier = (levelData.numberOfSpiders + levelData.sizeOfSpiders + levelData.speedOfSpiders + levelData.timeToGrabSpider) / 4.0f;
        scores.totalScore = scores.objectivesScore + scores.timeScore * scores.dificultySMultiplier;
        aracnophobiaObjectives.SetScores(scores);
        congratulations.SetActive(true);
        ray.SetActive(true);
    }
}
