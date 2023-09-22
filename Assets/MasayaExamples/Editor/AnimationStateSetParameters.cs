using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MasayaScripts;
using UnityEditor.Animations;

public class AnimationStateSetParameters : EditorWindow
{
    string animatorName = "No Animator Selected";
    AnimatorController currentAnim;

    [MenuItem("MasayaTools/DialogueSystem")]
    public static void ShowWindow()
    {
        GetWindow<AnimationStateSetParameters>(false, "DialogueSystem", true);
    }

    private void Update()
    {
        for (int i = 0; i < Selection.objects.Length; i++)
        {
            AnimatorController controller = Selection.objects[i] as AnimatorController;
            if (controller != null)
            {
                if (controller != currentAnim)
                {
                    animatorName = "Current Animator = " + controller.name;
                    currentAnim = controller;
                    Repaint();
                }
            }
            else
            {
                if (animatorName != "No Animator Selected" && currentAnim == null)
                {
                    animatorName = "No Animator Selected";
                    Repaint();
                }
            }
        }
    }

    private void OnGUI()
    {

        EditorGUILayout.LabelField(animatorName);
        if (GUILayout.Button("Convert To Dialogue"))
        {
            ConvertToDialogue(currentAnim);
        }
        if(GUILayout.Button("Create Text Node"))
        {
            AddDialogueNode(currentAnim, DialogueNode.DialogueType.Text);
        }
        if (GUILayout.Button("Create Multi Choice Node"))
        {
            AddDialogueNode(currentAnim, DialogueNode.DialogueType.MultiChoice);
        }
        if (GUILayout.Button("Create End Node"))
        {
            AddDialogueNode(currentAnim, DialogueNode.DialogueType.End);
        }
        if(GUILayout.Button("Set Selected Node Parameters"))
        {
            SetParameters();
        }
    }

    public static void ConvertToDialogue(AnimatorController currentAnim)
    {
        if (currentAnim != null)
        {
            currentAnim.parameters = null;
            currentAnim.AddParameter("NextDialogue", AnimatorControllerParameterType.Trigger);
            currentAnim.AddParameter("DialogueOption", AnimatorControllerParameterType.Int);
            AnimatorState state = currentAnim.layers[0].stateMachine.AddState("Dialogue");
        }
    }
    public static void AddDialogueNode(AnimatorController controller, DialogueNode.DialogueType dt)
    {
        if (controller != null)
        {
            AnimatorState state = controller.layers[0].stateMachine.AddState("Dialogue");
            DialogueNode node = state.AddStateMachineBehaviour<DialogueNode>();
            node.dialogueType = dt;
            switch (dt)
            {
                case DialogueNode.DialogueType.MultiChoice:
                    state.name = "{Choices}";
                    break;
                case DialogueNode.DialogueType.End:
                    state.name = "{End}";
                    break;
            }
        }
    }

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
                        string nodeName = node.dialogueText;
                        if (nodeName.Length > 20)
                        {
                            nodeName = nodeName.Substring(0, 20);
                            nodeName += "_";
                        }

                        nodeName = nodeName.Replace(".", "_");
                        Selection.objects[x].name = nodeName;
                        for (int i = 0; i < ac.transitions.Length; i++)
                        {
                            ac.transitions[i].hasExitTime = false;
                            ac.transitions[i].AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, 0, "NextDialogue");
                        }
                        break;
                    case DialogueNode.DialogueType.MultiChoice:
                        Selection.objects[x].name = "{Choices}";
                        for (int i = 0; i < ac.transitions.Length; i++)
                        {
                            ac.transitions[i].hasExitTime = false;
                            ac.transitions[i].AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, i + 1, "DialogueOption");
                        }
                        break;
                    case DialogueNode.DialogueType.End:
                        Selection.objects[x].name = "{End}";
                        break;
                }
            }
        }
    }
}
