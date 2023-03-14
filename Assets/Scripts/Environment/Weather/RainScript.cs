
//
// This is an adaptation of Rain Maker(c) 2016 Digital Ruby, LLC
// http://www.digitalruby.com
//

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class RainScript : MonoBehaviour
{
     public float RainHeight = 25.0f;
     public float RainForwardOffset = -7.0f;
     public Camera Camera;
     public bool FollowCamera = true;

    [Range(0.0f, 1.0f)]
    public float RainIntensity;

    public ParticleSystem RainFallParticleSystem;
    public ParticleSystem RainExplosionParticleSystem;

    protected Material rainMaterial;
    protected Material rainExplosionMaterial;

    private float lastRainIntensityValue = -1.0f;
    private float nextWindTime;



     void Start()
    {


        if (RainFallParticleSystem != null)
        {
            ParticleSystem.EmissionModule e = RainFallParticleSystem.emission;
            e.enabled = false;
            Renderer rainRenderer = RainFallParticleSystem.GetComponent<Renderer>();
            rainRenderer.enabled = false;
            rainMaterial = new Material(rainRenderer.material);
            rainMaterial.EnableKeyword("SOFTPARTICLES_OFF");
            rainRenderer.material = rainMaterial;
        }
        if (RainExplosionParticleSystem != null)
        {
            ParticleSystem.EmissionModule e = RainExplosionParticleSystem.emission;
            e.enabled = false;
            Renderer rainRenderer = RainExplosionParticleSystem.GetComponent<Renderer>();
            rainExplosionMaterial = new Material(rainRenderer.material);
            rainExplosionMaterial.EnableKeyword("SOFTPARTICLES_OFF");
            rainRenderer.material = rainExplosionMaterial;
        }
    }

     

private void UpdateRain()
        {
            if (RainFallParticleSystem != null)
            {
                if (FollowCamera)
                {
                    var s = RainFallParticleSystem.shape;
                    s.shapeType = ParticleSystemShapeType.ConeVolume;
                    RainFallParticleSystem.transform.position = Camera.transform.position;
                    RainFallParticleSystem.transform.Translate(0.0f, RainHeight, RainForwardOffset);
                    RainFallParticleSystem.transform.rotation = Quaternion.Euler(0.0f, Camera.transform.rotation.eulerAngles.y, 0.0f);                                                           
                }
                else
                {
                    var s = RainFallParticleSystem.shape;
                    s.shapeType = ParticleSystemShapeType.Box;
                  
                }
            }
        }

       

    void Update()
        {
        CheckForRainChange();
        UpdateRain();
        }

    private void CheckForRainChange()
    {
        if (lastRainIntensityValue != RainIntensity)
        {
            lastRainIntensityValue = RainIntensity;

                if (RainFallParticleSystem != null)
                {
                    ParticleSystem.EmissionModule e = RainFallParticleSystem.emission;
                    e.enabled = RainFallParticleSystem.GetComponent<Renderer>().enabled = true;
                    if (!RainFallParticleSystem.isPlaying)
                    {
                        RainFallParticleSystem.Play();
                    }
                    ParticleSystem.MinMaxCurve rate = e.rateOverTime;
                    rate.mode = ParticleSystemCurveMode.Constant;
                    rate.constantMin = rate.constantMax = RainFallEmissionRate();
                    e.rateOverTime = rate;
                }       
        }
    }

    public float RainFallEmissionRate()
    {
        return (RainFallParticleSystem.main.maxParticles / RainFallParticleSystem.main.startLifetime.constant) * RainIntensity;
    }



}




