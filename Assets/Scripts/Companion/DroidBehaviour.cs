using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD;
public class DroidBehaviour : MonoBehaviour
{
    private bool flag, SwitchingTool, Shooting;
    public bool canSwitch, isScanning, Alerted;
    private Transform target;
    public GameObject PlayerCompanionTarget, CrossHairTarget, PlayerSphereLookAt,CrossHairLookAt;
    public FMODUnity.StudioEventEmitter FMODDroidMovement, FMODDroidRotation;
    private FMOD.Studio.EventInstance FMODToolSwitchEvent, FMODDroidChitChat, FMODDDroidScanning;
    public Transform lookAt;
    public GameManager gmanager;
    private float speed,angle;
    private Vector3 velocity, FrameVelocity, PrevPosition, angularVelocity;
    private Quaternion lastRotation;
    public bool fidgeting;
    public Light droidLight;
    void Start() 
    {
        droidLight = GameObject.Find("DroidLight").GetComponent<Light>();
        flag = false;
        InvokeRepeating("randomChitChat", 5f, Random.Range(15f, 20f));    
        FrameVelocity = new Vector3(0f,0f,0f);
        gmanager.toolIndex = 0;
        target = PlayerSphereLookAt.transform;
    }

    void Update()
    {
        scanningSoundHandler();
        isAlerted();
        isDroidAlerted();
        mouseOverUIHandler();
        MoveDroid();

        if (fidgeting)
        {
            droidLight.intensity = lerpValueOverTime(Random.value * 100f, 3f);
            droidLight.spotAngle = lerpValueOverTime(Random.value * 100f, transform.localScale.x * 50f);
        }

    }


    public void useLaser()
    {
        if (!gmanager.overUIArea) // if not over UI Area
        {
            if (Input.GetMouseButton(1)) //if user Right clicks
            {
                gmanager.rightClicking = true; //user is rightclicking
                gmanager.Player.GetComponent<Animator>().SetBool("Radio", true);//start radio animation
                gmanager.EnableGameObject(gmanager.crossHair);
                RaycastHit hit; //setup raycast
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//continued

                if (Physics.Raycast(ray, out hit))//if the raycast hits a collider
                {
                    gmanager.crossHair.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (gmanager.toolIndex == 0)
                        {
                            if (!Shooting)
                            {
                                StartCoroutine(ResetShootAnimationAfterTime());
                                GetComponent<Animator>().SetBool("Shoot", true);
                                GameObject projectileInstance = Instantiate(gmanager.projectile);
                                projectileInstance.transform.parent = transform;
                                projectileInstance.transform.position = transform.position;
                                Vector3 dir = lookAt.transform.position - projectileInstance.transform.position;
                                projectileInstance.GetComponent<Rigidbody>().AddForce(dir * 10f, ForceMode.Impulse);
                            }
                        }
                        else if (gmanager.toolIndex == 2) 
                        {
                            if (gmanager.nSeeds > 0)
                            {
                                if (hit.collider.name == "ground")
                                {
                                    if (!Shooting)
                                    {
                                        StartCoroutine(ResetShootAnimationAfterTime());
                                        GetComponent<Animator>().SetBool("Shoot", true);
                                        GameObject projectileInstance = Instantiate(gmanager.seedProjectile);
                                        projectileInstance.transform.parent = transform;
                                        projectileInstance.transform.position = transform.position;
                                        Vector3 dir = lookAt.transform.position - projectileInstance.transform.position;
                                        projectileInstance.GetComponent<Rigidbody>().AddForce(dir * 10f, ForceMode.Impulse);
                                    }
                                    GameObject cactusInstance = Instantiate(gmanager.Cacti[Random.Range(0, gmanager.Cacti.Length)]);
                                    cactusInstance.transform.position = hit.point;

                                    gmanager.nSeeds--;
                                    gmanager.SeedCountText.text = gmanager.nSeeds.ToString();
                                    gmanager.SeedCountTextDropShadow.text = gmanager.nSeeds.ToString();
                                }
                            }
                        }
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            gmanager.Player.GetComponent<Animator>().SetBool("Radio", false);
            gmanager.DisableGameObject(gmanager.crossHair);
            gmanager.rightClicking = false;
        }
    }


    public void laserTypeSelect()
    {
        if (canSwitch)
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                if (!SwitchingTool)
                {
                    StartCoroutine(ResetSwitchAnimationAfterTime());
                    gmanager.toolIndex++;
                    playToolSwitchSound();
                    GetComponent<Animator>().SetBool("Switch", true);
                    if (gmanager.toolIndex > 2)
                    {
                        gmanager.toolIndex = 0;
                    }
                }
                switch (gmanager.toolIndex) 
                {
                    case 0:
                        gmanager.companion.GetComponentInChildren<Light>().color = Color.blue; //laser
                        gmanager.companion.transform.GetChild(0).GetComponent<Renderer>().materials[7].SetColor("_EmissionColor", Color.blue * 1.5f);
                        gmanager.companion.transform.GetChild(0).GetComponent<Renderer>().materials[2].SetColor("_EmissionColor", Color.blue * 1.5f);
                        FMODToolSwitchEvent.setParameterByName("ToolSelector", 0);
                        break;
                    case 1:
                        gmanager.companion.GetComponentInChildren<Light>().color = Color.white; //scanner
                        gmanager.companion.transform.GetChild(0).GetComponent<Renderer>().materials[7].SetColor("_EmissionColor", Color.white * 1.5f);
                        gmanager.companion.transform.GetChild(0).GetComponent<Renderer>().materials[2].SetColor("_EmissionColor", Color.white * 1.5f);
                        FMODToolSwitchEvent.setParameterByName("ToolSelector", 1);
                        break;
                    case 2:
                        gmanager.companion.GetComponentInChildren<Light>().color = Color.green; //planter
                        gmanager.companion.transform.GetChild(0).GetComponent<Renderer>().materials[7].SetColor("_EmissionColor", Color.green * 1.5f);
                        gmanager.companion.transform.GetChild(0).GetComponent<Renderer>().materials[2].SetColor("_EmissionColor", Color.green * 1.5f);
                        FMODToolSwitchEvent.setParameterByName("ToolSelector", 2);
                        break;                
                }
            }
        }
    }

