using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSecuence : MonoBehaviour
{
    public List<RingScriptableObject> ringsData;
    public GameObject ringPrefab;
    public List<Ring> rings;
    public TeleportZone teleport;
    private float ringPlacementRadius = 10.0f;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitSecuence()
    {
        for (int i = 0; i < ringsData.Count; i++)
        {
            Vector3 randomDirectorVector = new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)).normalized;
            rings.Add(Instantiate(ringPrefab, transform.position + (randomDirectorVector * ringPlacementRadius), Quaternion.identity).GetComponent<Ring>());
            SetRingData(rings[i], ringsData[i]);
        }

        foreach (var item in rings)
        {
            item.gameObject.SetActive(false);
        }

        rings[0].gameObject.SetActive(true);
    }

    public void SetRingData(Ring ring, RingScriptableObject ringData)
    {
        ring.movementType = ringData.movementType;
        ring.maxSize = ringData.size;
        ring.speed = ringData.speed;
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
            rings[currentRing].gameObject.SetActive(false);
            GameManager.instance.delegateHandler.OnSecuenceCompleted();
        }
    }

    public void OnDisable()
    {
        DelegateHandler.ringCompleted -= AdvanceInSecuence;
        DelegateHandler.teleport -= InitSecuence;
    }
}
