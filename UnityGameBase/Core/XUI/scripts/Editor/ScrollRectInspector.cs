using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(ScrollRect))]
public class ScrollRectInspector : Editor
{
    UnityEngine.UI.ScrollRect myTarget;

    void OnEnable()
    {
        myTarget = target as ScrollRect;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
