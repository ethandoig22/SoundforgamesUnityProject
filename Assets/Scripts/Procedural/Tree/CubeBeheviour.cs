using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBeheviour : MonoBehaviour
{
    public ProceduralTreeManager gmanager;
    public GameObject Cube;
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        gmanager = GameObject.Find("Manager").GetComponent<ProceduralTreeManager>();

        gmanager.count--;

        if (index  < 150 && gameObject.tag != "Root")
        {
            this.GetComponent<Renderer>().material.color = new Color(Random.Range(0.25f,0.65f), 0, Random.Range(0.25f, 0.65f), Random.value);
        }
        else 
        {
            this.GetComponent<Renderer>().material.color = new Color(0.2f, 0.6f, 0.4f);

        }

        if (gmanager.count > 1)
        {
            index = gmanager.count;
            /*Cube.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            Cube.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(Random.value, 0.4f, Random.value, Random.value));*/
            GameObject CubeInstanceOne = Instantiate(Cube);
            CubeInstanceOne.tag = "CubeChild";

            CubeInstanceOne.transform.parent = this.transform;
            CubeInstanceOne.transform.localPosition = new Vector3(-0.5f, 1f, 0f);
            CubeInstanceOne.transform.localScale = new Vector3(0.66f, 0.66f, 0.66f);
            CubeInstanceOne.transform.localRotation = Quaternion.Euler(0f, 0f, 45f);

            GameObject CubeInstanceTwo = Instantiate(CubeInstanceOne);
            CubeInstanceTwo.tag = "CubeChild2";
            CubeInstanceTwo.transform.parent = this.transform;
            CubeInstanceTwo.transform.localPosition = new Vector3(0.5f, 1f, 0f);
            CubeInstanceTwo.transform.localScale = new Vector3(0.6f, 0.66f, 0.66f);
            CubeInstanceTwo.transform.localRotation = Quaternion.Euler(0f, -180f, 45f);

            GameObject CubeInstanceThree = Instantiate(CubeInstanceTwo);
            CubeInstanceThree.tag = "CubeChild3";
            CubeInstanceThree.transform.parent = this.transform;
            CubeInstanceThree.transform.localPosition = new Vector3(0f, 1f, 0.5f);
            CubeInstanceThree.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            CubeInstanceThree.transform.localRotation = Quaternion.Euler(0f, 0f, 45f);

            GameObject CubeInstanceFour = Instantiate(CubeInstanceThree);
            CubeInstanceFour.tag = "CubeChild4";
            CubeInstanceFour.transform.parent = this.transform;
            CubeInstanceFour.transform.localPosition = new Vector3(0f, 1f, -0.5f);
            CubeInstanceFour.transform.localScale = new Vector3(0.6f, 0.6f, 0.66f);
            CubeInstanceFour.transform.localRotation = Quaternion.Euler(0f, 0f, 45f);

            
        }


    }
}
