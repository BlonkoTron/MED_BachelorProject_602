using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class RandomAnimTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator anim;

    [Header("Timing")]
    [SerializeField] private float minWait = 30f;
    [SerializeField] private float maxWait = 120f;

    [Header("Tile Type")]
    [SerializeField] private TileType tileType;

    private float timer;

    [Header("FMOD Events")]
    [SerializeField] private EventReference tileFarm_SFX;
    [SerializeField] private EventReference tileCow_SFX;
    [SerializeField] private EventReference tileMine_SFX;
    [SerializeField] private EventReference tileAggro_SFX;

    private EventInstance currentInstance;

    private void Start()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }

        anim.enabled = false;
        ResetTimer();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            TriggerAnimation();
            ResetTimer();
        }

        // Keep sound attached to object position
        if (currentInstance.isValid())
        {
            Audiomanager.instance.UpdateSoundPosition(currentInstance, transform.position);
        }
    }

    private void TriggerAnimation()
    {
        anim.enabled = true;
        anim.SetTrigger("AnimGo");

        PlayTileSound();
    }

    private void ResetTimer()
    {
        timer = Random.Range(minWait, maxWait);
    }

    private void PlayTileSound()
    {
        EventReference selectedEvent = default;

        switch (tileType)
        {
            case TileType.Farm:
                selectedEvent = tileFarm_SFX;
                break;

            case TileType.CowField:
                selectedEvent = tileCow_SFX;
                break;

            case TileType.Mine:
                selectedEvent = tileMine_SFX;
                break;

            case TileType.Agroforest:
                selectedEvent = tileAggro_SFX;
                break;
        }

        if (!selectedEvent.IsNull)
        {
            currentInstance = Audiomanager.instance.PlaySound(selectedEvent, transform.position);
        }
    }

    // Hook this to an Animation Event on the LAST FRAME
    public void OnAnimationComplete()
    {
        anim.enabled = false;

        if (currentInstance.isValid())
        {
            Audiomanager.instance.StopSound(currentInstance);
            currentInstance.clearHandle(); // prevents stale reference issues
        }
    }
}