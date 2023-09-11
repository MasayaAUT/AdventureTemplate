using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MasayaScripts
{
    public class DialogueManagerV2 : MonoBehaviour
    {
        public static DialogueManagerV2 current;
        private NPC currentNPC;
        private DialogueNode currentDialogue;
        private Animator anim;

        [SerializeField] private GameObject dialogueVisuals;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI dialogueText;

        [SerializeField] private GameObject multiChoiceVisuals;
        [SerializeField] private List<GameObject> options = new List<GameObject>();

        bool waitingForChoice;

        private void Start()
        {
            current = this;
            dialogueVisuals.SetActive(false);
            anim = GetComponent<Animator>();
        }

        public void StartDialogue(NPC npc, RuntimeAnimatorController dialogueData)
        {
            currentNPC = npc;
            anim.runtimeAnimatorController = dialogueData;
            anim.SetInteger("DialogueOption", 0);

            anim.enabled = true;
        }
        public void NextDialogue()
        {
            if (waitingForChoice)
                return;

            anim.SetTrigger("NextDialogue");
        }
        public void NextDialogueState(DialogueNode node)
        {
            currentDialogue = node;
            DialogueNode.DialogueType dialogueType = node.dialogueType;

            switch (dialogueType)
            {
                case DialogueNode.DialogueType.Text:
                    ShowDialogue();
                    break;
                case DialogueNode.DialogueType.MultiChoice:
                    ShowMultiChoice();
                    break;
                case DialogueNode.DialogueType.End:
                    End();
                    break;
            }
        }

        public void ShowDialogue()
        {
            waitingForChoice = false;

            multiChoiceVisuals.SetActive(false);
            dialogueVisuals.SetActive(true);

            nameText.text = currentDialogue.dialogueName;
            dialogueText.text = currentDialogue.dialogueText;
        }

        public void ShowMultiChoice()
        {
            waitingForChoice = true;

            dialogueVisuals.SetActive(false);
            multiChoiceVisuals.SetActive(true);

            foreach (GameObject buttons in options)
            {
                buttons.SetActive(false);
            }

            int index = 0;
            foreach (string option in currentDialogue.choices)
            {
                options[index].SetActive(true);
                options[index].GetComponentInChildren<TextMeshProUGUI>().text = currentDialogue.choices[index];
                index++;
            }
        }

        public void ChoiceSelected(int value)
        {
            anim.SetInteger("DialogueOption", value);
        }

        public void End()
        {
            waitingForChoice = false;
            anim.enabled = false;

            multiChoiceVisuals.SetActive(false);
            dialogueVisuals.SetActive(false);

            anim.runtimeAnimatorController = null;
            currentNPC.FinishedDialogue();
        }
    }
}
