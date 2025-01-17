﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Custom Scriptable Objects/Quests/Quest")]
public class Quest : ScriptableObject
{
    //IMPLEMENT
    //public QuestNoticeBoardUILoader loader;


    public string QuestName;
    public int experienceForCompletion;

    //The first node in the quest
    public QuestNodeBase initalNode;

    //The final node in the quest. Once this is done, the quest is completed.
    public QuestNodeBase finalNode;

    [Space]
    public List<QuestNodeBase> QuestNodes;

    public LevelData playerLevelData;

    public bool Started;
    public bool Completed;
    public bool Failed;
    [Space]
    [Space]
    public bool ResetButton;

    public void StartQuest()
    {
        foreach(QuestNodeBase node in QuestNodes)
        {
            node.ResetNode();
            node.parentQuest = this;
            node.NodeID = this.QuestName + "/" + node.nodeTitle;
        }

        //DisplayQuestNotification() that quest was started

        if(finalNode.OnNodeComplete == null)
        {
            finalNode.OnNodeComplete = new UnityEngine.Events.UnityEvent();
        }
        finalNode.OnNodeComplete.AddListener(CompleteQuest);
        initalNode.MarkAsAvailable();

        Started = true;
    }

    //Must be reworked to be properly implemented
    public void DisplayQuestNotification(string notificationString)
    {
        
    }


    public void CompleteQuest()
    {
        playerLevelData.GainExperience(experienceForCompletion);
        //DisplayQuestNotification("Quest Complete!");
        Completed = true;
    }

    
    public Quest CreateRuntimeCopy()
    {
        Quest copy = ScriptableObject.CreateInstance<Quest>();

        copy.QuestName = QuestName;
        copy.experienceForCompletion = experienceForCompletion;
        

        copy.playerLevelData = playerLevelData;

        List <QuestNodeBase> nodes = new List<QuestNodeBase>();
        foreach(QuestNodeBase node in QuestNodes)
        {
            nodes.Add(node.CreateRuntimeCopy());
        }

        copy.QuestNodes = nodes;

        return copy;
    }

    public void Reset()
    {
        foreach(QuestNodeBase node in QuestNodes)
        {
            node.available = false;
            node.completed = false;
            node.failed = false;
        }

        Failed = false;
        Completed = false;
        Started = false;
    }

    public void OnValidate()
    {
        if(ResetButton)
        {
            ResetButton = false;
        }
    }
}
