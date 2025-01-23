using UnityEditor;

[CustomEditor(typeof(UIPanelScaler))]
public class UIPanelScalerEditor : Editor
{
    private static readonly string[] hideFromInspector = new string[] {"m_Script"};

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, hideFromInspector);

        serializedObject.ApplyModifiedProperties();
    }
}
