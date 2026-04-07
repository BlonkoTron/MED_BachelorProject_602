using FMODUnity;
using FMOD.Studio;
using UnityEngine;
using FMOD;
using Mono.Cecil;

public class AudioTest : MonoBehaviour
{
    private EventInstance Soundtest;
    [SerializeField] private EventReference tester;

    private void Update()
    {
        //update position
        Audiomanager.instance.UpdateSoundPosition(Soundtest, transform.position);
    }

    private void Start()
    {
        //The sound does not follow because it takes the position one time and does not follow
        Soundtest = Audiomanager.instance.PlaySound(tester, transform.position);
    }

}