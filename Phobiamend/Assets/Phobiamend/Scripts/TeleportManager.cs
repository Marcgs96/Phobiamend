using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    public List<TeleportZone> teleports;
    public GameObject[] teleportGameObjects;
    // Start is called before the first frame update
    void Start()
    {
        teleports = new List<TeleportZone>();

        foreach (GameObject item in teleportGameObjects)
        {
            teleports.Add(item.GetComponent<TeleportZone>());
        }

        InitTeleports();
    }

    void InitTeleports()
    {
        foreach (TeleportZone item in teleports)
        {
            item.gameObject.SetActive(true);
            item.SetToInState();
        }

        teleports[0].gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTeleport(int id)
    {
        foreach (TeleportZone item in teleports)
        {
            item.gameObject.SetActive(true);
            item.SetToInState();
        }

        teleports[id].gameObject.SetActive(false);
    }

    public void OnHover()
    {

    }

    public void OnHoverExit()
    {

    }
}
