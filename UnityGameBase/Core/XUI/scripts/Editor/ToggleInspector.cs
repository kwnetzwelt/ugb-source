using System;
using UnityEditor;
using UnityGameBase.Core.XUI;

[CustomEditor(typeof(Toggle))]
/// <summary>
/// This inspector is needed to expose additional button functionality - without CustomEditor,
/// fields are not visible in Inspector.
/// </summary>
public class ToggleInspector : Editor
{
}