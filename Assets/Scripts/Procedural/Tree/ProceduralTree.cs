using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralTree : MonoBehaviour
{
    public GameObject line;
    public int nIterations;
    public float n;
    public float height;
    // Start is called before the first frame update
    void Start()
    {
        height = 1f;
        n = 0.66f;
        GameObject Root = Instantiate(line);

        

        for (int i = 0; i < nIterations; i++) 
        {
            height *= n;

            GameObject lineOneInstance = Instantiate(Root);

            lineOneInstance.transform.localScale = new Vector3(lineOneInstance.transform.localScale.x, height, lineOneInstance.transform.localScale.z);
            lineOneInstance.transform.position = new Vector3(lineOneInstance.transform.position.x, lineOneInstance.transform.position.y + lineOneInstance.transform.localScale.y / 2 + lineOneInstance.transform.localScale.y / 2, lineOneInstance.transform.position.z);
            lineOneInstance.transform.Rotate(new Vector3(0f, 0f, 0f));

            Root.transform.position = lineOneInstance.transform.position;


        }



    }

}
