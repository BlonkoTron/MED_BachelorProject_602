using UnityEngine;
using UnityEngine.UI; // Remove if using SpriteRenderer

public class GradientColor : MonoBehaviour
{
    [Range(0, 100)]
    public float value;

    public Gradient gradient;

    private Image img;
    // private SpriteRenderer sr;

    void Start()
    {
        img = GetComponent<Image>();
        // sr = GetComponent<SpriteRenderer>();

        UpdateColor();
    }

    public void SetValue(float newValue)
    {
        value = Mathf.Clamp(newValue, 0, 100);
        UpdateColor();
    }

    void UpdateColor()
    {
        float t = value / 100f;

        Color color = gradient.Evaluate(t);

        //img.color = color;
        //sr.color = color;
    }
}