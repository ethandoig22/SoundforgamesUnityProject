// Patrol.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class WormBehaviour : MonoBehaviour
{
    public bool isDead;
    public bool EncounteredPlayer;
    public FMODUnity.StudioEventEmitter FMODMoleEvent;
    public int Health, SpawnRadius;
    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    private Transform tempGO;
    public GameObject goldWorm, player, Target;
    public LayerMask Sand;
    public bool damaged, canMove, inCombatMode, wormSpawned;
    public GameObject wormInstance;
    public int Lives;
    private GameManager gmanager;
    public bool targettingPlayer;
    public bool hasntPoppedUpYet;
    public float intensity;
    void Start()
    {
        intensity = 0;
        gmanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Lives = 3;
        Health = 5;
        wormInstance = null;
        Shuffle(); //Randomise agent destinations when not following player
        agent = GetComponent<NavMeshAgent>(); // a reference to the agent
        agent.autoBraking = false; // this can result in odd looking movement
        GotoNextPoint();//start moving 
        canMove = true; //will start underground so can move on start
        inCombatMode = false;
    }

    void Update()
    {
        if (agent.velocity.magnitude > 1f) 
        {
            GetComponent<CameraShake>().Shake();
        }

        gmanager.Intensity = intensity;
        gmanager.music.SetParameter("Intensity", gmanager.Intensity);
        transform.LookAt(Target.transform);// always look at player

        if (!targettingPlayer)
        {
            player.GetComponentInChildren<NavMeshObstacle>().enabled = true;

            intensity = 0;
            Lives = 3;
            Health = 5;
            hasntPoppedUpYet = false;
            EncounteredPlayer = false;
        }


        if (!isDead)
        {
            transform.LookAt(Target.transform);// always look at player
        }
        if (!inCombatMode) //if not in combat mode
        {
            gmanager.CanlockOnToWorm = false;

            if (canMove) //if able to move
            {
                MoveUnderGround(); //move underground
            }
        }
        else 
        {
            combatMode(); //enter combat mode
        }
    }



    public void SpawnWorm() //this handles when and where the worm pops up
    {               
            inCombatMode = true;
            StartStopUndergroundParticleEffect(false, 1);//stop rubble effect
            canMove = false;
            RaycastHit hit; //setup raycast

            if (Physics.Raycast(player.transform.position + Random.onUnitSphere * SpawnRadius, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, Sand)) //fire a ray from above player but randomly around him
            {
                Vector3 SpawnPos = hit.point;

                if (wormInstance == null) //if the worm hasn't surfaced
                {
                if (!hasntPoppedUpYet)
                {
                    intensity ++;
                } 

                hasntPoppedUpYet = true;
                wormInstance = Instantiate(goldWorm);
                Health = 5;
                wormInstance.transform.position = SpawnPos;
                agent.Warp(SpawnPos); //move the worm to where the ray hit
                agent.isStopped = true; //stop the agent from moving
                }
            }
      
    }

    public void MoveUnderGround() 
    {
        
        agent.isStopped = false; //allow the agent to move
        StartStopUndergroundParticleEffect(true, 0);//start rubble effect
        
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
            GotoNextPoint();
            }
           if (Vector3.Distance(agent.transform.position, Target.transform.position) <= 30) //if the player gets close 
            {
            targettingPlayer = true;
            TargetPlayerUnderground(); //move towards the player
            }
            else
            {
            targettingPlayer = false;
            agent.stoppingDistance = 0f; //when not following the player, this make sures the agent reaches the destination
            }           
    }


    public void combatMode()
    {
        canMove = false; //stop movement
        gmanager.CanlockOnToWorm = true;

        if (Vector3.Distance(agent.transform.position, player.transform.position) > 20) //if the player gets too far away
        {
          
            wormInstance.GetComponent<Animator>().SetBool("Attack", false); // pop down

            inCombatMode = false; //move out of combat mode
            wormSpawned = false; //worm is no longer above the surface

        }

      
    }


    public void StartStopUndergroundParticleEffect(bool enable, int fmodSwitch) 
    {
        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        ParticleSystem.EmissionModule em = ps.emission;
        em.enabled = enable;
        FMODMoleEvent.SetParameter("MoleMovement", fmodSwitch);
    }


    public void TargetPlayerUnderground() 
    {

        if (!EncounteredPlayer) 
        {
            intensity = 1;
            EncounteredPlayer = true;
        }
        player.GetComponentInChildren<NavMeshObstacle>().enabled = false; 
        agent.destination = Target.transform.position;
        agent.stoppingDistance = 2f;
        if (Vector3.Distance(agent.transform.position, Target.transform.position) <= 2f)
        {
            StartStopUndergroundParticleEffect(false, 1);
            Invoke("SpawnWorm", 2f);
        }
        else
        {
            CancelInvoke();
        }

    }

   
    public void resetAnimationBool(string clip)
    {
        GetComponent<Animator>().SetBool(clip, false);
    }

    void GotoNextPoint() //when not following player, go to path node
    {
        if (points.Length == 0)
        {
            return;
        }
        agent.destination = points[destPoint].position;
        destPoint = (destPoint + 1) % points.Length;
    }

    public void Shuffle() //this randomises the path nodes
    {
        for (int i = 0; i < points.Length; i++)
        {
            int rnd = Random.Range(0, points.Length);
            tempGO = points[rnd];
            points[rnd] = points[i];
            points[i] = tempGO;
        }
    }

}
