using UnityEngine;
using UnityEngine.UI;

public class GradientColor : MonoBehaviour
{
    [Range(0, 100)]
    public float value;

    public Gradient gradient;

    private Image img;

    void Awake()
    {
        img = GetComponent<Image>();

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

        img.color = color;
    }
}