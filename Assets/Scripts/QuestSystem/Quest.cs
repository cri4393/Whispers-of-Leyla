using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "ScriptableObject/Create New Quest")]
public class Quest : ScriptableObject
{
    [Header("Quest Data")]
    public string questName;
    public string questDescription;
    public  QuestType questType;

    [Header("Ink Quest Variable")]
    public string completedName;
    public string hasReciveReward;
    
    public enum QuestType
    {
        Destination,
        Item,
        Kill,
    }
}
