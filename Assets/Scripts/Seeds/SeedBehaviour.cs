using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBehaviour : MonoBehaviour
{
    private GameManager gmanager;
    public int quantity;
    private bool pickedUp;
    public Animator CanvasAnimator;
    // Start is called before the first frame update
    void Start()
    {
        pickedUp = false;
        gmanager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if(other.tag == "Player" && !pickedUp) 
        {
            CanvasAnimator.SetBool("PunchSeed", true);
            Invoke("resetAnim", 1f);
            GetComponent<Animator>().SetBool("PickUp", true);

            gmanager.nSeeds += quantity;
            gmanager.SeedCountText.text = gmanager.nSeeds.ToString();
            gmanager.SeedCountTextDropShadow.text = gmanager.nSeeds.ToString();
            pickedUp = true;
            GetComponent<SphereCollider>().enabled = false;
            Destroy(gameObject,1f);

        }
        
    }

    public void resetAnim() 
    {

        CanvasAnimator.SetBool("PunchSeed", false);

    }



}
