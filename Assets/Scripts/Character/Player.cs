using UnityEngine;
using Unity;
using System.Collections;
using FMOD;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
	private FMOD.Studio.EventInstance footsteps;
	private CharacterController controller;
	public LayerMask Sand, Rock, Metal;
	public bool OnSand, OnRock, OnMetal;
	public Transform groundCheck;
	public float jumpForce;
	public float speed = 600.0f;
	public float turnSpeed = 400.0f;
	private Vector3 moveDirection = Vector3.zero;
	public float gravity = 20.0f;
	public Animator anim;
	public int inputIndex;
	public ParticleSystem LeftFootParticleSystem, RightFootParticleSystem;
	public GameObject GoldWorm;
	void Start()
	{
		controller = GetComponent<CharacterController>();
	}

	void Update()
	{
		terrainCheck();
		MovePlayer();
	}


	public void PlayRandomFootStep()
	{
		if (OnSand)
		{
			PlayFootstep(0);
		}
		else if (OnRock)
		{
			PlayFootstep(1);
		}
		else
		{
			PlayFootstep(2);
		}
	}


	private void PlayFootstep(int terrain)
	{
        footsteps = FMODUnity.RuntimeManager.CreateInstance("event:/Character/Player Footsteps");
        footsteps.setParameterByName("Surface", terrain);
        footsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        footsteps.start();
        footsteps.release();
    }


	public void LeftFootParticleEmitter() 
	{
		if (OnSand)
		{
			ParticleSystem.EmissionModule lfem = LeftFootParticleSystem.emission;
			lfem.enabled = true;
			LeftFootParticleSystem.Play();
		}
	}


	public void RightFootParticleEmitter() 
	{
		if (OnSand)
		{
			ParticleSystem.EmissionModule rfem = RightFootParticleSystem.emission;
			rfem.enabled = true;
			LeftFootParticleSystem.Play();
		}
	}

	public void terrainCheck() 
	{
		OnSand = Physics.CheckSphere(groundCheck.position, 1f, Sand);
		OnRock = Physics.CheckSphere(groundCheck.position, 1f, Rock);
		OnMetal = Physics.CheckSphere(groundCheck.position, 1f, Metal);
	}

	public void MovePlayer() 
	{
		if (Input.GetAxis("Vertical") > 0)
		{
			speed = 12;
			anim.SetInteger("AnimationPar", 1);
			inputIndex = 1;
		}
		else
		{
			speed = 0;
			anim.SetInteger("AnimationPar", 0);
			inputIndex = 0;
		}
		if (controller.isGrounded)
		{
			moveDirection = transform.forward * Input.GetAxis("Vertical") * speed;
		}
		float turn = Input.GetAxis("Horizontal");
		transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
		controller.Move(moveDirection * Time.deltaTime);
		moveDirection.y -= gravity * Time.deltaTime;
	}
}