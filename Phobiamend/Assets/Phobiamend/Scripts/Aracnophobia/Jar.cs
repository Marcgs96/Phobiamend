using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jar : MonoBehaviour
{
    BoxCollider collider;
    Rigidbody rigidbody;
    public GameObject jarPosition;

    bool beingGrabbed = false;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider>();
        rigidbody = GetComponent<Rigidbody>();
        jarPosition.SetActive(false);
    }

    public void SetAsGrabbed(bool set)
    {
        beingGrabbed = set;
    }

    public void SetRigidBodyAsKinematic(bool set)
    {
        rigidbody.isKinematic = set;
    }

    public void SetColliderAsTrigger(bool set)
    {
        collider.isTrigger = set;
    }

    public void ActivateJarPosition()
    {
        jarPosition.SetActive(true);
    }

    public void DeActivateJarPosition()
    {
        jarPosition.SetActive(false);
        GameManager.instance.aracnophobiaLevel.aracnophobiaObjectives.CompleteObjective("JarDrop");
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "JarUpAux")
        {
            GameManager.instance.aracnophobiaLevel.aracnophobiaObjectives.CompleteObjective("JarTake");
            GameManager.instance.aracnophobiaLevel.MoveSpiders();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!beingGrabbed)
        {
            SetRigidBodyAsKinematic(true);
            SetColliderAsTrigger(true);
        }
    }

    private void OnTriggerStay(Collider other) { 
        if (!beingGrabbed)
        {
            if (other.gameObject.name == "JarPosition")
            {
                DeActivateJarPosition();
            }
        }
    }
  
}
