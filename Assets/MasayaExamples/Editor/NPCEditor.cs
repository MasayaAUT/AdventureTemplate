using UnityEngine;
using UnityEditor;
using MasayaScripts;
using UnityEditorInternal;

[CustomEditor(typeof(NPC))]
public class NPCEditor : Editor
{
    NPC npc;
    ReorderableList list;
    public SerializedProperty 
        dialogueConditions,
        dialogueController,
        enterEvent,
        exitEvent,
        finishedDialogueEvent
        ;

    private void OnEnable()
    {
        npc = (NPC)target;
        dialogueConditions = serializedObject.FindProperty("dialogueConditions");
        dialogueController = serializedObject.FindProperty("dialogueController");
        enterEvent = serializedObject.FindProperty("enterEvent");
        exitEvent = serializedObject.FindProperty("exitEvent");
        finishedDialogueEvent = serializedObject.FindProperty("finishedDialogueEvent");

        list = new ReorderableList(serializedObject, dialogueConditions, true, true, true, true);

        list.drawElementCallback = DrawListItems;
        list.drawHeaderCallback = DrawHeader;
        list.elementHeightCallback = DrawHeight;
    }

    private void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
        DialogueConditions.DialogueConditionType conditionType = (DialogueConditions.DialogueConditionType)element.FindPropertyRelative("dialogueConditionType").enumValueIndex;
        rect.y += 2;

        GUIStyle headStyle = new GUIStyle();
        headStyle.fontSize = 13;
        headStyle.fontStyle = FontStyle.Bold;
        headStyle.normal.textColor = Color.white;

        EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), "Priority: " + (index + 1), headStyle);
        int extraValue = 20;
        EditorGUI.PropertyField(
            new Rect(rect.x, rect.y + extraValue, 120
            , EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("dialogueConditionType"),
            GUIContent.none
            );

        switch (conditionType)
        {
            case DialogueConditions.DialogueConditionType.QuestComplete:
                QuestValues(rect, element, extraValue);
                break;
            case DialogueConditions.DialogueConditionType.QuestIncomplete:
                QuestValues(rect, element, extraValue);
                break;
            case DialogueConditions.DialogueConditionType.QuestToGive:
                QuestValues(rect, element, extraValue);
                break;
            case DialogueConditions.DialogueConditionType.HasItem:
                ItemValues(rect, element, true, extraValue);
                break;
            case DialogueConditions.DialogueConditionType.NoItem:
                ItemValues(rect, element, false, extraValue);
                break;
        }

        DefaultValue(rect, element, extraValue);
    }

    void QuestValues(Rect rect, SerializedProperty element, int extraValue)
    {
        EditorGUI.LabelField(new Rect(rect.x + 130, rect.y + extraValue, 100, EditorGUIUtility.singleLineHeight), "Quest Name");
        EditorGUI.PropertyField(
                new Rect(rect.x + 210, rect.y + extraValue, 150, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("questName"),
                GUIContent.none
                );
    }

    void ItemValues(Rect rect, SerializedProperty element, bool amount, int extraValue)
    {
        EditorGUI.LabelField(new Rect(rect.x + 130, rect.y + extraValue, 100, EditorGUIUtility.singleLineHeight), "Item");
        EditorGUI.PropertyField(
                new Rect(rect.x + 160, rect.y + extraValue, 200, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("itemRequired"),
                GUIContent.none
                );

        if (amount)
        {
            EditorGUI.LabelField(new Rect(rect.x + 380, rect.y + extraValue, 120, EditorGUIUtility.singleLineHeight), "Amount");
            EditorGUI.PropertyField(
                    new Rect(rect.x + 435, rect.y + extraValue, 50, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("itemAmountRequired"),
                    GUIContent.none
                    );
        }
    }

    void DefaultValue(Rect rect, SerializedProperty element, int extraValue)
    {
        EditorGUI.PropertyField(
            new Rect(rect.x, rect.y + 30 + extraValue, rect.width, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("dialogueController")
            );

        EditorGUI.PropertyField(
        new Rect(rect.x, rect.y + 60 + extraValue, rect.width, EditorGUIUtility.singleLineHeight),
        element.FindPropertyRelative("finishedDialogueEvent"),
        GUIContent.none
        );
    }

    void DrawHeader(Rect rect)
    {
        string name = "Dialogue Conditions";
        EditorGUI.LabelField(rect, name);
    }

    private float DrawHeight(int index)
    {
        float height = 180;
        int eventAmount = npc.dialogueConditions[index].finishedDialogueEvent.GetPersistentEventCount() - 1;
        if(eventAmount < 0)
        {
            eventAmount = 0;
        }
        height += eventAmount * 50;
        return height;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(dialogueController, new GUIContent("Single Dialogue"));
        if (npc.dialogueController != null)
        {
            EditorGUI.LabelField(new Rect(20, 0, 300, 80), "(Remove the Single Dialogue for Condition Options)");

        }
        EditorGUILayout.Space(30);
        if (npc.dialogueController == null)
        {
            list.DoLayoutList();
            EditorGUILayout.Space(30);
        }
        EditorGUILayout.PropertyField(enterEvent);
        EditorGUILayout.PropertyField(exitEvent);
        EditorGUILayout.PropertyField(finishedDialogueEvent);
        serializedObject.ApplyModifiedProperties();
    }
}
