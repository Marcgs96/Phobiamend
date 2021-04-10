using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportZone : MonoBehaviour
{
    public int id;
    private MeshRenderer renderer;
    private Collider collider;

    public Material[] materials;

    public enum TELEPORT_STATE { NONE, OUT, IN, HOVER};
    public TELEPORT_STATE state = TELEPORT_STATE.NONE;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetToHoverState()
    {
        state = TELEPORT_STATE.HOVER;
        renderer.material = materials[(int)TELEPORT_STATE.HOVER];
    }

    public void SetToOutState()
    {
        state = TELEPORT_STATE.OUT;
        renderer.material = materials[(int)TELEPORT_STATE.OUT];
        collider.enabled = false;
    }
    public void SetToInState()
    {
        state = TELEPORT_STATE.IN;
        renderer.material = materials[(int)TELEPORT_STATE.IN];
        collider.enabled = true;
    }

    public void SetToNoneState()
    {
        state = TELEPORT_STATE.NONE;
        renderer.material = materials[(int)TELEPORT_STATE.OUT];
        renderer.enabled = false;
        collider.enabled = false;
    }
}
