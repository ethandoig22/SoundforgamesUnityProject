using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBehaviour : MonoBehaviour
{
    public Material startMaterial;
    private bool hasBeenScanned;
    private GameManager gmanager;
    private DroidBehaviour droidBehaviour;
    private float angle, freq;
    public Material GlowingRed;
    private Light droidLight;
    void Start()
    {
        droidBehaviour = GameObject.Find("Droid").GetComponent<DroidBehaviour>();
        droidLight = GameObject.Find("DroidLight").GetComponent<Light>();
        gmanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        float randomV = Random.Range(0.1f, 0.3f);
        GetComponent<Renderer>().material.color = new Color(0.40f - randomV, 0.47f - randomV, 0.29f - randomV);
        startMaterial = this.GetComponent<Renderer>().material;
    }

    private void OnMouseOver()
    {
        if (!hasBeenScanned)
        {
            GameObject.Find("Canvas").GetComponent<Animator>().SetBool("Punch", false);
            if (gmanager.rightClicking)
            {
                if (gmanager.toolIndex == 1)
                {
                    if (GameObject.Find("ConeCollider").GetComponent<DroidLightCollider>().inScanPosition)
                    {
                        droidBehaviour.canSwitch = false;
                        droidBehaviour.isScanning = true;
                        gameObject.GetComponent<Renderer>().material = GlowingRed;
                        gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                        gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(1f, 1f, 1f, lerpValueOverTime(Random.value * 100f, 1f)) * lerpValueOverTime(Random.value * 100f, 1f));
                        droidLight.intensity = lerpValueOverTime(Random.value * 100f, 3f);
                        droidLight.spotAngle = lerpValueOverTime(Random.value * 100f, transform.parent.localScale.x * 50f);
                        Invoke("cloneObject", 3f);
                    }
                }
            }
            else
            {
                droidBehaviour.canSwitch = true;
                droidBehaviour.isScanning = false;
                CancelInvoke();
                gameObject.GetComponent<Renderer>().material = startMaterial;
                gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
                droidLight.intensity = 2f;
                droidLight.spotAngle = 65f;
            }
        }
    }
    

    private void OnMouseExit()
    {
        droidBehaviour.canSwitch = true;
        droidBehaviour.isScanning = false;
        if (!hasBeenScanned)
            {
            CancelInvoke();
            gameObject.GetComponent<Renderer>().material = startMaterial;
            gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");            
            }
        droidLight.intensity = 2f;
        droidLight.spotAngle = 65f;
    }


    public float lerpValueOverTime(float freq, float amplitude)
    {
        return Mathf.Abs(Mathf.Sin(angle += Time.deltaTime * freq) * amplitude);

    }

    public void cloneObject()
    {       
        if (!hasBeenScanned)
        {
            droidBehaviour.canSwitch = true;
            droidBehaviour.isScanning = false;
        GameObject.Find("Canvas").GetComponent<Animator>().SetBool("Punch", true);
        gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");    
        }
        droidLight.intensity = 2f;
        droidLight.spotAngle = 65f;
        gameObject.GetComponent<Renderer>().material = startMaterial;
        hasBeenScanned = true;
    }  
}

