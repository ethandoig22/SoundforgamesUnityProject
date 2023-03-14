using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class WeatherManager : MonoBehaviour
{
    public WindZone windZone;
    public Animator anim;
    //public PostProcessVolume m_PostProcessVolume;
    public PostProcessProfile m_PostProcessProfile;
    public FMODUnity.StudioEventEmitter FMODWeatherEvent;
    public Light DirectionalLight;
    public RainScript rainScript;
    [Range(0f, 1f)]
    public float WindIntensity, RainIntensity, RainController;
    public Color DirectionalLightColour;
    public Color ColourFilter;
    public float postExposureAmount;
    public int chanceOfRainPercentage;
    private bool isRaining;
    private bool DoRain;
    public float RandomRainAmount;
    public Color CameraBGColor;
    public Camera mainCam;
    [Range(0f, 5f)]
    public float dlightIntensity;
    // Start is called before the first frame update


    void Start()
    {
        anim.Play("Sun", -1, 0.5f);
        InvokeRepeating("chance", 30f, 120f);
    }

    // Update is called once per frame
    void Update()
    {

        if (RainController <= 0)
        {
            RainController = 0;
        }

        WindIntensity = Mathf.PerlinNoise(Time.time * 0.01f, Time.time * 0.01f);
        windZone.windMain = WindIntensity;


            if (RainController < RandomRainAmount)
            {
                RainController += Mathf.Abs(0.0001f);
            }

            if (RainController > RandomRainAmount)
            {
                RainController -= Mathf.Abs(0.0001f);
            }

           
        DirectionalLight.intensity = dlightIntensity;
        mainCam.backgroundColor = CameraBGColor;
        rainScript.RainIntensity = RainIntensity;
        ControlColorFilter(ColourFilter);
        ControlPostExposure(postExposureAmount);
        DirectionalLight.color = DirectionalLightColour;
        FMODWeatherEvent.SetParameter("Wind", WindIntensity);
        FMODWeatherEvent.SetParameter("Rain", RainIntensity);

  
        if (RainController > 0.75f)
        {
            anim.SetFloat("Blend", 1f);
        }
        else 
        {
            anim.SetFloat("Blend", RainController);
        }
     
    }


    private void ControlColorFilter(Color color)
    {
        if (m_PostProcessProfile != null)
        {
            ColorGrading cGrading;
            if (m_PostProcessProfile.TryGetSettings(out cGrading))
            {
                cGrading.colorFilter.value = color;
            }
        }
    }

    private void ControlPostExposure(float PostExposureAmount)
    {
        if (m_PostProcessProfile != null)
        {
            ColorGrading cGrading;
            if (m_PostProcessProfile.TryGetSettings(out cGrading))
            {
                cGrading.postExposure.value = PostExposureAmount;
            }
        }
    }

    public void chance()
    {
        float random = Random.value;
        if (random < (chanceOfRainPercentage / 100f))
        {
            RandomRainAmount = Random.Range(0.25f, 1f);
        }
        else
        {
            RandomRainAmount = 0;
        }
    }

    public void doLightning() 
    {
        FMODWeatherEvent.SetParameter("Lightning", 1);
    }

    public void resetLightning() 
    {
        FMODWeatherEvent.SetParameter("Lightning", 0);
    }
    public float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;
        return (NewValue);
    }
}

  




