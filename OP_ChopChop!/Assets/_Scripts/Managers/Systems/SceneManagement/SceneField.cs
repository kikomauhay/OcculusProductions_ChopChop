using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class SceneField {

    public string SceneName => _sceneName;

    [SerializeField] Object _sceneAsset;
    [SerializeField] string _sceneName = "";

    // makes it work with the existing Unity methods (LoadLevel/LoadScene)
    public static implicit operator string(SceneField sceneField) =>
        sceneField.SceneName;
}

#if UNITY_EDITOR

public class SceneFieldPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, GUIContent.none, property);

        SerializedProperty sceneAsset = property.FindPropertyRelative("_sceneAsset");
        SerializedProperty sceneName = property.FindPropertyRelative("_sceneName");

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        if (sceneAsset != null)
        {
            sceneAsset.objectReferenceValue = EditorGUI.ObjectField(position, 
                                                                    sceneAsset.objectReferenceValue, 
                                                                    typeof(SceneAsset), false);

            if (sceneAsset.objectReferenceValue != null)
                sceneName.stringValue = (sceneAsset.objectReferenceValue as SceneAsset).name;           
        }
            
        EditorGUI.EndProperty();
    }
}

#endif