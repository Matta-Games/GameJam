using UnityEngine;
using TMPro;

public class Needs : MonoBehaviour
{
    [Header("Bars (transform that you want to scale X)")]
    public Transform bodyTempBar;
    public Transform fatigueBar;
    public Transform paranoiaBar;

    [Header("Value texts")]
    public TextMeshProUGUI moneyText;

    [Header("Current Percentages 0-1")]
    [Range(0f, 1f)] public float bodyTempPercent = 0.2f;
    [Range(0f, 1f)] public float fatiguePercent = 0.2f;
    [Range(0f, 1f)] public float paranoiaPercent = 0.2f;
    [Header("Values")]
    public int money = 2999;

    [Header("Increase per 45s")]
    public float fatigueRatePer45s = 0.0098f;
    public float paranoiaRatePer45s = 0.0120f;

    [Header("Paranoia Drain")]
    public float paranoiaDrainPerSecond = 0.01f;

    [Header("Heat System")]
    public bool isBeingHeated = false;
    public float heatGainPerSecond = 0.05f;
    public float coldDrainPerSecond = 0.01f;

    private float timer = 0f;
    private float paranoiaTimer = 0f;

    private const float interval = 45f;
    private const float paranoiaInterval = 1f;

    private Vector3 bodyTempBaseScale;
    private Vector3 fatigueBaseScale;
    private Vector3 paranoiaBaseScale;
    [Header("How often bodytemp updatess")]


    float bodyTempTimer = 0f;
    float bodyTempInterval = 1f; // 1 second

    void Start()
    {
        if (bodyTempBar != null)
        {
            bodyTempBaseScale = bodyTempBar.localScale;
            bodyTempBaseScale.x = 1f;
        }

        if (fatigueBar != null)
        {
            fatigueBaseScale = fatigueBar.localScale;
            fatigueBaseScale.x = 1f;
        }

        if (paranoiaBar != null)
        {
            paranoiaBaseScale = paranoiaBar.localScale;
            paranoiaBaseScale.x = 1f;
        }

        UpdateAllBars();
    }

    void Update()
    {
      // Body temperature (every 1 second)
      bodyTempTimer += Time.deltaTime;

        if (bodyTempTimer >= bodyTempInterval)
        {
            if (isBeingHeated)
                bodyTempPercent += heatGainPerSecond;
            else
                bodyTempPercent -= coldDrainPerSecond;

            bodyTempPercent = Mathf.Clamp01(bodyTempPercent);

            bodyTempTimer = 0f;

        }

        // 45s fatigue/paranoia increase
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            fatiguePercent += fatigueRatePer45s;
            paranoiaPercent += paranoiaRatePer45s;

            fatiguePercent = Mathf.Clamp01(fatiguePercent);
            paranoiaPercent = Mathf.Clamp01(paranoiaPercent);

            timer = 0f;
        }


        UpdateAllBars();
    }

    void UpdateAllBars()
    {
        UpdateBar(bodyTempBar, bodyTempPercent, bodyTempBaseScale);
        UpdateBar(fatigueBar, fatiguePercent, fatigueBaseScale);
        UpdateBar(paranoiaBar, paranoiaPercent, paranoiaBaseScale);
    }

    void UpdateBar(Transform bar, float percent, Vector3 baseScale)
    {
        if (bar == null) return;

        Vector3 scale = baseScale;
        scale.x = percent;
        bar.localScale = scale;
    }

    public void addMoney(int moneyToAdd) {
        money += moneyToAdd;
        moneyText.text = money.ToString();
    }

    public bool Spend(int amount)
    {
        if (money < amount)
            return false;

        money -= amount;
        moneyText.text = money.ToString(); // update UI

        return true;
    }
}
