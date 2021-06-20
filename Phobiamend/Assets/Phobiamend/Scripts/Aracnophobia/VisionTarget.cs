using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionTarget : MonoBehaviour
{
    public VisionObjective currentTargetedObjective = null;
    // See Order of Execution for Event Functions for information on FixedUpdate() and Update() related to physics queries
    void FixedUpdate()
    {
        // Bit shift the index of the layer (9) to get a bit mask
        int layerMask = 1 << 10;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            currentTargetedObjective = hit.collider.gameObject.GetComponent<VisionObjective>();
            GameManager.instance.delegateHandler.OnTargetObjective(currentTargetedObjective.gameObject);
        }
        else
        {
            if(currentTargetedObjective != null)
            {
                currentTargetedObjective = null;
                GameManager.instance.delegateHandler.OnDeTargetObjective();
            }
        }
    }
}
