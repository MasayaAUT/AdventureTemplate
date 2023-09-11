using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MasayaScripts.Quest
{
    public class QuestManager : MonoBehaviour
    {

        [System.Serializable]
        public class QuestData
        {
            public string questName;
            public bool isCompleted = false;
        }

        public static QuestManager current;
        public List<QuestData> questList = new List<QuestData>();

        private void Awake()
        {
            if (current == null)
            {
                current = this;
            }
        }

        public void AddQuest(string questName)
        {
            foreach (QuestData quest in questList)
            {
                if (quest.questName == questName)
                {
                    return;
                }
            }

            QuestData questData = new QuestData();
            questData.questName = questName;

            questList.Add(questData);
        }

        public void HasCompletedQuest(string questName)
        {
            foreach (QuestData quest in questList)
            {
                if (quest.questName == questName)
                {
                    quest.isCompleted = true;
                }
            }
        }
        public bool PlayerHasQuest(string questName)
        {
            foreach (QuestData quest in questList)
            {
                if (quest.questName == questName)
                {
                    return true;
                }
            }
            return false;
        }
        public bool CheckQuestStatus(string questName)
        {
            foreach (QuestData quest in questList)
            {
                if (quest.questName == questName)
                {
                    return quest.isCompleted;
                }
            }

            return false;
        }
    }
}
