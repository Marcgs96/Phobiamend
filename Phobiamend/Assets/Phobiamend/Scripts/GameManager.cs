using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public DelegateHandler delegateHandler;
    public AcrophobiaLevel acrophobiaLevel;
    public AracnophobiaLevel aracnophobiaLevel;

    public GameObject player;

    public AcrophobiaLevelData acrophobiaLevelData;
    public AracnophobiaLevelData aracnophobiaLevelData;

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;//Avoid doing anything else
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject;
        delegateHandler = GetComponent<DelegateHandler>();
    }

    public void StartLevel(string level)
    {
       SceneManager.LoadScene(level);
    }

    public void ExposureLevelUp(string level)
    {
        if (level == "Aracnophobia")
        {
            if (aracnophobiaLevelData.numberOfSpiders < 10)
                aracnophobiaLevelData.numberOfSpiders++;
            if (aracnophobiaLevelData.speedOfSpiders < 5)
                aracnophobiaLevelData.speedOfSpiders++;
            if (aracnophobiaLevelData.sizeOfSpiders < 1.0f)
                aracnophobiaLevelData.sizeOfSpiders += 0.1f;
            if (aracnophobiaLevelData.timeToGrabSpider < 5)
            {
                aracnophobiaLevelData.timeToGrabSpider++;
                aracnophobiaLevelData.timeToObserveSpider++;
            }
        }

        SceneManager.LoadScene(level);
    }

    //Acrophobia
    public void SetPlatformHeightData(float height)
    {
        acrophobiaLevelData.platformsHeight = (int)height;
    }

    public int GetPlatformHeightData()
    {
        return acrophobiaLevelData.platformsHeight;
    }

    public void SetPlatformSizeData(float size)
    {
        acrophobiaLevelData.platformsSize = (int)size;
    }

    public int GetPlatformSizeData()
    {
        return acrophobiaLevelData.platformsSize;
    }

    public void SetPlatformTransparencyData(float transparency)
    {
        acrophobiaLevelData.platformsTransparency = transparency;
    }

    public float GetPlatformTransparencyData()
    {
        return acrophobiaLevelData.platformsTransparency;
    }

    //Aracnophobia
    public void SetNumberOfSpiders(float num)
    {
        aracnophobiaLevelData.numberOfSpiders = (int)num;
    }

    public void SetSpeedOfSpiders(float speed)
    {
        aracnophobiaLevelData.speedOfSpiders = (int)speed;
    }
    public void SetSizeofSpiders(float size)
    {
        aracnophobiaLevelData.sizeOfSpiders = size;
    }
    public void SetTimeOfEvents(float time)
    {
        aracnophobiaLevelData.timeToGrabSpider = (int)time;
        aracnophobiaLevelData.timeToObserveSpider = (int)time;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "PhobiamendHall")
        {
            transform.position = new Vector3(0, 0, 0);
        }
        else
        {
            SetLevelData(scene.name);
        }

        SetCanvasUIEventCamera();
    }

    public void SetCanvasUIEventCamera()
    {
        GameObject[] canvas = GameObject.FindGameObjectsWithTag("Canvas");
        foreach (var item in canvas)
        {
            item.GetComponent<Canvas>().worldCamera = Camera.main;
        }
    }

    void SetLevelData(string level)
    {
        if (level.Equals("Acrophobia"))
        {
            acrophobiaLevel = gameObject.AddComponent<AcrophobiaLevel>();
            acrophobiaLevel.levelData = acrophobiaLevelData;
            acrophobiaLevel.gameSecuences = GameObject.Find("Level").GetComponent<AcrophobiaLevelContainer>().gameSecuences; //Contains the GameSecuences of the level.
        }

        if (level.Equals("Aracnophobia"))
        {
            aracnophobiaLevel = GameObject.Find("Level").GetComponent<AracnophobiaLevel>();
            aracnophobiaLevel.levelData = aracnophobiaLevelData;
        }
    }

}
