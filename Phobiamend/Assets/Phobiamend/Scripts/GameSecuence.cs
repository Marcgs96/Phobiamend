using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSecuence : MonoBehaviour
{
    public List<RingScriptableObject> ringsData;
    public GameObject ringPrefab;
    public List<Ring> rings;
    public TeleportZone teleport;
    private float ringPlacementRadius = 10.0f;
    public float timeToComplete = 30.0f;
    public float currentTime;
    public bool completed = false;
    public bool active = false;

    public GameObject userInterface;
    private Text timeText;
    private Text ringsCompletedText;

    public GameObject platform;

    private int currentRing = 0;
    public int platformSize;

    private void OnEnable()
    {
        DelegateHandler.ringCompleted += AdvanceInSecuence;
        DelegateHandler.teleport += InitSecuence;
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(1).localScale = new Vector3(platformSize, 1.0f, platformSize);
        teleport = transform.GetChild(0).GetComponent<TeleportZone>();
        timeText = userInterface.transform.GetChild(0).GetChild(1).GetComponent<Text>();
        ringsCompletedText = userInterface.transform.GetChild(0).GetChild(3).GetComponent<Text>();
        userInterface.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            currentTime -= Time.deltaTime;
            var ts = TimeSpan.FromSeconds(currentTime);
            timeText.text = string.Format("{0:00}:{1:00}", ts.TotalMinutes, ts.Seconds);
            ringsCompletedText.text = currentRing.ToString() + "/" + rings.Count.ToString();

            if (!completed && currentTime <= 0)
            {
                GameManager.instance.acrophobiaLevel.EndLevel(false);
                active = false;
            }
        }
    }

    public void InitSecuence()
    {
        teleport.SetToNoneState();
        teleport.gameObject.SetActive(false);

        for (int i = 0; i < ringsData.Count; i++)
        {
            Vector3 randomDirectorVector = new Vector3(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, -0.7f), UnityEngine.Random.Range(0.0f, 1.0f)).normalized;
            rings.Add(Instantiate(ringPrefab, transform.position + (randomDirectorVector * ringPlacementRadius), Quaternion.identity).GetComponent<Ring>());
            SetRingData(rings[i], ringsData[i]);
        }

        foreach (var item in rings)
        {
            item.gameObject.SetActive(false);
        }

        rings[0].gameObject.SetActive(true);

        currentTime = timeToComplete;
        userInterface.SetActive(true);
        active = true;

        Debug.Log("Secuence initialized");
    }

    public void Restart()
    {
        currentTime = 30;
        currentRing = 0;
        userInterface.SetActive(false);
    }

    public void SetRingData(Ring ring, RingScriptableObject ringData)
    {
        ring.movementType = ringData.movementType;
        ring.maxSize = ringData.size;
        ring.speed = ringData.speed;
        ring.mode = ringData.mode;
    }

    public void AdvanceInSecuence()
    {
        if (currentRing < rings.Count - 1)
        {
            rings[currentRing].gameObject.SetActive(false);
            currentRing++;
            rings[currentRing].gameObject.SetActive(true);
        }
        else
        {
            foreach (var item in rings)
            {
                Destroy(item.gameObject);
            }
            GameManager.instance.acrophobiaLevel.timeScore += (int)currentTime;
            GameManager.instance.delegateHandler.OnSecuenceCompleted();
        }

        Debug.Log("Advanced to ring" + currentRing);
    }

    public void CleanUp() {

        foreach (var item in rings)
        {
            Destroy(item.gameObject);
        }
    
    }


    public void OnDisable()
    {
        DelegateHandler.ringCompleted -= AdvanceInSecuence;
        DelegateHandler.teleport -= InitSecuence;
    }
}
