using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Phobiamend/ScriptableObjects/RingScriptableObject", order = 1)]
public class RingScriptableObject : ScriptableObject
{
    public string prefabName;

    public float size;
    public float speed;
    public Ring.MOVEMENT_TYPE movementType;
}