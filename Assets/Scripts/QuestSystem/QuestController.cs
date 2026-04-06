using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    Quest quest;
    [SerializeField] private GameObject questName;
    [SerializeField] private GameObject questDescription;
    [SerializeField] private GameObject completed;
    public void RemoveItem()
    {
        QuestManager.instance.Remove(quest);
        Destroy(gameObject);
    }
    public void AddQuest(Quest newQuest)
    {
        quest = newQuest;
    }

    private void Update()
    {
        bool completedQuest = ((Ink.Runtime.BoolValue) DialogueManager.GetInstance().GetVariableState(quest.completedName)).value;
        bool rewardQuest = ((Ink.Runtime.BoolValue) DialogueManager.GetInstance().GetVariableState(quest.hasReciveReward)).value;

        if(rewardQuest)
        {
            completed.SetActive(true);
            questName.SetActive(false);
            questDescription.SetActive(false);
        }
    }
    public void UseQuest()
    {
        switch(quest.questType)
        {
            case Quest.QuestType.Destination:
                //PlayerLife.instance.IncreaseHealth(item.value);
                //FindObjectOfType<AudioManager>().Play("Health");
                break;
            case Quest.QuestType.Item:
                Debug.Log("HAI USATO UNA CHIAVE");
                break;
            case Quest.QuestType.Kill:
                Debug.Log("HAI USATO UCCISO");
                break;
        }
        RemoveItem();
    }
}