    public float lerpValueOverTime(float freq, float amplitude)
    {
        return Mathf.Abs(Mathf.Sin(angle += Time.deltaTime * freq) * amplitude);
    }

    public void resetAnimBool(string clip) 
    {
        GetComponent<Animator>().SetBool(clip, false);

    }

    public IEnumerator ResetSwitchAnimationAfterTime() 
    {
        SwitchingTool = true;
        yield return new WaitForSeconds(0.25f);
        SwitchingTool = false;
        GetComponent<Animator>().SetBool("Switch", false);
    }

    public IEnumerator ResetShootAnimationAfterTime()
    {
        Shooting = true;
        yield return new WaitForSeconds(0.15f);
        Shooting = false;
        GetComponent<Animator>().SetBool("Shoot", false);
    }



    public void randomChitChat() 
    {
        if (!gmanager.rightClicking)
        {
            if (gmanager.Intensity == 0 || gmanager.Intensity == 5)
            {
                if (Random.value > 0.5f)
                {
                    GetComponent<Animator>().SetBool("FidgetOne", true);
                }
                else
                {
                    GetComponent<Animator>().SetBool("FidgetTwo", true);
                }
                fidgeting = true;
                playRandomChitChatSound();
            }
        }
    }

    public void resetFidgets() 
    {
        fidgeting = false;
        droidLight.intensity = 2f;
        droidLight.spotAngle = 65f;
        GetComponent<Animator>().SetBool("FidgetOne", false);
        GetComponent<Animator>().SetBool("FidgetTwo", false);
    }

    public void scanningSoundHandler() 
    {
        if (isScanning)
        {
            if (!flag)
            {
                startScanningSound();
                flag = true;
            }
        }
        else
        {
            stopScanningSound();
            flag = false;
        }

    }

