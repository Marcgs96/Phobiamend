using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Phobiamend/ScriptableObjects/ObjectiveScriptableObject", order = 1)]
public class Objective : ScriptableObject
{
    public int id = 0;
    public string name;
    public string description;
    public bool completed = false;
}
