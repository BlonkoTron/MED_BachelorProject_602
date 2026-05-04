using UnityEngine;
using TMPro;

public class FactBoxController : MonoBehaviour
{
    public static FactBoxController Instance;

    [SerializeField] private GameObject factBoxPanel;
    [SerializeField] private TextMeshProUGUI factBoxText;

    [Tooltip("Drag the empty GameObject here to set the fact box position.")]
    [SerializeField] private Transform factBoxPositionReference;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Ensure it starts hidden
        factBoxPanel.SetActive(false);
    }

    public void ShowFactBox(string text)
    {
        factBoxPanel.SetActive(true);
        factBoxText.text = text;

        // Move the panel to the position of the reference transform
        if (factBoxPositionReference != null)
        {
            factBoxPanel.transform.position = factBoxPositionReference.position;
        }
    }

    public void HideFactBox()
    {
        factBoxPanel.SetActive(false);
    }
}