    public void MoveDroid() 
    {
        //******Get Objects velocity without Rigidbody****
        Vector3 currFrameVelocity = (transform.position - PrevPosition) / Time.deltaTime;
        FrameVelocity = Vector3.Lerp(FrameVelocity, currFrameVelocity, 0.05f);
        PrevPosition = transform.position;
        FMODDroidMovement.SetParameter("DroidVelocity", FrameVelocity.magnitude); //update fmod
        //******Get Objects angular velocity without Rigidbody****
        Quaternion deltaRot = transform.rotation * Quaternion.Inverse(lastRotation);
        Vector3 eulerRot = new Vector3(Mathf.DeltaAngle(0, deltaRot.eulerAngles.x), Mathf.DeltaAngle(0, deltaRot.eulerAngles.y), Mathf.DeltaAngle(0, deltaRot.eulerAngles.z));
        angularVelocity = eulerRot / Time.deltaTime;
        lastRotation = transform.rotation;
        FMODDroidRotation.SetParameter("Rotation", angularVelocity.magnitude);//update fmod
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z), ref velocity, speed); //move droid to target smoothly
        if (lookAt != null)
        {
            Quaternion rotation = Quaternion.LookRotation(lookAt.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 15f); //rotate the droid to face target (this is a different object to the position target)
        }

        if (gmanager.rightClicking) //if the user right clicks
        {
            speed = 0.75f; //make droid move slightly quicker
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(target.transform.position.x, target.transform.position.y + 10f, target.transform.position.z), ref velocity, speed); //set the target pos to be 10 crosshair 
            target = CrossHairTarget.transform;//target is the crosshair(raycast from mouse)
            lookAt = CrossHairLookAt.transform;// look at the target
            if (gmanager.CanlockOnToWorm)
            {
                lookAt = GameObject.Find("Bone04").transform;
            }
            else
            {
                return;
            }
        }
        else
        {
            speed = 0.3f; //slow droid down when following player
            target = PlayerCompanionTarget.transform; //target the player
            lookAt = PlayerSphereLookAt.transform; //look at the sphere that is invisible and floats above player
        }
    }

    public void mouseOverUIHandler() 
    {
        if (gmanager.overUIArea)
        {
            gmanager.DisableGameObject(gmanager.crossHair);
            gmanager.Player.GetComponent<Animator>().SetBool("Radio", false);
        }
    }

    public void isDroidAlerted() 
    {
        if (gmanager.Intensity > 0 && gmanager.Intensity < 5)
        {
            GetComponent<Animator>().SetBool("Alerted", true);
            Alerted = true;
        }
        else
        {
            GetComponent<Animator>().SetBool("Alerted", false);
            Alerted = false;
        }
    }

    public void isAlerted() 
    {
        if (!gmanager.gameIsPaused)
        {
            if (!Alerted)
            {
                laserTypeSelect();
            }
            else
            {
                gmanager.toolIndex = 0;
                gmanager.companion.GetComponentInChildren<Light>().color = Color.blue; //laser
                gmanager.companion.transform.GetChild(0).GetComponent<Renderer>().materials[7].SetColor("_EmissionColor", Color.red * 1.5f);
                gmanager.companion.transform.GetChild(0).GetComponent<Renderer>().materials[2].SetColor("_EmissionColor", Color.red * 1.5f);
            }
            useLaser();
        }
    }


    //***************** FMOD Event instances **************************
    public void playToolSwitchSound()
    {
        FMODToolSwitchEvent = FMODUnity.RuntimeManager.CreateInstance("event:/Companion/SwitchTool");
        FMODToolSwitchEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        FMODToolSwitchEvent.start();
        FMODToolSwitchEvent.release();
    }

    public void playRandomChitChatSound()
    {
        FMODDroidChitChat = FMODUnity.RuntimeManager.CreateInstance("event:/Companion/ChitChat");
        FMODDroidChitChat.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        FMODDroidChitChat.start();
        FMODDroidChitChat.release();
    }
    public void startScanningSound()
    {
        FMODDDroidScanning = FMODUnity.RuntimeManager.CreateInstance("event:/Companion/Scan");
        FMODDDroidScanning.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        FMODDDroidScanning.start();
        FMODDDroidScanning.release();
    }

    public void stopScanningSound()
    {
        FMODDDroidScanning.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        FMODDDroidScanning.release();
    }
}

