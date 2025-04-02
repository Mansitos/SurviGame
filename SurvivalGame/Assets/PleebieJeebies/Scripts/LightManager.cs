using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField, Header("Managed Objects")] private Light DirectionalLight = null;
    [SerializeField] private LightPreset DayNightPreset, LampPreset;
    private List<Light> SpotLights = new List<Light>();

    [SerializeField, Range(0, 1440), Header("Modifiers"), Tooltip("The game's current time of day")] private float TimeOfDay;
    [SerializeField, Tooltip("Angle to rotate the sun")] private float SunDirection = 170f;
    [SerializeField] private bool ControlLights = true;
    [SerializeField] private float timeProgress = 0.0f;

    private void Start()
    {
    }

    /// This method will not run if there is no preset set
    /// On each frame, this will calculate the current time of day factoring game time and the time multiplier (1440 is how many minutes exist in a day 24 x 60)
    /// Then send a time percentage to UpdateLighting, to evaluate according to the set preset, what that time of day should look like
    private void Update()
    {
        if (DayNightPreset == null)
            return;

        UpdateLighting(timeProgress);
    }

    public void SetTimeProgress(float progress)
    {
        timeProgress = progress;
    }

    /// Based on the time percentage recieved, set the current scene's render settings and light coloring to the preset
    /// In addition, rotate the directional light (the sun) according to the current time
    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = DayNightPreset.AmbientColour.Evaluate(timePercent);
        RenderSettings.fogColor = DayNightPreset.FogColour.Evaluate(timePercent);

        //Set the directional light (the sun) according to the time percent
        if (DirectionalLight != null)
        {
            if (DirectionalLight.enabled == true)
            {
                DirectionalLight.color = DayNightPreset.DirectionalColour.Evaluate(timePercent);
                DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, SunDirection, 0));
            }
        }

        //Go through each spot light, ensure it is active, and set it's color accordingly
        foreach (Light lamp in SpotLights)
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
}