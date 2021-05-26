using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VisionObjective : MonoBehaviour
{

    public Transform target;
    private void OnEnable()
    {
        DelegateHandler.targetObjective += ScaleTarget;
        DelegateHandler.deTargetObjective += CancelTargetScale;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, target.position.x, Time.deltaTime * 2.0f),
                                         Mathf.Lerp(transform.position.y, target.position.y, Time.deltaTime * 2.0f),
                                         Mathf.Lerp(transform.position.z, target.position.z, Time.deltaTime * 2.0f));
        if (DOTween.TweensById("scaleTarget") != null)
        {
            float time = GameManager.instance.aracnophobiaLevel.levelData.timeToGrabSpider - DOTween.TweensById("scaleTarget").ToArray()[0].Elapsed();
            transform.GetChild(0).GetComponent<TextMesh>().text = time.ToString("F1") + "s";
        }
    }

    void ScaleTarget(GameObject target)
    {
        if(DOTween.TweensById("scaleTarget") != null)
        {
            DOTween.Play("scaleTarget");
        }
        else {
            target.transform.DOScale(0.08f, GameManager.instance.aracnophobiaLevel.levelData.timeToGrabSpider).SetId("scaleTarget").OnComplete(CompleteVisionTarget);
        }
    }

    void CancelTargetScale()
    {
        if (DOTween.IsTweening("scaleTarget"))
            DOTween.Pause("scaleTarget");
    }

    void CompleteVisionTarget()
    {
        transform.DOScale(0.2f, 1.0f).OnComplete(CompletionEffect);
        transform.GetComponent<MeshRenderer>().material.DOFade(0.0f, 1.0f);
        transform.GetChild(0).GetComponent<TextMesh>().text = "+" + 500.ToString();
        transform.GetChild(1).GetComponent<TextMesh>().text = "Bien Hecho!";
        GetComponent<BoxCollider>().enabled = false;
        GameManager.instance.aracnophobiaLevel.aracnophobiaObjectives.CompleteObjective("SpiderLook");
    }

    void CompletionEffect()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        DelegateHandler.targetObjective -= ScaleTarget;
        DelegateHandler.deTargetObjective -= CancelTargetScale;
    }

}
