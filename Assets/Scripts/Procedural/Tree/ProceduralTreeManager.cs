using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProceduralTreeManager : MonoBehaviour
{

    public GameObject cube;
    public int count;
    private float n = 1;
    private GameObject[] cubeChildren1,cubeChildren2, cubeChildren3, cubeChildren4;
    [Range(0, 360)]
    public float x,y,z;
    public float scale, WholeScale;
    public GameObject TreeStartPoint;
    private FMOD.Studio.EventInstance ProceduralTreeFMODEvent;

    void Start() { 

        count = 25;
        GameObject cubeInstance = Instantiate(cube); //spawn one cube;
        cubeInstance.tag = "Root";
        cubeInstance.name = "Root";
        cubeInstance.transform.position = TreeStartPoint.transform.position;
        cubeInstance.transform.localRotation = TreeStartPoint.transform.localRotation;

        ProceduralTreeFMODEvent = FMODUnity.RuntimeManager.CreateInstance("event:/Procedural/Tree");
        ProceduralTreeFMODEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        ProceduralTreeFMODEvent.start();
        ProceduralTreeFMODEvent.release();
    }

    void Update()
    {
        y += 0.5f;
        GameObject.FindGameObjectWithTag("Root").transform.localScale = new Vector3(x / 360f * WholeScale, x / 360f * WholeScale, x / 360f * WholeScale);

        ProceduralTreeFMODEvent.setParameterByName("TreeScale", x /360f);
        ProceduralTreeFMODEvent.setParameterByName("TreeRotation", GameObject.FindGameObjectWithTag("Root").transform.GetChild(0).rotation.y);

        cubeChildren1 = GameObject.FindGameObjectsWithTag("CubeChild");

        foreach (GameObject cubeChileOne in cubeChildren1)
        {
            cubeChileOne.transform.localRotation = Quaternion.Euler(x, y, z + 45f);
            cubeChileOne.transform.localScale = new Vector3(scale, scale, scale);

        }

        cubeChildren2 = GameObject.FindGameObjectsWithTag("CubeChild2");

        foreach (GameObject cubeChildtwo in cubeChildren2)
        {
            cubeChildtwo.transform.localRotation = Quaternion.Euler(x, y - 180f, z + 45f);
            cubeChildtwo.transform.localScale = new Vector3(scale, scale, scale);

        }

        cubeChildren3 = GameObject.FindGameObjectsWithTag("CubeChild3");

        foreach (GameObject cubeChildThree in cubeChildren3)
        {
            cubeChildThree.transform.localRotation = Quaternion.Euler(x, y +90f, z + 45f);
            cubeChildThree.transform.localScale = new Vector3(scale, scale, scale);

        }

        cubeChildren4 = GameObject.FindGameObjectsWithTag("CubeChild4");

        foreach (GameObject cubeChildFour in cubeChildren4)
        {
            cubeChildFour.transform.localRotation = Quaternion.Euler(x, y - 90f, z + 45f);
            cubeChildFour.transform.localScale = new Vector3(scale, scale, scale);

        }
    }






}




