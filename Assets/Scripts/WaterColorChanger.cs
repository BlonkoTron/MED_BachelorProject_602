using UnityEngine;

public class WaterColorChanger : MonoBehaviour
{
    [SerializeField] private Renderer[] waterRenderers;
    [SerializeField] private string colorPropertyName = "_WaterColor";
    [SerializeField] private Color brownColor = new Color(0.4f, 0.2f, 0.05f, 1f);

    private Color originalColor;
    private bool initialized = false;

    private void Start()
    {
        if (waterRenderers == null || waterRenderers.Length == 0)
        {
            waterRenderers = GetComponentsInChildren<Renderer>();
        }

        if (waterRenderers != null && waterRenderers.Length > 0 && waterRenderers[0] != null)
        {
            originalColor = waterRenderers[0].material.GetColor(colorPropertyName);
            initialized = true;
        }
    }

    private void Update()
    {
        if (!initialized) return;

        int barrenTiles = TileTypeAndAmountUI.Instance.barrenTiles;
        int threshold = GameManager.Instance.BarrenTilesLoseThreshold;
        int fullyBrownAt = Mathf.Max(1, threshold - 5);

        float t = Mathf.Clamp01((float)barrenTiles / fullyBrownAt);
        Color targetColor = Color.Lerp(originalColor, brownColor, t);

        foreach (Renderer r in waterRenderers)
        {
            if (r != null)
                r.material.SetColor(colorPropertyName, targetColor);
        }
    }
}
