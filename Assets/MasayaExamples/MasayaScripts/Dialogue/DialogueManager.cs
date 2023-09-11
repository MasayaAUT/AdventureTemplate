using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MasayaScripts
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager current;
        private NPC currentNPC;
        private List<DialogueData> currentDialogue = new List<DialogueData>();
        private int currentIndex;

        [SerializeField] private GameObject dialogueVisuals;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI dialogueText;

        private void Start()
        {
            current = this;
            dialogueVisuals.SetActive(false);
        }

        public void StartDialogue(NPC npc, List<DialogueData> dialogueData)
        {
            dialogueVisuals.SetActive(true);
            currentNPC = npc;
            currentDialogue = dialogueData;
            currentIndex = 0;

            nameText.text = currentDialogue[0].characterName;
            dialogueText.text = currentDialogue[0].dialogueText;
        }

        public void NextDialogue()
        {
            currentIndex += 1;
            if (currentIndex >= currentDialogue.Count)
            {
                FinishDialogue();
            }
            else
            {
                nameText.text = currentDialogue[currentIndex].characterName;
                dialogueText.text = currentDialogue[currentIndex].dialogueText;
            }
        }

        public void FinishDialogue()
        {
            dialogueVisuals.SetActive(false);
            currentNPC.FinishedDialogue();
        }
    }

    [System.Serializable]
    public class DialogueData
    {
        public string characterName;
        [TextArea]
        public string dialogueText;
    }
}



