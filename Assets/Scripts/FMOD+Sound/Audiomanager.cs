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

    public EventInstance PlaySound(EventReference sound, Vector3 position)
    {
        if (sound.IsNull)
        {
            Debug.LogWarning("Audiomanager: Tried to play a null or missing sound!");
            return default; // returns an "empty" EventInstance
        }

        EventInstance instance = RuntimeManager.CreateInstance(sound);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        instance.start();
        return instance;
    }

    public void StopSound(EventInstance instance)
    {
        if (!instance.isValid())
            return;

        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
    }

    public void UpdateSoundPosition(EventInstance instance, Vector3 position)
    {
        if (!instance.isValid())
            return;

        instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
    }

    public void PauseSound(EventInstance instance, bool pause)
    {
        if (!instance.isValid())
            return;

        instance.setPaused(pause);
    }
}