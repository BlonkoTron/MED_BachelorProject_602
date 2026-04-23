using UnityEngine;

public class RandomAnimTrigger : MonoBehaviour
{
    public Animator anim;
    public float waitTime = 5f;
    private float timer;
    private bool initialized = false;

    void Start()
    {
        anim.enabled = false;
        timer = Random.Range(0, waitTime);
        initialized = true;
    }

    void Update()
    {
        if (!initialized) return;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = waitTime + Random.Range(0f, 45f);

            anim.enabled = true;
            anim.SetTrigger("AnimGo");
        }
    }

    // Call this via Animation Event at the end of the "Barren" clip
    public void OnAnimationComplete()
    {
        anim.enabled = false;
    }
}