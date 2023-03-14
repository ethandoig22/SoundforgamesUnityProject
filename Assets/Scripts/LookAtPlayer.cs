using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private GameObject playerPos;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(playerPos.transform.position);
    }
}
