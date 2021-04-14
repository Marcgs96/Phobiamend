using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    public Dictionary<int, TeleportZone> teleports;
    public GameObject[] teleportGameObjects;
    public int currentTeleportZone = 0;

    // Start is called before the first frame update
    void Start()
    {
        teleports = new Dictionary<int, TeleportZone>();
        teleportGameObjects = GameObject.FindGameObjectsWithTag("Teleport");

        foreach (GameObject item in teleportGameObjects)
        {
            teleports.Add(item.GetComponent<TeleportZone>().id, item.GetComponent<TeleportZone>());
        }

        InitTeleports();
    }

    void InitTeleports()
    {
        foreach (KeyValuePair<int, TeleportZone> item in teleports)
        {
            item.Value.SetToOutState();
        }

        currentTeleportZone = 0;
        teleports[currentTeleportZone].SetToInState();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTeleport()
    {
        if (teleports[currentTeleportZone].state == TeleportZone.TELEPORT_STATE.HOVER)
        {
            teleports[currentTeleportZone].SetToNoneState();
            if (currentTeleportZone < teleports.Count - 1)
            {
                currentTeleportZone++;
                teleports[currentTeleportZone].SetToInState();
            }
        }
    }

    public void OnHover()
    {
        if (teleports[currentTeleportZone].state == TeleportZone.TELEPORT_STATE.IN)
            teleports[currentTeleportZone].SetToHoverState();
    }

    public void OnHoverExit()
    {
        if (teleports[currentTeleportZone].state == TeleportZone.TELEPORT_STATE.HOVER)
            teleports[currentTeleportZone].SetToInState();
    }
}
