using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class InkExternalFunctions 
{
    public void Bind(Story story, Animator emoteAnimator, Quest quest)
    {
        story.BindExternalFunction("playEmote", (string emoteName) =>
        {
            PlayEmote(emoteName, emoteAnimator);
        });

        story.BindExternalFunction("StartQuest", () => 
        {
            if(quest != null)
            {
                QuestManager.instance.Add(quest);
            }
            else
            {
                Debug.Log("Quest Scriptable Object was not initialize!");
            }
        });
    }

    public void Unbind(Story story)
    {
        //story.UnbindExternalFunction("playEmote");
        //story.UnbindExternalFunction("StartQuest");
    }

    public void PlayEmote(string emoteName, Animator emoteAnimator)
    {
        if(emoteAnimator != null)
            {
                emoteAnimator.Play(emoteName);
            }
            else
            {
                Debug.Log("animator was not initialize!");
            }
    }

    public void SeeQuest(Quest quest)
    {
        if(quest != null)
        {
            QuestManager.instance.Add(quest);
        }
        else
        {
            Debug.Log("Quest Scriptable Object was not initialize!");
        }
    }
}