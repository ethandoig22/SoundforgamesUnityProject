using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD;
public class JazzPlant : MonoBehaviour
{
    public float smoothed;
    public int samples;
    public FMODUnity.EventReference eventReference;
    public GameManager gManager;
    public float leftVolume;
    public float rightVolume;
    public float volume;
    FMOD.DSP_METERING_INFO inputInfo, outputInfo;

    FMOD.Studio.PLAYBACK_STATE playbackState;
    FMOD.DSP dsp;

    void Start()
    {
        StartCoroutine(PlayEventAsync());
    }

    IEnumerator PlayEventAsync()
    {
        FMODUnity.RuntimeManager.StudioSystem.getEvent(eventReference.Path, out FMOD.Studio.EventDescription eventDescription);
        eventDescription.createInstance(out FMOD.Studio.EventInstance eventInstance);
        eventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        eventInstance.start();

        // Need to wait for event instance to play before getting channel group
        do
        {
            eventInstance.getPlaybackState(out playbackState);
            yield return null;
        }
        while (playbackState != FMOD.Studio.PLAYBACK_STATE.PLAYING);

        FMOD.ChannelGroup channelGroup;
        eventInstance.getChannelGroup(out channelGroup);

        channelGroup.getDSP(FMOD.CHANNELCONTROL_DSP_INDEX.FADER, out dsp);
        dsp.setMeteringEnabled(true, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (playbackState == FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            dsp.getMeteringInfo(out inputInfo, out outputInfo);
            leftVolume = outputInfo.rmslevel[0];
            rightVolume = outputInfo.rmslevel[1];
            volume = leftVolume + rightVolume / 2f;
            smoothed = smoothed + ((volume - smoothed) / samples);//smooth this value as it is inherently noisy.
            transform.LookAt(GameObject.Find("Player").transform);
            transform.transform.localScale = new Vector3(scale(0f, 0.15f, 0f, 3f, smoothed), scale(0f, 0.15f, 0f,3f, smoothed), scale(0f, 0.15f, 0f, 3f, smoothed));
        }
    }

    public float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }
}