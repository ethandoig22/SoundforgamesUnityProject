using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class AnimationManager : MonoBehaviour
{
    public GameObject wormCollider;
    public GameObject Target;
    public WormBehaviour worm;
    private CinemachineImpulseSource _ImpulseSource;

    private void Start()
    {
        _ImpulseSource = GetComponent<CinemachineImpulseSource>();

        worm = GameObject.Find("GoldSandWormUnderground").GetComponent<WormBehaviour>();
        Shake();

        Target = GameObject.Find("Player");
    }


    private void Update()
    {
        if (!worm.isDead)
        {
            transform.LookAt(Target.transform);// always look at player
        }
    }

    public void Destroy()
    {     
        worm.inCombatMode = false;
        worm.canMove = true;
        Destroy(gameObject);
    }
    public void resetDamage() 
    {
        GetComponent<Animator>().SetBool("Damage", false);
    
    }


    public void Shake()
    {
        _ImpulseSource.GenerateImpulse();

    }

}
