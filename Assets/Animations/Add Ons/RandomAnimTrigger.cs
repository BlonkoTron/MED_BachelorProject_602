using UnityEngine;

public class RandomAnimTrigger : MonoBehaviour
{
    public Animator anim;
    public float minWait = 30f;
    public float maxWait = 120f;

    private float _timer;

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
        }
    }

    // Call this from an Animation Event on the LAST FRAME of your clip
    public void OnAnimationComplete()
    {
        anim.enabled = false;
    }
}