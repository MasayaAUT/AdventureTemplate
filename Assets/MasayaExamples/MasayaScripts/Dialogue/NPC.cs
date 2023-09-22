using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MasayaScripts.InventorySystem;
using MasayaScripts.Quest;

namespace MasayaScripts
{
    [System.Serializable]
    public class NPC : MonoBehaviour
    {
        public RuntimeAnimatorController dialogueController; //Single Dialogue
        //Multiple Dialogues
        public List<DialogueConditions> dialogueConditions = new List<DialogueConditions>();
        public UnityEvent enterEvent;
        public UnityEvent exitEvent;
        public UnityEvent finishedDialogueEvent;
        bool usingCondition;
        int conditionIndex;
        bool playerFound;
        bool isTalking;
        int itemAmount;

        private void Update()
        {
            if (playerFound == true & Input.GetKeyDown(KeyCode.E))
            {
                if (isTalking == true)
                {
                    DialogueManagerV2.current.NextDialogue();
                    return;
                }

                if (dialogueController == null)
                {
                    foreach (DialogueConditions checkDialogueCondition in dialogueConditions)
                    {
                        bool conditionMet = CheckCondition(checkDialogueCondition);
                        if (conditionMet)
                        {
                            if (checkDialogueCondition.dialogueController != null)
                            {
                                isTalking = true;
                                PlayerMovement.current.SetMovement(false);
                                exitEvent.Invoke();
                                usingCondition = true;
                                conditionIndex = dialogueConditions.IndexOf(checkDialogueCondition);
                                DialogueManagerV2.current.StartDialogue(this, checkDialogueCondition.dialogueController);
                                return;
                            }
                            else
                            {
                                checkDialogueCondition.finishedDialogueEvent.Invoke();
                                return;
                            }
                        }
                    }

                    exitEvent.Invoke();
                    FinishedDialogue();
                    return;
                }


                isTalking = true;
                PlayerMovement.current.SetMovement(false);
                exitEvent.Invoke();
                DialogueManagerV2.current.StartDialogue(this, dialogueController);
            }
        }

        bool CheckCondition(DialogueConditions checkDialogueCondition)
        {
            switch (checkDialogueCondition.dialogueConditionType)
            {
                case DialogueConditions.DialogueConditionType.QuestComplete:
                    if (QuestManager.current.CheckQuestStatus(checkDialogueCondition.questName))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case DialogueConditions.DialogueConditionType.QuestIncomplete:
                    if (!QuestManager.current.PlayerHasQuest(checkDialogueCondition.questName))
                    {
                        return false;
                    }

                    if (QuestManager.current.CheckQuestStatus(checkDialogueCondition.questName))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case DialogueConditions.DialogueConditionType.QuestToGive:
                    if (QuestManager.current.PlayerHasQuest(checkDialogueCondition.questName))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case DialogueConditions.DialogueConditionType.HasItem:
                    if (checkDialogueCondition.itemAmountRequired > 0)
                    {
                        if (InventoryManager.current.CheckItemForQuantity(checkDialogueCondition.itemRequired, checkDialogueCondition.itemAmountRequired))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (InventoryManager.current.CheckForItemInInventory(checkDialogueCondition.itemRequired))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                case DialogueConditions.DialogueConditionType.NoItem:
                    if (InventoryManager.current.CheckForItemInInventory(checkDialogueCondition.itemRequired))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case DialogueConditions.DialogueConditionType.Default:
                    return true;
            }
            return false;
        }

        public void FinishedDialogue()
        {
            PlayerMovement.current.SetMovement(true);
            isTalking = false;
            finishedDialogueEvent.Invoke();
            if (usingCondition)
            {
                dialogueConditions[conditionIndex].finishedDialogueEvent.Invoke();
            }
        }
        public void AddQuest(string questName)
        {
            QuestManager.current.AddQuest(questName);
        }
        public void CompleteQuest(string questName)
        {
            QuestManager.current.HasCompletedQuest(questName);
        }
        public void AddItem(Item item)
        {
            int amount = itemAmount;
            InventoryManager.current.AddItemToInventory(item, amount);

            itemAmount = 1;
        }
        public void RemoveItem(Item item)
        {
            int amount = itemAmount;
            InventoryManager.current.RemoveItemFromInventory(item, amount);

            itemAmount = 1;
        }
        public void SetItemAmount(int amount)
        {
            itemAmount = amount;
        }
        public void OnTriggerEnter(Collider other)
        {
            enterEvent.Invoke();
            playerFound = true;
        }

        public void OnTriggerExit(Collider other)
        {
            exitEvent.Invoke();
            playerFound = false;
            isTalking = false;
        }
    }
}

[System.Serializable]
public class DialogueConditions
{
    public DialogueConditionType dialogueConditionType;
    public RuntimeAnimatorController dialogueController;
    public string questName;
    public Item itemRequired;
    public int itemAmountRequired;
    public UnityEvent finishedDialogueEvent;

    public enum DialogueConditionType
    {
        QuestComplete,
        QuestIncomplete,
        QuestToGive,
        HasItem,
        NoItem,
        Default
    }
}
