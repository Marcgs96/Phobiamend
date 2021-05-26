using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    public Transform movePosition;
    public bool isWandering = false;
    public bool isWalking = false;
    public bool isRotatingRight = false;
    public bool isRotatingLeft = false;
    public float speed = 5.0f;
    public float rotSpeed = 5.0f;
    bool grabbed = false;
    GameObject distanceText;
    float distanceToPlayer = 0.0f;
    Rigidbody rigidbody;
    BoxCollider collider;

    //spider close
    float currentTimeClose;

    Rigidbody rbody;
    public enum SPIDER_STATE
    {
        NONE = -1,
        IDLE,
        MOVEMENT,
        GRABBED
    }

    public SPIDER_STATE state = SPIDER_STATE.NONE;

    // Start is called before the first frame update
    void Start()
    {
        currentTimeClose = GameManager.instance.aracnophobiaLevel.levelData.timeToGrabSpider;
        SetState(SPIDER_STATE.IDLE);
        rbody = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        distanceText = transform.GetChild(2).gameObject;
        distanceText.SetActive(false);
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case SPIDER_STATE.NONE:
                break;
            case SPIDER_STATE.IDLE:
                UpdateIdleState();
                break;
            case SPIDER_STATE.MOVEMENT:
                UpdateMoveState();
                break;
            case SPIDER_STATE.GRABBED:
                UpdateGrabbedState();
                break;
            default:
                break;
        }
    }

    void UpdateIdleState()
    {
        UpdateWander();
    }

    void UpdateMoveState()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePosition.position, 0.1f);
        if (Vector3.Distance(transform.position, movePosition.position) <= 0.1f)
            SetState(SPIDER_STATE.IDLE);
    }

    void UpdateGrabbedState()
    {
        distanceToPlayer = Vector3.Distance(transform.position, Camera.main.transform.position);
        if (distanceToPlayer >= 0.5f)
            distanceText.GetComponent<TextMesh>().text = "Acercame más!";
        if (distanceToPlayer < 0.5f && distanceToPlayer  >= 0.35f)
            distanceText.GetComponent<TextMesh>().text = "Acercame un poco más!";
        if (distanceToPlayer < 0.35f)
        {
            currentTimeClose -= Time.deltaTime;
            distanceText.GetComponent<TextMesh>().text = "Aquí! " + currentTimeClose.ToString("F1") + "s";
            if(currentTimeClose <= 0.1f)
            {
                GameManager.instance.aracnophobiaLevel.aracnophobiaObjectives.CompleteObjective("SpiderClose");
                Invoke("DisableDistanceText", 2.0f);
            }
        }

        if (currentTimeClose <= 0.1f)
            distanceText.GetComponent<TextMesh>().text = "Bien hecho!";

        distanceText.transform.LookAt(Camera.main.transform, Vector3.up);
    }

    void DisableDistanceText()
    {
        distanceText.SetActive(false);
    }

    public void CheckIfDropSpiderComplete()
    {
        if (GameManager.instance.aracnophobiaLevel.aracnophobiaObjectives.FindObjectiveByName("SpiderClose").completed)
        {
            GameManager.instance.aracnophobiaLevel.aracnophobiaObjectives.CompleteObjective("SpiderDrop");
        }
    }

    void UpdateWander()
    {
        if (isWandering == false)
        {
            StartCoroutine("Wander");
        }
        if (isRotatingRight)
            transform.Rotate(transform.up * rotSpeed * Time.deltaTime);
        if (isRotatingLeft)
            transform.Rotate(transform.up * -rotSpeed * Time.deltaTime);
        if (isWalking)
            transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void SpiderGrabbed()
    {
        distanceText.SetActive(true);
        grabbed = true;
        state = SPIDER_STATE.GRABBED;
        GameManager.instance.aracnophobiaLevel.aracnophobiaObjectives.CompleteObjective("SpiderTake");
    }

    public void SetState(SPIDER_STATE state)
    {
        switch (state)
        {
            case SPIDER_STATE.NONE:
                break;
            case SPIDER_STATE.IDLE:
                break;
            case SPIDER_STATE.MOVEMENT:
                break;
            case SPIDER_STATE.GRABBED:
                isWandering = false;
                break;
            default:
                break;
        }

        this.state = state;
    }
        IEnumerator Wander()
    {
        float rotateTime = 3.0f;
        int rotateWait = Random.Range(1, 5);
        int rotateLorR = Random.Range(1, 3);
        int walkWait = Random.Range(1, 5);
        float walkTime = 0.2f;

        isWandering = true;

        yield return new WaitForSeconds(walkWait);
        isWalking = true;
        yield return new WaitForSeconds(walkTime);
        isWalking = false;
        yield return new WaitForSeconds(rotateWait);

        if (rotateLorR == 1)
        {
            isRotatingRight = true;
            yield return new WaitForSeconds(rotateTime);
            isRotatingRight = false;
        }

        if (rotateLorR == 2)
        {
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotateTime);
            isRotatingLeft = false;
        }

        isWandering = false;
    }

    public void SetAsGrabbed(bool set)
    {
        grabbed = set;
    }

    public void SetRigidBodyAsKinematic(bool set)
    {
        rigidbody.isKinematic = set;
    }

    public void SetColliderAsTrigger(bool set)
    {
        collider.isTrigger = set;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!grabbed)
        {
            SetRigidBodyAsKinematic(true);
            SetColliderAsTrigger(true);
        }
    }
}
