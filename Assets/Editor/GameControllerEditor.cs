using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameController))]
public class GameControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GameController controller = (GameController) target;
        
        DrawDefaultInspector();
        if (GUILayout.Button("End game"))
        {
            controller.onGameOver.Invoke();
        }
    }
}
