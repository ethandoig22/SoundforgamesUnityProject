using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PathManager : MonoBehaviour
{

    public bool IsCompletePath;
    // Start is called before the first frame update

    private bool hasExistedForOneSecond;


   void Start()
    {
        StartCoroutine(shortTimer());
        
    }
    void Update()
    {

        if (hasExistedForOneSecond) 
        {
            if (transform.childCount == 0) 
            
            {
                Destroy(gameObject);           
            }                
        }

    }

    public IEnumerator shortTimer() 
    {


        yield return new WaitForSeconds(2f);
        hasExistedForOneSecond = true;
    }


    public void RenderLineThroughPath() 
    {
        this.GetComponent<LineRenderer>().positionCount = transform.childCount;

        for (int i = 0; i < transform.childCount; i++) 
        {
            this.GetComponent<LineRenderer>().SetPosition(i, transform.GetChild(i).transform.position);

        }
    }

}
