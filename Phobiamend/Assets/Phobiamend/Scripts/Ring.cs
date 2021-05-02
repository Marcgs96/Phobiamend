using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;

public class Ring : MonoBehaviour
{
    public enum MODE { ONESHOT, CHANNEL, MOVEMENT };
    public MODE mode = MODE.ONESHOT;

    public float speed = 1.0f;

    bool beingHitted;
    public float maxSize = 1.0f;
    float currentSize = 1.0f;
    float minSize = 0.2f;
    int completionScore = 500;
    int channelTickScore = 100;
    int timesHitted = 0;
    int totalTimesToHit = 3;
    BoxCollider col;

    GameObject scoreText;

    Vector3 move;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider>();
        scoreText = transform.GetChild(0).gameObject;
        scoreText.GetComponent<MeshRenderer>().material.DOFade(0.0f, 0.0f);
        transform.LookAt(GameManager.instance.player.transform);

        switch (mode)
        {
            case MODE.ONESHOT:
                break;
            case MODE.CHANNEL:
                currentSize = maxSize;
                transform.localScale = new Vector3(currentSize, currentSize, currentSize);
                break;
            case MODE.MOVEMENT:
                move = Vector3.right * speed;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(GameManager.instance.player.transform);
    }

    IEnumerator RingBeingHitted()
    {
        StartCoroutine(ShowChannelingScore(1.0f));
        while (currentSize >= 1.0f)
        {
            transform.localScale -= new Vector3(0.01f, 0.01f, 0.0f);
            currentSize -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        CompleteRing();
    }


    IEnumerator ShowChannelingScore(float time)
    {
        while (beingHitted)
        {
            ShowScore(channelTickScore, time);
            GameManager.instance.acrophobiaLevel.ringScore += channelTickScore;
            yield return new WaitForSeconds(time);
        }
    }

    IEnumerator MoveRing(float time)
    {
        float currentTime = 0.0f;
        move = Random.insideUnitSphere;
        while(currentTime <= time)
        {
            currentTime += Time.deltaTime;
            transform.localPosition += move * speed * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    public void Activate()
    {
        switch (mode)
        {
            case MODE.ONESHOT:
                CompleteRing();
                break;
            case MODE.CHANNEL:
                beingHitted = true;
                StartCoroutine("RingBeingHitted");
                break;
            case MODE.MOVEMENT:
                timesHitted += 1;

                if (timesHitted == totalTimesToHit)
                {
                    CompleteRing();
                }
                else
                {
                    StartCoroutine(MoveRing(0.35f));
                    ShowScore(channelTickScore, 1.0f);
                }
                break;
            default:
                break;
        }
    }

    public void Deactivate()
    {
        if (beingHitted)
        {
            beingHitted = false;
            StopCoroutine("RingBeingHitted");
        }
    }

    public void CompleteRing() {
        Vector3 newScale = transform.localScale + new Vector3(1.0f, 1.0f, 1.0f);
        transform.DOScale(newScale, 1.0f);
        GetComponent<MeshRenderer>().material.DOFade(0.0f, 1.0f);
        beingHitted = false;
        StopCoroutine("RingBeingHitted");
        Invoke("OnCompleteRing", 1.5f);
        ShowScore(completionScore, 1.0f);
        GameManager.instance.acrophobiaLevel.ringScore += completionScore;
        col.enabled = false;
    }

    public void ShowScore(int score, float showTime)
    {
        scoreText.transform.DOScale(2.0f, 0.5f);
        scoreText.GetComponent<TextMesh>().text = "+" + score.ToString();
        scoreText.GetComponent<MeshRenderer>().material.DOFade(1.0f, showTime * 0.5f);
        scoreText.transform.DOLocalMoveY(1.0f, showTime).OnComplete(ResetScoreTextPosition);
        StartCoroutine(HideScore((float)(showTime * 0.5)));
    }

    IEnumerator HideScore(float hideTime)
    {
        yield return new WaitForSeconds(hideTime);

        scoreText.transform.DOScale(1.0f, hideTime);
        scoreText.GetComponent<MeshRenderer>().material.DOFade(0.0f, hideTime);
    }

    public void ResetScoreTextPosition()
    {
        scoreText.transform.localPosition = new Vector3(-1.5f, 0.0f, 0.0f);
    }

    public void OnCompleteRing()
    {
        GameManager.instance.delegateHandler.OnRingCompleted();
    }

}
