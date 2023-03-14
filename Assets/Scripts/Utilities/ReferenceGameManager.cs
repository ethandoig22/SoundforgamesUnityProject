using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceGameManager : MonoBehaviour
{
    public GameManager gmanager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playUISound(int index) 
    {
        gmanager.PlayUIEventSound(index);
    }
}
