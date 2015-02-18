using UnityEngine;
using UnityEditor;
using System.Collections;

public class CameraControlEditor
{
    [MenuItem("UGB/CameraControl/Top #8")]
    public static void Top()
    {    
        SceneView scene = SceneView.currentDrawingSceneView;
        scene.LookAt(scene.pivot, Quaternion.Euler(90, 0, 0));
    }
    
    [MenuItem("UGB/CameraControl/Bottom #2")]
    public static void Bottom()
    {    
        SceneView scene = SceneView.currentDrawingSceneView;
        scene.LookAt(scene.pivot, Quaternion.Euler(-90, 0, 0));
    }
    
    [MenuItem("UGB/CameraControl/Back #3")]
    public static void Back()
    {    
        SceneView scene = SceneView.currentDrawingSceneView;
        scene.LookAt(scene.pivot, Quaternion.Euler(0, 360, 0));
    }
    
    [MenuItem("UGB/CameraControl/Front #1")]
    public static void Front()
    {    
        SceneView scene = SceneView.currentDrawingSceneView;
        scene.LookAt(scene.pivot, Quaternion.Euler(0, 180, 0));
    }
    
    [MenuItem("UGB/CameraControl/Left #4")]
    public static void Left()
    {    
        SceneView scene = SceneView.currentDrawingSceneView;
        scene.LookAt(scene.pivot, Quaternion.Euler(0, -270, 0));
    }
    
    
    [MenuItem("UGB/CameraControl/Right #6")]
    public static void Right()
    {    
        SceneView scene = SceneView.currentDrawingSceneView;
        scene.LookAt(scene.pivot, Quaternion.Euler(0, 270, 0));
    }
}
