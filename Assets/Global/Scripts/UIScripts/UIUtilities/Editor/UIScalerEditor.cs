using UnityEditor;

[CustomEditor(typeof(UIScaler))]
public class UIScalerEditor : Editor
{
    private static readonly string[] hideFromInspector = new string[] {"m_Script"};

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, hideFromInspector);

        serializedObject.ApplyModifiedProperties();
    }
}
