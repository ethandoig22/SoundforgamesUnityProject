using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flammable : MonoBehaviour
{
    public bool onFire;
    public int FireDamage;
    public GameObject FireInstance;
    public bool isDead;

    public void whenOnFire() 
    {
        StartCoroutine(whilstOnFire());
    }

    public IEnumerator whilstOnFire()
    {

            while (FireDamage < 5)
            {
                yield return new WaitForSeconds(1f);
                FireDamage++;
            }
            ParticleSystem.EmissionModule FireParticleSystemEmitter = FireInstance.GetComponent<ParticleSystem>().emission;
            ParticleSystem.EmissionModule SmokeParticleSystemEmitter = FireInstance.transform.GetChild(0).GetComponent<ParticleSystem>().emission;
            FireParticleSystemEmitter.enabled = false;
            SmokeParticleSystemEmitter.enabled = false;

            if (GetComponent<Renderer>() != null)
            {
                isDead = true;
                GetComponent<Renderer>().material.color = new Color(0, 0, 0);
                StartCoroutine(Die());
            }
            Destroy(FireInstance, 5f);
            onFire = false;
           
    }

    public IEnumerator Die()
    {
        float Scale = transform.parent.lossyScale.y;

        while (Scale > 0)
        {
            Scale -= 0.01f;
            transform.parent.localScale = new Vector3(Scale, Scale, Scale);
            yield return new WaitForSeconds(0.1f);
              
        }
        Destroy(transform.parent.gameObject);
      
    }


}
