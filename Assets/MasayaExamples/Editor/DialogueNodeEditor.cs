using UnityEngine;
using UnityEditor;
using MasayaScripts;

[CustomEditor(typeof(DialogueNode))]
public class DialogueNodeEditor : Editor
{

    public SerializedProperty
        dialogueType_prop,
        dialogueName_prop,
        dialogueText_prop,
        choices_prop;

    private void OnEnable()
    {
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

        serializedObject.ApplyModifiedProperties();

    }
}
