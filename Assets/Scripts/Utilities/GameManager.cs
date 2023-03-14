using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using Cinemachine;
public class GameManager : MonoBehaviour
{
	public bool droidIsScanning;
	public bool CanlockOnToWorm;
	public GameObject compendium, Player, projectile, seedProjectile;
	public bool overUIArea, rightClicking, gameIsPaused;
	private FMOD.Studio.EventInstance UIMenuFMODEvent;
	public int nSeeds, nCactus;
	public PostProcessVolume m_PostProcessVolume;
	public GameObject crossHair;
	public Sprite[] toolUISprites;
	public int toolIndex;
	public GameObject companion;
	public Color ambientColour;
	public GameObject[] Cacti;
	public Text timeText, SeedCountText, SeedCountTextDropShadow;
	public float Intensity;
	FMOD.Studio.EventInstance snapshot;
	public bool Indoors;
	public FMODUnity.StudioEventEmitter music;
	void Start()
    {
		nSeeds = 0;
	}

	void Awake()
	{
		QualitySettings.vSyncCount = 0;  // VSync must be disabled
		Application.targetFrameRate = 120;
	}
	public void PauseGame()
	{
		GameObject.Find("CM FreeLook1").GetComponent<CinemachineFreeLook>().enabled = false;
		gameIsPaused = true;
		Player.GetComponent<Player>().enabled = false;
		Player.GetComponent<CharacterController>().enabled = false;
		ControlDepthOfField(288f);
		snapshot = FMODUnity.RuntimeManager.CreateInstance("snapshot:/Paused");
		snapshot.start();
	}
	public void ResumeGame()
	{
		GameObject.Find("CM FreeLook1").GetComponent<CinemachineFreeLook>().enabled = true;

		gameIsPaused = false;
		
		Player.GetComponent<Player>().enabled = true;
		Player.GetComponent<CharacterController>().enabled = true;
		ControlDepthOfField(70f);
		snapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		snapshot.release();
	}

	public void UIZone(bool inZone)
	{
		overUIArea = inZone;
	}


	public void EnableGameObject(GameObject gobject)
	{
		gobject.SetActive(true);
	}

	public void DisableGameObject(GameObject gobject)
	{
		gobject.SetActive(false);
	}


	private void ControlDepthOfField(float val)
	{
		if (m_PostProcessVolume != null)
		{
			DepthOfField depthOfField;
			if (m_PostProcessVolume.profile.TryGetSettings(out depthOfField))
			{
				depthOfField.focalLength.value = val;
			}
		}
	}


	public void PlayUIEventSound(int paramIndex) 
	{
		UIMenuFMODEvent = FMODUnity.RuntimeManager.CreateInstance("event:/UI/Menu");
		UIMenuFMODEvent.setParameterByName("UIMode", paramIndex);
		UIMenuFMODEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
		UIMenuFMODEvent.start();
		UIMenuFMODEvent.release();
	}

	public void insideBuilding() 
	{
		snapshot = FMODUnity.RuntimeManager.CreateInstance("snapshot:/Indoors");
		snapshot.start();
	}

	public void inCover(float coverAmount) 
	{
		FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Cover", coverAmount);
	}

	public void outsideBuilding()
	{
		snapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		snapshot.release();
	}

}




