using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using UnityEngine.AI;
public class PlayerProjectileBehaviour : MonoBehaviour
{
    private GameManager gmanager;
    //7 rock, 8 sand, metal 9
    public GameObject[] ImpactParticleEffect;
    private FMOD.Studio.EventInstance ProjectileImpactSound;
    public int surfaceIndex;

    void Start()
    {
        gmanager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Player")
        {
            Vector3 normalDir = other.GetContact(0).point + other.GetContact(0).normal;
            Vector3 impactPoint = other.GetContact(0).point + other.GetContact(0).normal;

            UnityEngine.Debug.DrawLine(other.GetContact(0).point, other.GetContact(0).point + other.GetContact(0).normal, Color.white, 2.5f);

            if (other.gameObject.layer == 7) //rock
            {
                
                SelectParticleSystemTypeAndSpawn(ImpactParticleEffect[0], normalDir, impactPoint,0);
                Destroy(gameObject);
            }
            else if (other.gameObject.layer == 8) //sand
            {
        
                SelectParticleSystemTypeAndSpawn(ImpactParticleEffect[1], ImpactParticleEffect[1].transform.position, other.GetContact(0).point, 1);
                Destroy(gameObject);
            }
            else if (other.gameObject.layer == 9) //metal
            {
           
                SelectParticleSystemTypeAndSpawn(ImpactParticleEffect[2], normalDir, impactPoint,2);
                Destroy(gameObject);
            }

            else if (other.gameObject.layer == 18)//flesh
            {
                AnimationManager wormAnimManager = other.transform.root.GetComponent<AnimationManager>();
                SelectParticleSystemTypeAndSpawn(ImpactParticleEffect[3], normalDir, impactPoint,3);
                wormAnimManager.worm.Health--;
                if (wormAnimManager.worm.Health <= 0)
                {
                    wormAnimManager.wormCollider.GetComponent<CapsuleCollider>().enabled = false;
                    if (wormAnimManager.worm.Lives <= 3)
                    {
                        wormAnimManager.worm.intensity++;
                    }
                    wormAnimManager.worm.wormInstance.GetComponent<Animator>().SetBool("Damage", true); // pop down
                    wormAnimManager.worm.wormInstance.GetComponent<Animator>().SetBool("Attack", false); // pop down
                    wormAnimManager.worm.inCombatMode = false; //move out of combat mode
                    wormAnimManager.worm.wormSpawned = false; //worm is no longer above the surface
                    wormAnimManager.worm.Lives--;
                    if (wormAnimManager.worm.Lives == 0)
                    {
                        gmanager.CanlockOnToWorm = false;
                        wormAnimManager.worm.wormInstance.GetComponent<Animator>().SetBool("Damage", false); // pop down
                        wormAnimManager.worm.wormInstance.GetComponent<Animator>().SetBool("Attack", false); // pop down
                        wormAnimManager.worm.wormInstance.GetComponent<Animator>().SetBool("Die", true); // pop down                      
                        wormAnimManager.worm.targettingPlayer = false;
                        wormAnimManager.worm.intensity = 0;
                        gmanager.Intensity = 0;
                        gmanager.music.SetParameter("Intensity", gmanager.Intensity);

                        GameObject.Find("Player").transform.GetComponentInChildren<NavMeshObstacle>().enabled = true;
                        wormAnimManager.worm.isDead = true;
                        Destroy(GameObject.Find("GoldSandWormUnderground").gameObject);
                    }
                }
                Destroy(gameObject);
            }
            else if (other.gameObject.layer == 22) //plant
            {
                SelectParticleSystemTypeAndSpawn(ImpactParticleEffect[4], normalDir, impactPoint, 4);
                Destroy(gameObject);
            }
        }
    }

    public void SelectParticleSystemTypeAndSpawn(GameObject pSystemType, Vector3 Forward, Vector3 spawnPos, int surfaceIndex)
    {
        GameObject particleEffectInstance = Instantiate(pSystemType);
        particleEffectInstance.transform.position = spawnPos;
        particleEffectInstance.transform.LookAt(Forward);
        playImpactSound(particleEffectInstance, surfaceIndex);
        Destroy(particleEffectInstance, 5f);
    }

    public void playImpactSound(GameObject particleInstance, int surfaceTypeIndex) 
    {
        ProjectileImpactSound = FMODUnity.RuntimeManager.CreateInstance("event:/Projectiles/PlayerProjectileHit");
        ProjectileImpactSound.setParameterByName("SurfaceType", surfaceTypeIndex);
        ProjectileImpactSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(particleInstance));
        ProjectileImpactSound.start();
        ProjectileImpactSound.release();
    }


}
