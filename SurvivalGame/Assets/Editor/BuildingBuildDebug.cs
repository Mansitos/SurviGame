using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Building<BuildingData>),true)]
public class BuildingBuildDebug : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Building<BuildingData> building = (Building<BuildingData>)target;

        if (GUILayout.Button("Build"))
        {
            bool canBuild = building.CanBuild(GameManager.Instance);

            if (canBuild)
            {
                building.Build(GameManager.Instance);
            }

            string resultMessage = canBuild ? "Build started successfully!" : "Cannot build!";
            Debug.Log(resultMessage);
        }
    }
}
