using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSource : MonoBehaviour
{
    // Start is called before the first frame update
    public int nFood;
    public int StartingFoodCount;
    void Start()
    {
        nFood = StartingFoodCount;
    }

    void Update()
    {
    }
}
