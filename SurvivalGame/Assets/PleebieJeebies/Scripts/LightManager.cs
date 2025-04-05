using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField, Header("Managed Objects")] private Light DirectionalLight = null;
    [SerializeField] private LightPreset DayNightPreset, LampPreset;
    private List<Light> PointLights = new List<Light>();

    [SerializeField, Tooltip("Angle to rotate the sun")] private float SunDirection = 170f;
    [SerializeField] private bool ControlLights = true;

    private float timeProgress = 0.0f;
    private float timeElapsed = 0.0f;

    private float startProgress = 0.0f;
    private float endProgress = 1.0f;
    private float durationInSeconds = 100.0f;

    private void Start()
    {
        FindLights();
    }

    // Set the light manager's time range and duration
    // startProgress and endProgress are percentages (0 to 1) corresponding to the start and end times of the day.
    // For example, if the start time is 4 AM, startProgress is 4/24.
    public void SetLightManager(float startProgress, float endProgress, float durationInSeconds)
    {
        this.timeProgress = startProgress;
        this.startProgress = startProgress;
        this.endProgress = endProgress;
        this.durationInSeconds = durationInSeconds;
    }

    public void ResetTime()
    {
        timeElapsed = 0.0f;
    }

    /// Updates the time progress based on the real-world time elapsed.
    /// timeProgress is calculated as a fraction of the total time between startProgress and endProgress.
    private void Update()
    {
        if (DayNightPreset == null)
            return;

        timeElapsed += Time.deltaTime;

        // Calculate timeProgress as a fraction of the day cycle between startProgress and endProgress
        timeProgress = startProgress + (timeElapsed / durationInSeconds) * (endProgress - startProgress);

        // Clamp timeProgress to stay within bounds (0 to 1)
        timeProgress = Mathf.Clamp01(timeProgress);

        // Update lighting based on the current time progress
        UpdateLighting(timeProgress);
    }

    /// Updates the scene's lighting and fog based on the time progress
    /// Adjusts the sun's position and color, and updates spotlights.
    private void UpdateLighting(float timePercent)
    {
        // Update ambient light and fog color based on the time of day
        RenderSettings.ambientLight = DayNightPreset.AmbientColour.Evaluate(timePercent);
        RenderSettings.fogColor = DayNightPreset.FogColour.Evaluate(timePercent);

        // Update the directional light (sun) based on the time progress
        if (DirectionalLight != null)
        {
            if (DirectionalLight.enabled)
            {
                DirectionalLight.color = DayNightPreset.DirectionalColour.Evaluate(timePercent);
                DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, SunDirection, 0));
            }
        }

        // Update each spotlight if it's active and set its color based on the time progress
        foreach (Light lamp in PointLights)
        {
            if (lamp != null)
            {
                if (lamp.isActiveAndEnabled && lamp.shadows != LightShadows.None && LampPreset != null)
                {
                    lamp.color = LampPreset.DirectionalColour.Evaluate(timePercent);
                }
            }
        }
    }

    public void FindLights()
    {
        // Find all lights in the scene
        Light[] allLights = FindObjectsOfType<Light>();

        // Filter spotlights from all the lights
        foreach (var light in allLights)
        {
            if (light.type == LightType.Point)
            {
                PointLights.Add(light);
            }
        }
    }
}
