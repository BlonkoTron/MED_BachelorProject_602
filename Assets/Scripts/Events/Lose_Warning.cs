using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using FMOD.Studio;
using FMODUnity;

public class Lose_Warning : MonoBehaviour
{
    public static Lose_Warning Instance;

    [SerializeField] private float flashDuration;

    public bool Warning_bio = false;
    public bool Warning_happy = false;

    public Image imagewar;
    public Sprite imagewarclicked;
    public Sprite imagewarNOTclicked;

    private EventInstance Warning;
    [SerializeField] private EventReference Warning_SFX;

    private Coroutine flashCoroutine;

    public TextMeshProUGUI warningText;
    [SerializeField] private string[] warningMessages = new string[3];

    public int Happinessthreshold = 1;
    public int Bioscorethreshold = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        imagewar.gameObject.SetActive(false);
    }

    void Update()
{
    if ((Warning_bio || Warning_happy) && flashCoroutine == null)
    {
        imagewar.sprite = imagewarNOTclicked;
        flashCoroutine = StartCoroutine(FlashRoutine());
            Warning = Audiomanager.instance.PlaySound(Warning_SFX, transform.position);
    }
    else if (!(Warning_bio || Warning_happy) && flashCoroutine != null)
    {
        StopCoroutine(flashCoroutine);
        flashCoroutine = null;
    }
}

    private IEnumerator FlashRoutine()
    {
        while (Warning_bio || Warning_happy)
        {
            imagewar.gameObject.SetActive(true);
            yield return new WaitForSeconds(flashDuration);

            imagewar.gameObject.SetActive(false);
            yield return new WaitForSeconds(flashDuration);
        }

        flashCoroutine = null;
    }

    public void info_Enable()
    {
        imagewar.sprite = imagewarclicked;
        Warning_bio = false;
        Warning_happy = false;


        if (GameManager.Instance.HappinessTicktime == Happinessthreshold && GameManager.Instance.RainscoreTicktime < Bioscorethreshold)
        {
            Debug.Log("WARNINGPISSEMAND");
            warningText.text = warningMessages[0];
        }
        else if (GameManager.Instance.RainscoreTicktime == Bioscorethreshold && GameManager.Instance.HappinessTicktime < Happinessthreshold)
        {
            warningText.text = warningMessages[1];
        }
        else if (GameManager.Instance.HappinessTicktime == Happinessthreshold && GameManager.Instance.RainscoreTicktime == Bioscorethreshold)
        {
            warningText.text = warningMessages[2];
        }
    }
}