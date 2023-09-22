using UnityEngine;
using UnityEditor;
using MasayaScripts;

[CustomEditor(typeof(DialogueNode))]
public class DialogueNodeEditor : Editor
{

    DialogueNode node;

    public SerializedProperty
        dialogueType_prop,
        dialogueName_prop,
        dialogueText_prop,
        choices_prop;

    private void OnEnable()
    {
        node = (DialogueNode)target;

        dialogueType_prop = serializedObject.FindProperty("dialogueType");
        dialogueName_prop = serializedObject.FindProperty("dialogueName");
        dialogueText_prop = serializedObject.FindProperty("dialogueText");
        choices_prop = serializedObject.FindProperty("choices");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(dialogueType_prop);

        DialogueNode.DialogueType dt = (DialogueNode.DialogueType)dialogueType_prop.enumValueIndex;
        switch (dt)
        {
            case DialogueNode.DialogueType.Text:
                EditorGUILayout.PropertyField(dialogueName_prop, new GUIContent("CharacterName"));
                EditorGUILayout.PropertyField(dialogueText_prop, new GUIContent("CharacterDialogue"));
                break;
            case DialogueNode.DialogueType.MultiChoice:
                EditorGUILayout.PropertyField(choices_prop, new GUIContent("Choices"));
                break;
            case DialogueNode.DialogueType.End:
                break;
        }

        if (GUILayout.Button("Set Parameters"))
        {
            UpdateParameters();
        }

        serializedObject.ApplyModifiedProperties();
    }

    public void UpdateParameters()
    {
        UnityEditor.Animations.AnimatorState ac = Selection.activeObject as UnityEditor.Animations.AnimatorState;
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

        DialogueNode.DialogueType dt = (DialogueNode.DialogueType)dialogueType_prop.enumValueIndex;

        switch (dt)
        {
            case DialogueNode.DialogueType.Text:
                Selection.activeObject.name = node.dialogueText;
                for (int i = 0; i < ac.transitions.Length; i++)
                {
                    ac.transitions[i].hasExitTime = false;
                    ac.transitions[i].AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, 0, "NextDialogue");
                }
                break;
            case DialogueNode.DialogueType.MultiChoice:
                Selection.activeObject.name = "Choices";
                for (int i = 0; i < ac.transitions.Length; i++)
                {
                    ac.transitions[i].hasExitTime = false;
                    ac.transitions[i].AddCondition(UnityEditor.Animations.AnimatorConditionMode.Equals, i + 1, "DialogueOption");
                }
                break;
            case DialogueNode.DialogueType.End:
                Selection.activeObject.name = "End";
                break;
        }
    }
}
