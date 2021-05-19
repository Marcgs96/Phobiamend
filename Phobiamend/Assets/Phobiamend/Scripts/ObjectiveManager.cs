using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour
{
    public Objective[] objectives;
    public int numberOfObjectives;
    int currentObjective = 0;
    public GameObject objectivePanel;
    public GameObject objectiveUI;

    private void Start()
    {
        for (int i = 0; i < objectives.Length; ++i)
        {
            GameObject newObjectiveUI = Instantiate(objectiveUI, objectivePanel.transform);
            newObjectiveUI.GetComponent<Text>().text = (objectives[i].id + 1).ToString() + ". " + objectives[i].description;
            newObjectiveUI.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        DelegateHandler.objective += OnObjectiveCompleted;
    }

    void OnObjectiveCompleted(int id)
    {
        currentObjective++;
        objectivePanel.transform.GetChild(id).GetChild(0).gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        DelegateHandler.objective -= OnObjectiveCompleted;
    }

    public Objective FindObjectiveByName(string name)
    {
        foreach (var item in objectives)
        {
            if (item.name == name)
                return item;
        }

        return null;
    }

    public void CompleteObjective(string name)
    {
        GameManager.instance.delegateHandler.OnObjectiveComplete(FindObjectiveByName(name).id);
    }
}
