using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MasayaScripts;

public static class AnimationStateSetParameters
{
    [MenuItem("Tools/Set Animation State Parameters #&P", false, 901)]
    public static void SetParameters()
    {
        for (int x = 0; x < Selection.objects.Length; x++)
        {
            UnityEditor.Animations.AnimatorState ac = Selection.objects[x] as UnityEditor.Animations.AnimatorState;
            if (ac != null)
            {
                DialogueNode node = ac.behaviours[0] as DialogueNode;
                DialogueNode.DialogueType dt = node.dialogueType;

                if (ac.transitions.Length > 0)
                {
                    for (int i = 0; i < ac.transitions.Length; i++)
                    {
                        if (ac.transitions[i].conditions.Length > 0)
                        {
                            ac.transitions[i].conditions = null;
                        }
                    }
                }

                switch (dt)
                {
                    case DialogueNode.DialogueType.Text:
                        Selection.objects[x].name = node.dialogueText;
                        for (int i = 0; i < ac.transitions.Length; i++)
                        {
                            ac.transitions[i].hasExitTime = false;
                            ac.transitions[i].AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, 0, "NextDialogue");
                        }
                        break;
                    case DialogueNode.DialogueType.MultiChoice:
                        Selection.objects[x].name = "Choices";
                        for (int i = 0; i < ac.transitions.Length; i++)
                        {
                            ac.transitions[i].hasExitTime = false;
                            ac.transitions[i].AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, i + 1, "DialogueOption");
                        }
                        break;
                    case DialogueNode.DialogueType.End:
                        Selection.objects[x].name = "End";
                        break;
                }
            }
        }
    }

    [MenuItem("Tools/Set Animation State Parameters", true)]
    public static bool SetParametersValidation()
    {
        return Selection.objects.Length > 0;
    }
}
