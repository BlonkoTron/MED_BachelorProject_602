using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class Audiomanager : MonoBehaviour
{
    public static Audiomanager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("NOPE");
            return;
        }
        instance = this;
    }

    // Play a sound at a specific position
    public EventInstance PlaySound(EventReference sound, Vector3 position)
    {
        EventInstance instance = RuntimeManager.CreateInstance(sound);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        instance.start();
        return instance;
    }

    public void StopSound(EventInstance instance)
    {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
    }

    public void UpdateSoundPosition(EventInstance instance, Vector3 position)
    {
        //HAS TO BE CALLE DIN UPDATE TO WORK!!!!
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
    }

    public void PauseSound(EventInstance instance, bool pause)
    {
        instance.setPaused(pause);
    }
}
