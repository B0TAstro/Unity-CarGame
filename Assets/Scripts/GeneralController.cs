using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralController : BaseController<GeneralController>
{
    [Header("Day/Night Settings")]
    public bool isNight = false;
    public bool autoCycle = false;
    public float cycleDuration = 120f;

    private float cycleTimer = 0f;

    [Header("Lighting")]
    public Light directionalLight;
    public Light[] nightLights;

    [Header("Skyboxes")]
    public Material daySkybox;
    public Material nightSkybox;

    void Start()
    {
        ApplyDayNightCycle();
    }

    void Update()
    {
        if (autoCycle)
        {
            cycleTimer += Time.deltaTime;
            if (cycleTimer >= cycleDuration)
            {
                cycleTimer = 0f;
                isNight = !isNight;
            }
        }
        ApplyDayNightCycle();
    }

    private void ApplyDayNightCycle()
    {
        if (isNight)
        {
            if (directionalLight != null)
                directionalLight.enabled = false;

            Light[] allLights = FindObjectsOfType<Light>();
            foreach (Light l in allLights)
            {
                if (System.Array.IndexOf(nightLights, l) >= 0)
                {
                    l.enabled = true;
                }
                else
                {
                    l.enabled = false;
                }
            }
            if (nightSkybox != null)
                RenderSettings.skybox = nightSkybox;
        }
        else
        {
            if (directionalLight != null)
                directionalLight.enabled = true;

            foreach (Light l in nightLights)
            {
                if (l != null) l.enabled = false;
            }

            if (daySkybox != null)
                RenderSettings.skybox = daySkybox;
        }
    }
}