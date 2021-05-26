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
    public GameObject scoreboardPanel;
    public GameObject buttonsPanel;

    private void Start()
    {
        objectivePanel = GameObject.Find("ObjectivesPanel");
        scoreboardPanel = GameObject.Find("ScoreBoardPanel");
        scoreboardPanel.SetActive(false);
        buttonsPanel = GameObject.Find("ButtonsPanel");
        buttonsPanel.SetActive(false);

        for (int i = 0; i < objectives.Length; ++i)
        {
            GameObject newObjectiveUI = Instantiate(objectiveUI, objectivePanel.transform);
            newObjectiveUI.GetComponent<Text>().text = (objectives[i].id + 1).ToString() + ". " + objectives[i].description;
            newObjectiveUI.transform.GetChild(0).gameObject.SetActive(false);
            objectives[i].completed = false;
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
        if (!FindObjectiveByName(name).completed)
        {
            FindObjectiveByName(name).completed = true;
            GameManager.instance.delegateHandler.OnObjectiveComplete(FindObjectiveByName(name).id);
        }

        if (CheckAllObjectiesCompleted())
        {
            ShowScoreboard();
            GameManager.instance.delegateHandler.OnAllObjectivesComplete();
        }
    }

    void ShowScoreboard()
    {
        objectivePanel.SetActive(false);
        scoreboardPanel.SetActive(true);
        buttonsPanel.SetActive(true);
    }

    public void SetScores(AracnophobiaScore scores)
    {
        scoreboardPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = scores.objectivesScore.ToString();
        scoreboardPanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = scores.timeScore.ToString("F1");
        scoreboardPanel.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = scores.dificultySMultiplier.ToString("F1");
        scoreboardPanel.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = scores.totalScore.ToString("F1");
    }

    bool CheckAllObjectiesCompleted()
    {
        foreach (var item in objectives)
        {
            if (!item.completed)
                return false;
        }

        return true;
    }
}
