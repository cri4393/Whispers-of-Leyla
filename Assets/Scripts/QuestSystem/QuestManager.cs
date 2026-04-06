using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour, IDataPersistence
{
    public static QuestManager instance;
    public List<Quest> quests = new List<Quest>();
    public Transform questContent;
    public GameObject questPrefab;
    public QuestController[] questController;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Add(Quest quest)
    {
        quests.Add(quest);
    }

    public void Remove(Quest quest)
    {
        quests.Remove(quest);
    }

    public void ListQuests()
    {
        foreach (var quest in quests)
        {
            GameObject obj = Instantiate(questPrefab, questContent);
            var questName = obj.transform.Find("QuestName").GetComponent<TextMeshProUGUI>();
            var questDescription = obj.transform.Find("QuestDescription").GetComponent<TextMeshProUGUI>();

            questName.text = quest.questName;
            questDescription.text = quest.questDescription;
        }
        SetQuests();
    }

    public void SetQuests()
    {
        questController = questContent.GetComponentsInChildren<QuestController>();

        for (int i = 0; i < quests.Count; i++)
        {
            questController[i].AddQuest(quests[i]);
        }
    }

    public void CleanQuest()
    {
        //Clean content before open.
        foreach (Transform quest in questContent)
        {
            Destroy(quest.gameObject);
        }
    }

    public void LoadData(GameData data)
    {
        foreach (var quest in data.questGained)
        {
            Add(quest);
        }
    }

    public void SaveData(GameData data)
    {
        // no data needs to be saved for this script
        foreach (var quest in quests)
        {
            data.questGained.Add(quest);
        }
    }
}
