using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class RandomAnimTrigger : MonoBehaviour
{
    public Animator anim;

    public float minWait = 30f;
    public float maxWait = 120f;

    public TileType tileType;

    private float _timer;

    [Header("FMOD Events")]
    [SerializeField] private EventReference TileFarm_SFX;
    [SerializeField] private EventReference TileCow_SFX;
    [SerializeField] private EventReference TileMine_SFX;
    [SerializeField] private EventReference TileAggro_SFX;

    private EventInstance currentInstance;

    void Start()
    {
        anim.enabled = false;
        _timer = Random.Range(2f, maxWait);
    }

    void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            _timer = Random.Range(minWait, maxWait);

            anim.enabled = true;
            anim.SetTrigger("AnimGo");

            PlayTileSound();
        }

        // Optional: keep sound following object if it moves
        if (currentInstance.isValid())
        {
            Audiomanager.instance.UpdateSoundPosition(currentInstance, transform.position);
        }
    }

    void PlayTileSound()
    {
        EventReference selectedEvent = default;

        switch (tileType)
        {
            case TileType.Farm:
                selectedEvent = TileFarm_SFX;
                break;

            case TileType.CowField:
                selectedEvent = TileCow_SFX;
                break;

            case TileType.Mine:
                selectedEvent = TileMine_SFX;
                break;

            case TileType.Agroforest:
                selectedEvent = TileAggro_SFX;
                break;
        }

        if (!selectedEvent.IsNull)
        {
            currentInstance = Audiomanager.instance.PlaySound(selectedEvent, transform.position);
        }
    }

    // Called from animation event (last frame)
    public void OnAnimationComplete()
    {
        anim.enabled = false;

        // Stop sound when animation ends (optional)
        if (currentInstance.isValid())
        {
            Audiomanager.instance.StopSound(currentInstance);
        }
    }
}