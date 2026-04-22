using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HappinessUI : MonoBehaviour
{
    private Happiness happiness;

    [Header("UI References")]
    [SerializeField] private TMP_Text happinessText;
    [SerializeField] private Image happinessBarFill;
    [SerializeField] private Image happinessLevelImage;

    [Header("Happiness Level Pictures")]
    [SerializeField] private Sprite levelCritical;      // 0-20% (Red)
    [SerializeField] private Sprite levelLow;           // 21-40% (Orange)
    [SerializeField] private Sprite levelGood;          // 41-70% (Yellow)
    [SerializeField] private Sprite levelExcellent;     // 71-100% (Green)

    [Header("Color Settings")]
    [SerializeField] private bool useColorGradient = true;
    [SerializeField] private Color criticalColor = new Color(0.8f, 0.1f, 0.1f); // Red
    [SerializeField] private Color lowColor = new Color(1f, 0.6f, 0f); // Orange
    [SerializeField] private Color goodColor = new Color(1f, 1f, 0f); // Yellow
    [SerializeField] private Color excellentColor = new Color(0.2f, 0.8f, 0.2f); // Green

    [Header("Warning UI")]
    [SerializeField] private GameObject warningPanel;
    [SerializeField] private TMP_Text warningText;

    [Header("Feedback Arrows")]
    [SerializeField] private GameObject feedbackArrow;
    [SerializeField] private Sprite increaseArrowSprite;
    [SerializeField] private Sprite decreaseArrowSprite;
    
    private Image arrowImage;

    void Start()
    {
        happiness = Happiness.Instance;
        
        if (happiness == null)
        {
            Debug.LogError("HappinessUI: Happiness Instance not found!");
            return;
        }

        // Subscribe to all happiness events
        happiness.onHappinessChanged.AddListener(OnHappinessChanged);
        happiness.onHappinessIncreased.AddListener(OnHappinessIncreased);
        happiness.onHappinessDecreased.AddListener(OnHappinessDecreased);
        happiness.onHappinessCritical.AddListener(OnHappinessCritical);

        // Get arrow image component
        if (feedbackArrow != null)
        {
            arrowImage = feedbackArrow.GetComponent<Image>();
        }

        // Initial UI update
        OnHappinessChanged(happiness.HappinessLevel);

        // Hide warning panel initially
        if (warningPanel != null)
        {
            warningPanel.SetActive(false);
        }
    }

    private void OnHappinessChanged(int newHappiness)
    {
        // Update text display
        if (happinessText != null)
        {
            happinessText.text = $"{newHappiness}%";
        }

        // Update fill bar
        if (happinessBarFill != null)
        {
            happinessBarFill.fillAmount = newHappiness / 100f;
            
            // Update color based on happiness level
            if (useColorGradient)
            {
                happinessBarFill.color = GetHappinessColor(newHappiness);
            }
        }

        // Update happiness level image
        if (happinessLevelImage != null)
        {
            happinessLevelImage.sprite = GetHappinessLevelSprite(newHappiness);
        }

        // Update warning panel visibility
        UpdateWarningPanel(newHappiness);
    }

    private void OnHappinessIncreased(int amount)
    {
        // Change sprite and activate arrow
        if (feedbackArrow != null && arrowImage != null && increaseArrowSprite != null)
        {
            arrowImage.sprite = increaseArrowSprite;
            feedbackArrow.SetActive(true);
        }
        
        Debug.Log($"UI: Happiness increased by {amount}");
    }

    private void OnHappinessDecreased(int amount)
    {
        // Change sprite and activate arrow
        if (feedbackArrow != null && arrowImage != null && decreaseArrowSprite != null)
        {
            arrowImage.sprite = decreaseArrowSprite;
            feedbackArrow.SetActive(true);
        }
        
        Debug.Log($"UI: Happiness decreased by {amount}");
    }

    private void OnHappinessCritical()
    {
        // Show critical warning
        if (warningPanel != null)
        {
            warningPanel.SetActive(true);
            if (warningText != null)
            {
                warningText.text = "⚠ CRITICAL HAPPINESS! ⚠\nPopulation is very unhappy!";
            }
        }
        Debug.LogWarning("UI: Happiness is CRITICAL!");
    }

    private Color GetHappinessColor(int happiness)
    {
        if (happiness <= 20)
        {
            return criticalColor;
        }
        else if (happiness <= 40)
        {
            // Lerp between critical and low
            float t = (happiness - 20) / 20f;
            return Color.Lerp(criticalColor, lowColor, t);
        }
        else if (happiness <= 70)
        {
            // Lerp between low and good
            float t = (happiness - 40) / 30f;
            return Color.Lerp(lowColor, goodColor, t);
        }
        else
        {
            // Lerp between good and excellent
            float t = (happiness - 70) / 30f;
            return Color.Lerp(goodColor, excellentColor, t);
        }
    }

    private Sprite GetHappinessLevelSprite(int happiness)
    {
        if (happiness <= 20)
        {
            return levelCritical; // Critical - Red
        }
        else if (happiness <= 40)
        {
            return levelLow; // Low - Orange
        }
        else if (happiness <= 70)
        {
            return levelGood; // Good - Yellow
        }
        else
        {
            return levelExcellent; // Excellent - Green
        }
    }

    private void UpdateWarningPanel(int happiness)
    {
        if (warningPanel == null) return;

        if (happiness <= 20)
        {
            // Critical - always show
            warningPanel.SetActive(true);
            if (warningText != null)
            {
                warningText.text = "⚠ CRITICAL HAPPINESS! ⚠\nPopulation is very unhappy!";
            }
        }
        else if (happiness <= 40)
        {
            // Low - show warning
            warningPanel.SetActive(true);
            if (warningText != null)
            {
                warningText.text = "⚠ Low Happiness\nPeople are becoming unhappy";
            }
        }
        else
        {
            // Good or excellent - hide warning
            warningPanel.SetActive(false);
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from events
        if (happiness != null)
        {
            happiness.onHappinessChanged.RemoveListener(OnHappinessChanged);
            happiness.onHappinessIncreased.RemoveListener(OnHappinessIncreased);
            happiness.onHappinessDecreased.RemoveListener(OnHappinessDecreased);
            happiness.onHappinessCritical.RemoveListener(OnHappinessCritical);
        }
    }
}
