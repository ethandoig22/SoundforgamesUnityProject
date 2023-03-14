using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPPheromone : MonoBehaviour
{
    public float strength;

  
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(reduceStrengthOverTime());
    }

    // Update is called once per frame
    void Update()
    {
        if (strength == 0)
        {

            Destroy(gameObject);

        }

    }

    public IEnumerator reduceStrengthOverTime()
    {
        while (strength > 0)
        {
            this.GetComponent<Renderer>().material.color = new Color(0, 1, 0, strength / 100);
            strength--;
            yield return new WaitForSeconds(0.1f);
        }

    }

}
