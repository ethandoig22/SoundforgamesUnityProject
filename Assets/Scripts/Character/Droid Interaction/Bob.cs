using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob : MonoBehaviour
{
    public float freq = 0.5f;
    float sine;

    void Update()
    {
        sine += Time.deltaTime * freq;
        transform.localPosition = new Vector3(transform.localPosition.x, scale(-1, 1, 0.4f, 0.5f, Mathf.Sin(sine)), transform.localPosition.z);

    }

    public float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }
}
