using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public DelegateHandler delegateHandler;

    public int playerScore = 0;

    public GameObject player;

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
    // Start is called before the first frame update
    void Start()
    {
        delegateHandler = GetComponent<DelegateHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
