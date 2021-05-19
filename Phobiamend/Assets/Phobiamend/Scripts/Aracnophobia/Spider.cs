using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    public Transform movePosition;
    Vector3 move = Vector3.zero;
    public bool isWandering = false;
    public bool isWalking = false;
    public bool isRotatingRight = false;
    public bool isRotatingLeft = false;
    public float speed = 5.0f;
    public float rotSpeed = 5.0f;
    bool grabbed = false;

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
        SetState(SPIDER_STATE.IDLE);
        rbody = GetComponent<Rigidbody>();
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

    void StartIdleState(SPIDER_STATE previousState)
    {

    }

    void StartMoveState(SPIDER_STATE previousState)
    {

    }

    void StartGrabbedState(SPIDER_STATE previousState)
    {
        isWandering = false;
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

    public void SetState(SPIDER_STATE state)
    {
        switch (state)
        {
            case SPIDER_STATE.NONE:
                break;
            case SPIDER_STATE.IDLE:
                StartIdleState(this.state);
                break;
            case SPIDER_STATE.MOVEMENT:
                StartMoveState(this.state);
                break;
            case SPIDER_STATE.GRABBED:
                StartGrabbedState(this.state);
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
}
