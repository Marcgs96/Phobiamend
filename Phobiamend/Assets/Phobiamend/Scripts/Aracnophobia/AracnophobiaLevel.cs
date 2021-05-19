using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AracnophobiaLevelData
{
    public int numberOfSpiders;
    public int speedOfSpiders;
    public float sizeOfSpiders;
    public float timeToObserveSpider;
    public float timeToGrabSpider;
}

public class AracnophobiaLevel : MonoBehaviour
{
    AracnophobiaLevelData levelData;
    public Transform spiderMovePoints;
    public List<Spider> spiders;
    public GameObject spiderPrefab;
    public Transform spawnPosition;
    public ObjectiveManager aracnophobiaObjectives;
    public GameObject visionObjectivePrefab;

    void Start()
    {
        levelData.numberOfSpiders = 5;
        InitLevel();
    }

    private void OnEnable()
    {
        DelegateHandler.objective += OnObjectiveCompleted;
    }

    void OnObjectiveCompleted(int id)
    {
        switch (id)
        {
            case 1:
                FindSpiderObjective();
                break;
            case 2: EnableGrabSpiders();
                break;
            default:
                break;
        }
    }

    void FindSpiderObjective()
    {
        int rand = Random.Range(0, spiders.Count - 1);
        Instantiate(visionObjectivePrefab, spiders[rand].transform.position, spiders[rand].transform.rotation);
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
        for(int i = 0; i < levelData.numberOfSpiders; ++i){
            spiders.Add(Instantiate(spiderPrefab, spawnPosition.position + new Vector3(Random.Range(-0.1f, 0.1f), 0.0f, Random.Range(-0.1f, 0.2f)), Quaternion.Euler(0.0f, Random.rotation.eulerAngles.y, 0.0f)).GetComponent<Spider>());
            spiders[i].movePosition = spiderMovePoints.GetChild(i);
            spiders[i].GetComponent<BoxCollider>().enabled = false;
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
    }
}
