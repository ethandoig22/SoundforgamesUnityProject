using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCollider : MonoBehaviour
{
    public bool GrowFast, growSlow;
    public List<GameObject> currentCollisions = new List<GameObject>();
    public GameObject TreeManager;
    public float growthSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TreeManager.GetComponent<ProceduralTreeManager>().x < 360f)
        {
            if (currentCollisions.Count == 1)
            {
                TreeManager.GetComponent<ProceduralTreeManager>().x += Time.deltaTime * growthSpeed;
            }
            else if (currentCollisions.Count == 2)
            {
                TreeManager.GetComponent<ProceduralTreeManager>().x += Time.deltaTime * growthSpeed * 2;
            }

        }

        if (TreeManager.GetComponent<ProceduralTreeManager>().x >= 39f) 
        {
            if (currentCollisions.Count == 0)
            {
                TreeManager.GetComponent<ProceduralTreeManager>().x -= Time.deltaTime * growthSpeed * 3;
            }

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "OutterBeam" || other.tag == "InnerBeam")
        {
            currentCollisions.Add(other.gameObject);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "OutterBeam" || other.tag == "InnerBeam")
        {
            currentCollisions.Remove(other.gameObject);
        }
    }




}
