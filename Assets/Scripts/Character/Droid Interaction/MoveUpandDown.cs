using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpandDown : MonoBehaviour
{
    private float angle;
    // Start is called before the first frame update
    void Start()
    {
        angle = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (GameObject.Find("Player").GetComponent<Player>().inputIndex == 1)
        {
            if (angle < 0.45f)
            {
                angle += Time.deltaTime;
                transform.localPosition = new Vector3(transform.localPosition.x, angle, transform.localPosition.z);
            }

        }
        else 
        {
            if (angle > 0) 
            {

                angle -= Time.deltaTime * 0.01f;
                transform.localPosition = new Vector3(transform.localPosition.x, angle, transform.localPosition.z);
            }
        
        }
    }
}
