using UnityEngine;

public class WaterColorChanger : MonoBehaviour
{
    [SerializeField] private Renderer[] waterRenderers;
    [SerializeField] private string colorPropertyName = "_WaterColor";
    [SerializeField] private Color brownColor = new Color(0.4f, 0.2f, 0.05f, 1f);
    [SerializeField] private Color foamBrownColor = new Color(0.8f, 0.35f, 0.05f, 1f);
    [SerializeField] private string foamColorProperty = "_FoamColor";
    [SerializeField] public string foamDensProperty = "_FoamDens";
    [SerializeField] private float foamDensityTarget = 3f;

    private Color originalColor;
    private Color originalFoamColor;
    private float originalFoamDens;
    private MaterialPropertyBlock propertyBlock;
    private bool initialized = false;

    private void Start()
    {
        if (waterRenderers == null || waterRenderers.Length == 0)
        {
            waterRenderers = GetComponentsInChildren<Renderer>();
        }

        if (waterRenderers != null && waterRenderers.Length > 0 && waterRenderers[0] != null)
        {
            Material mat = waterRenderers[0].sharedMaterial;
            originalColor = mat.GetColor(colorPropertyName);
            originalFoamColor = mat.GetColor(foamColorProperty);
            originalFoamDens = mat.GetFloat(foamDensProperty);
            propertyBlock = new MaterialPropertyBlock();
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
        Color targetFoamColor = Color.Lerp(originalFoamColor, foamBrownColor, t);
        float targetFoamDensity = Mathf.Lerp(originalFoamDens, foamDensityTarget, t);

        foreach (Renderer r in waterRenderers)
        {
            if (r != null)
            {
                r.GetPropertyBlock(propertyBlock);
                propertyBlock.SetColor(colorPropertyName, targetColor);
                propertyBlock.SetColor(foamColorProperty, targetFoamColor);
                propertyBlock.SetFloat(foamDensProperty, targetFoamDensity);
                r.SetPropertyBlock(propertyBlock);
            }
        }
    }
}
