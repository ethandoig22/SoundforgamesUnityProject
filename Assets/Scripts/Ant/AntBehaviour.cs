using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AntBehaviour : MonoBehaviour
{

    private bool flag;
    private FMOD.Studio.EventInstance AntIndividualInstanceFMOD;
    private GameObject Hive;
    public float wanderRadius;
    private Transform target;
    private NavMeshAgent agent;
    public float speed;
    public bool trackPlayer;
    public AntManager antmanager;
    // Use this for initialization
    public Vector3 lookDir;
    public int Index, NodeIndex; //Ant index (number asigned to agent on start), NodeIndex (sibling index of node when it is a child of a pathparent)
    public GameObject pheromone, PathParent; //pheromne prefab //pathparent prefab
    public bool hasFood, isProducingPheromone, onPheromoneTrailToFood; //booleans

    private GameObject FoodSource, PathParentInstantiate;
    private Vector3 lastDestination;

    private void Start()
    {

        GetComponent<Animator>().Play("AntWalk", -1, Random.Range(0.0f, 1.0f));
        GetComponent<Animator>().speed = Random.Range(0.5f, 1.0f);

        antmanager = GameObject.Find("AntManager").GetComponent<AntManager>();
        Hive = GameObject.Find("AntHive").gameObject;
        agent = GetComponent<NavMeshAgent>();
        Vector3 newPos = RandomNavSphere(Hive.transform.position, wanderRadius, -1);
        agent.SetDestination(newPos);
    }


    // Update is called once per frame
    void Update()
    {
        //FaceTarget(agent.destination);

        if (agent.velocity.magnitude > 2f && !flag) 
        {
            AntIndividualInstanceFMOD = FMODUnity.RuntimeManager.CreateInstance("event:/Ant/Ant");
            AntIndividualInstanceFMOD.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            AntIndividualInstanceFMOD.start();
            AntIndividualInstanceFMOD.release();
            flag = true;
        }

        if (agent.velocity.magnitude < 2f) 
        {
            flag = false;
        }
        //*************************************FMOD*************************************
        

        if (agent.velocity.magnitude < 0.005f) //check if the agent has stopped
        {
            onPheromoneTrailToFood = false;
            if (Vector3.Distance(transform.position, agent.destination) < 1f)
            {
                Vector3 newPos = RandomNavSphere(Hive.transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
            }
        }


        //**************************** Agent picks up pheromone trail to food **************************************

        else if (onPheromoneTrailToFood) //if the agent has come across a pheromone trail - see pheromone trigger enter
        {
            if (NodeIndex > 0)//stops the count from going into the minuses (stops child count out of bounds)
            {
                if (PathParentInstantiate != null) //if the path parent still exists
                {
                    if (PathParentInstantiate.transform.childCount > NodeIndex) //if the path still has nodes 
                    {
                        if (PathParentInstantiate.transform.GetChild(NodeIndex) != null) //if the node still exists in the path
                        {
                            if (PathParentInstantiate.transform.GetChild(NodeIndex).GetComponent<Pheromone>().strength > 30) //if the node is relatively fresh
                            {
                                agent.destination = PathParentInstantiate.transform.GetChild(NodeIndex).transform.position; // set agent to travel to that node

                                if (agent.remainingDistance < 2) //if the remaining distance to that node is less than 2
                                {
                                    NodeIndex--; //decrement the node index to the next node child
                                }
                            }
                        }
                        else
                        {
                            onPheromoneTrailToFood = false;
                        }
                    }
                    else
                    {
                        onPheromoneTrailToFood = false;
                    }
                }
                else
                {
                    onPheromoneTrailToFood = false;
                }
            }
        }

        else
        {
            onPheromoneTrailToFood = false;
        }
    }




public  Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation,Time.deltaTime * speed);
        lookDir = lookPos;
    }

 


    //*************************** On trigger enters *************************************

    void OnTriggerEnter(Collider other)
    {
        //*********************************** Agent enters FOODSOURCE ******************************************

        if (other.tag == "FoodSource") //if the agent enters a food source
        {
            onPheromoneTrailToFood = false; //no longer on trail to food as the agent has found food
                                            //    agent.radius = 0.01f; //this stops the agents from colliding with other agents when in the foodsource
            FoodSource = other.gameObject; //set the foodsource to be the one entered


            if (!hasFood) //if the agent doesn't have food
            {
                if (FoodSource.GetComponent<FoodSource>().nFood > 0) //if the source has food available
                {
                    FoodSource.GetComponent<FoodSource>().nFood--; //take one away from the foodsources total
                    gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true; //render food infront of agent
                    hasFood = true;//the agent now has food
                }
            }
            else //if the agent already has food
            {

                isProducingPheromone = false; //stop producing a pheromnone trail (this stops the agent from polluting the foodsource with nodes
                StopCoroutine(SpawnPheromoneOverTime(pheromone, 10f, FoodSource));

            }
            agent.destination = Hive.transform.position;
        }

        //*********************************** Agent enters HIVE ******************************************

        if (other.tag == "Hive") //if the agent walks over the hive 
        {
            //  agent.radius = 0.0f; //this stops the agent from colliding with other agents in the hive
            Hive = other.gameObject; //set the hive to be the hive entered

            if (hasFood) //if the agent has food
            {
                gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false; //render food infront of agent

                antmanager.FoodCount++; //add one to the hives foodcount
                if (antmanager.proceduralTreeManager.x < 360f)
                {
                    antmanager.proceduralTreeManager.x += 0.25f;
                }
                //  gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;//stop rendering food infront of the agent
                hasFood = false;//the agent no longer has food
                onPheromoneTrailToFood = false;//???

                if (isProducingPheromone) //if the agent is producing a trail
                {
                    if (PathParentInstantiate != null)
                    {
                        GameObject pheromoneInstantiate = Instantiate(pheromone);//spawn a pheromone at the agents position
                        pheromoneInstantiate.transform.position = other.transform.position; //spawn it in the centre of the food source
                        pheromoneInstantiate.transform.parent = PathParentInstantiate.transform;

                        PathParentInstantiate.GetComponent<PathManager>().RenderLineThroughPath();
                        isProducingPheromone = false;
                        StopCoroutine(SpawnPheromoneOverTime(pheromone, 10f, FoodSource)); // stop producing a trail

                    }
                }
            }
        }

        //*********************************** Agent enters Pheromone ******************************************

        if (other.tag == "Pheromone") //if the agent comes across a pheromone trail
        {
            if (!hasFood && other.GetComponent<Pheromone>().strength > 25) //if the agent doesnt have food and the trail is fresh
            {
                if (!onPheromoneTrailToFood) // if the agent isn't already following a pheromone trail to food
                {

                    NodeIndex = other.transform.GetSiblingIndex(); // its position index in the path
                    PathParentInstantiate = other.transform.parent.gameObject; //the path that pheromone belongs to
                    onPheromoneTrailToFood = true; //now on trail to food                    
                }
            }
        }

 
    }

    //*************************** On trigger exits *************************************

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "FoodSource")
        {
            if (other.transform.childCount < 1)
            {

                isProducingPheromone = true;//the agent is now producing a pheromone path
                StartCoroutine(SpawnPheromoneOverTime(pheromone, 30f, FoodSource));

            }
            //Radius = 25f;
        }

    }

    //*********************************** Spawn Pheromones ***************************************************
    public IEnumerator SpawnPheromoneOverTime(GameObject pheromoneType, float duration, GameObject FoodSourceOrHive)
    {
        StartCoroutine(SpawnTimer(duration));
        SpawnPheromoneAtSpecificLocation(FoodSource, pheromone);
        while (isProducingPheromone)
        {
            GameObject pheromoneInstantiate = Instantiate(pheromoneType);//spawn a pheromone at the agents position
            pheromoneInstantiate.transform.position = this.gameObject.transform.position;// else spawn it at the location of the ant
            if (PathParentInstantiate != null)
            {
                pheromoneInstantiate.transform.parent = PathParentInstantiate.transform;
            }
            yield return new WaitForSeconds(0.1f);//wait before spawning another
        }
    }


    public IEnumerator SpawnTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        isProducingPheromone = false;
    }

    public void SpawnPheromoneAtSpecificLocation(GameObject FoodSourceOrHive, GameObject pheromoneType)
    {
        PathParentInstantiate = Instantiate(PathParent); //instantiate a parent so that we can nest the path nodes
        PathParentInstantiate.transform.parent = FoodSourceOrHive.transform; //make it a child of the foodsource
        PathParentInstantiate.transform.position = FoodSourceOrHive.transform.position;
        GameObject pheromoneInstantiate = Instantiate(pheromoneType);//spawn a pheromone at the agents position
        pheromoneInstantiate.transform.position = PathParentInstantiate.transform.position; //spawn it in the centre of the food source
        pheromoneInstantiate.transform.parent = PathParentInstantiate.transform;

        pheromoneInstantiate = Instantiate(pheromoneType);//spawn a pheromone 
        pheromoneInstantiate.transform.position = (this.gameObject.transform.position + PathParentInstantiate.transform.position) / 2; //spawn it between the agent and  the food source
        pheromoneInstantiate.transform.parent = PathParentInstantiate.transform;

    }

   
}






/*

*/