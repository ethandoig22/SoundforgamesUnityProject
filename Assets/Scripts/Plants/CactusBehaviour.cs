using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CactusBehaviour : MonoBehaviour
{
    private float scaleIncrement;
    private float randomScale;
    private FMOD.Studio.EventInstance CactusGrowEvent;

    private CinemachineImpulseSource _ImpulseSource;

    // Start is called before the first frame update
    void Start()
    {

        _ImpulseSource = GetComponent<CinemachineImpulseSource>();

        scaleIncrement = 0;
        randomScale = Random.Range(0.3f,1.0f);
        transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        playGrowthSound();
        Shake();

    }

    // Update is called once per frame
    void Update()
    {

        if (scaleIncrement < randomScale)
        {
            scaleIncrement += Time.deltaTime * 0.5f;
            transform.localScale = new Vector3(scaleIncrement, scaleIncrement, scaleIncrement);
            CactusGrowEvent.setParameterByName("CactusGrowth", scale(0,randomScale,0,1,scaleIncrement));
        }
        else 
        {
            ParticleSystem ps = transform.GetChild(0).GetComponent<ParticleSystem>();
            var em = ps.emission;
            em.enabled = false;
        }

    }

    public void playGrowthSound()
    {
        CactusGrowEvent = FMODUnity.RuntimeManager.CreateInstance("event:/Cactus/Grow");
        CactusGrowEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        CactusGrowEvent.start();
        CactusGrowEvent.release();

    }


    public float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }
    public void Shake()
    {
        _ImpulseSource.GenerateImpulse();

    }
}
