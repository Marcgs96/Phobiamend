using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Ring : MonoBehaviour
{
    public enum MOVEMENT_TYPE { NONE, UP, DOWN, LEFT, RIGHT, UPLEFT, UPRIGHT, DOWNLEFT, DOWNRIGHT, CIRCULAR};
    public MOVEMENT_TYPE movementType = MOVEMENT_TYPE.NONE;
    public float speed = 1.0f;

    bool beingHitted;
    public float maxSize = 1.0f;
    float currentSize = 1.0f;
    float minSize = 0.2f;
    int score = 10;
    SphereCollider col;

    Vector3 move;
    // Start is called before the first frame update
    void Start()
    {
        col = this.GetComponent<SphereCollider>();
        currentSize = maxSize;
        transform.localScale = new Vector3(currentSize, currentSize, currentSize);

        switch (movementType)
        {
            case MOVEMENT_TYPE.NONE:
                move = Vector3.zero;
                break;
            case MOVEMENT_TYPE.UP:
                move = Vector3.up;
                break;
            case MOVEMENT_TYPE.DOWN:
                move = Vector3.down;
                break;
            case MOVEMENT_TYPE.LEFT:
                move = Vector3.left;
                break;
            case MOVEMENT_TYPE.RIGHT:
                move = Vector3.right;
                break;
            case MOVEMENT_TYPE.UPLEFT:
                move = Vector3.up + Vector3.left;
                break;
            case MOVEMENT_TYPE.UPRIGHT:
                move = Vector3.up + Vector3.right;
                break;
            case MOVEMENT_TYPE.DOWNLEFT:
                move = Vector3.down + Vector3.left;
                break;
            case MOVEMENT_TYPE.DOWNRIGHT:
                move = Vector3.down + Vector3.right;
                break;
            case MOVEMENT_TYPE.CIRCULAR:

                break;
            default:
                break;
        }

        move *= speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(GameManager.instance.player.transform);
    }

    IEnumerator RingBeingHitted()
    {
        while (currentSize >= 0.2f)
        {
            transform.localScale -= new Vector3(0.01f, 0.01f, 0.0f);
            currentSize -= 0.01f;
            col.radius -= 0.01f;
            transform.position += move * Time.deltaTime;
            GameManager.instance.playerScore += score;
            yield return new WaitForSeconds(0.01f);
        }

        GameManager.instance.delegateHandler.OnRingCompleted();
    }

    public void Activate()
    {
        beingHitted = true;
        StartCoroutine("RingBeingHitted");
    }

    public void Deactivate()
    {
        beingHitted = false;
        StopCoroutine("RingBeingHitted");
    }
}
