using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(bl_ChatManager))]
public class bl_ChatManagerEditor : Editor {

    ReorderableList GroupList;

    void OnEnable()
    {
        GroupList = new ReorderableList(serializedObject, serializedObject.FindProperty("Groups"), true, true, true, true);
        GroupList.drawElementCallback = OnDrawElementGroups;
        GroupList.drawHeaderCallback = OnHeaderGroup;
    }

    public override void OnInspectorGUI()
    {
        bl_ChatManager myTarget = (bl_ChatManager)target;

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Groups Settings",EditorStyles.boldLabel);
        myTarget.GroupID = EditorGUILayout.Popup("My Group:",myTarget.GroupID, myTarget.GetGroupArray);
        myTarget.ShowGroupsEditor = EditorGUILayout.ToggleLeft("Show Groups", myTarget.ShowGroupsEditor, EditorStyles.toolbarButton);
        if (myTarget.ShowGroupsEditor)
        {
            serializedObject.Update();
            GroupList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Chat Settings", EditorStyles.boldLabel);
        myTarget.useBothSides = EditorGUILayout.ToggleLeft("Use Both Sides",myTarget.useBothSides, EditorStyles.toolbarButton);
        myTarget.useGroupPrefix = EditorGUILayout.ToggleLeft("Use Group Prefix", myTarget.useGroupPrefix, EditorStyles.toolbarButton);
        myTarget.ShowPlayerNameInput = EditorGUILayout.ToggleLeft("Show Player Name Input", myTarget.ShowPlayerNameInput, EditorStyles.toolbarButton);
        myTarget.MaxMessages = EditorGUILayout.IntSlider("Max Messages",myTarget.MaxMessages, 0, 50);
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Messages Settings", EditorStyles.boldLabel);
        myTarget.FadeMessage = EditorGUILayout.ToggleLeft("Fade Messages", myTarget.FadeMessage, EditorStyles.toolbarButton);
        if (myTarget.FadeMessage)
        {
            myTarget.FadeMessageIn = EditorGUILayout.Slider("Fade In:", myTarget.FadeMessageIn, 3, 60);
            myTarget.FadeMessageSpeed = EditorGUILayout.Slider("Fade Speed:", myTarget.FadeMessageSpeed, 0.5f, 8);
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Bad Word List", EditorStyles.boldLabel);
        myTarget.ShowBlackListEditor = EditorGUILayout.ToggleLeft("Show Black Words List", myTarget.ShowBlackListEditor, EditorStyles.toolbarButton);
        if (myTarget.ShowBlackListEditor)
        {
            myTarget.BlackList = EditorGUILayout.TextArea(myTarget.BlackList,GUILayout.Height(70));
            EditorGUILayout.HelpBox("Separate words by ',' all words should be in lowercase.", MessageType.Info);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Replace Char:",GUILayout.Width(100));
            myTarget.ReplaceString = EditorGUILayout.TextField(myTarget.ReplaceString, GUILayout.Width(30));
            GUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }

    void OnDrawElementGroups(Rect rect,int index,bool isActive,bool isFocus)
    {
        SerializedProperty element = GroupList.serializedProperty.GetArrayElementAtIndex(index);
        SerializedProperty thename = element.FindPropertyRelative("Name");
        SerializedProperty thecolor = element.FindPropertyRelative("GroupColor");
        rect.y += 2;
        thename.stringValue = EditorGUI.TextField(new Rect(rect.x,rect.y, 200, EditorGUIUtility.singleLineHeight), thename.stringValue);
        thecolor.colorValue = EditorGUI.ColorField(new Rect(rect.x + 210, rect.y, 100, EditorGUIUtility.singleLineHeight), thecolor.colorValue);
    }

    void OnHeaderGroup(Rect rect)
    {
        EditorGUI.LabelField(rect,"Groups");
    }
}