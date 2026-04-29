using UnityEngine;
using TMPro;
using System.Collections;

public class PointsUI : MonoBehaviour
{
    private PointSystem pointSystem;

    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private Animator moneyTextAnimator;
    [SerializeField] private TMP_Text moneyAddedText;
    [SerializeField] private Animator moneyAddedTextAnimator;
    [SerializeField] private TMP_Text moneyPerDayText;
    [SerializeField] private AnimationCurve moneyCountCurve;
    [SerializeField] private float moneyCountAnimTime=1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pointSystem = PointSystem.Instance;
        pointSystem.onMoneyEarned.AddListener(OnMoneyAdded);
        pointSystem.onMoneySpent.AddListener(OnMoneyRemoved);
        pointSystem.onMoneyLost.AddListener(OnMoneyRemoved);
        TileTypeAndAmountUI.Instance.onTileUIUpdate.AddListener(UpdateMoneyPerDay);
        GameManager.Instance.onGameTick.AddListener(ShowMoneyAddedPerDay);

        moneyPerDayText.text = "+" + TileTypeAndAmountUI.Instance.GetTotalMoneyGain().ToString() + "/dag";
        moneyText.text = pointSystem.CurrentMoney.ToString();
    }

    private void OnMoneyAdded(int money)
    {
        moneyAddedText.text ="+"+money.ToString();
        moneyAddedTextAnimator.SetTrigger("moneyAdd");
        StartCoroutine(CountUpMoney(pointSystem.CurrentMoney- money, pointSystem.CurrentMoney));
        moneyTextAnimator.SetTrigger("moneyAdd");
    }
    private void OnMoneyRemoved(int money)
    {
        moneyAddedText.text ="-"+ money.ToString();
        moneyAddedTextAnimator.SetTrigger("moneyRemove");
        StartCoroutine(CountUpMoney(pointSystem.CurrentMoney + money, pointSystem.CurrentMoney));
        moneyTextAnimator.SetTrigger("moneyRemove");
    }
    private void UpdateMoneyPerDay()
    {
        moneyPerDayText.text = "+" + TileTypeAndAmountUI.Instance.GetTotalMoneyGain().ToString() + "/dag";
        //moneyAddedText.text = "+" + TileTypeAndAmountUI.Instance.GetTotalMoneyGain().ToString();
        //moneyAddedTextAnimator.SetTrigger("moneyAdd");
    }
    private void ShowMoneyAddedPerDay()
    {
        moneyAddedText.text = "+" + TileTypeAndAmountUI.Instance.GetTotalMoneyGain().ToString();
        moneyAddedTextAnimator.SetTrigger("moneyAdd");
    }

    private IEnumerator CountUpMoney(int startMoney,int newMoney)
    {
        float moneyToDisplay = startMoney;
        float journey = 0f;
        while (journey <= moneyCountAnimTime)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey / moneyCountAnimTime);
            moneyToDisplay = Mathf.Lerp((float)startMoney, (float)newMoney, moneyCountCurve.Evaluate(percent));
            moneyText.text =Mathf.RoundToInt(moneyToDisplay).ToString();
            yield return null;
        }
        moneyText.text = pointSystem.CurrentMoney.ToString();

    }

}
