using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VisionObjective : MonoBehaviour
{
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
        if (DOTween.TweensById("scaleTarget") != null)
        {
            transform.GetChild(0).GetComponent<TextMesh>().text = DOTween.TweensById("scaleTarget").ToArray()[0].Elapsed().ToString("F1");
        }
    }

    void ScaleTarget(GameObject target)
    {
        if(DOTween.TweensById("scaleTarget") != null)
        {
            DOTween.Play("scaleTarget");
        }
        else {
            target.transform.DOScale(0.08f, 5.0f).SetId("scaleTarget").OnComplete(CompleteVisionTarget);
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
