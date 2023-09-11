using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MasayaScripts
{
    public class DialogueNode : StateMachineBehaviour
    {
        public DialogueType dialogueType;

        //Text
        public string dialogueName;
        [TextArea]
        public string dialogueText;

        //MultiChoice
        public List<string> choices = new List<string>();

        public enum DialogueType
        {
            Text,
            MultiChoice,
            End
        }

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetInteger("DialogueOption", 0);
            DialogueManagerV2.current.NextDialogueState(this);
        }
    }
}
