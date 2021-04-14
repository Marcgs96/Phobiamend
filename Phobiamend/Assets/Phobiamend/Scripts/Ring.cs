using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    bool beingHitted;
    float maxSize = 1.0f;
    float currentSize = 1.0f;
    float minSize = 0.2f;
    int score = 10;
    SphereCollider col;
    // Start is called before the first frame update
    void Start()
    {
        col = this.GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator RingBeingHitted()
    {
        while (currentSize >= 0.2f)
        {
            transform.localScale -= new Vector3(0.01f, 0.01f, 0.0f);
            currentSize -= 0.01f;
            col.radius -= 0.01f;
            GameManager.instance.playerScore += score;
            yield return new WaitForSeconds(0.01f);
        }

        //ring ended lel
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
