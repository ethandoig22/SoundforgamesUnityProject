using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pheromone : MonoBehaviour
{
    public float strength;
    void Start()
    {
        if (transform.root.gameObject.GetComponent<FoodSource>() != null)
        {
            strength = transform.root.gameObject.GetComponent<FoodSource>().nFood;
            StartCoroutine(reduceStrengthOverTime());
        }
    }

    void Update()
    {


        if (transform.root.gameObject == this.gameObject)
        {
            Destroy(gameObject);
        }

        if (strength == 0) 
        {

                Destroy(gameObject);

            }

        
    }

    public IEnumerator reduceStrengthOverTime()
    {
            while (strength > 0)
            {
                this.GetComponent<Renderer>().material.color = new Color(0, 1, 0, strength);
                strength--;
                yield return new WaitForSeconds(0.25f);
            }
        
    }




}
