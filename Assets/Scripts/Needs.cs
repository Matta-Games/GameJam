using UnityEngine;

public class Needs : MonoBehaviour
{
    [Header("Bars (transform that you want to scale X)")]
    public Transform bodyTempBar;
    public Transform fatigueBar;
    public Transform paranoiaBar;

    [Header("Current Percentages 0-1")]
    [Range(0f, 1f)] public float bodyTempPercent = 0.2f;
    [Range(0f, 1f)] public float fatiguePercent = 0.2f;
    [Range(0f, 1f)] public float paranoiaPercent = 0.2f;

    [Header("Increase per 45s")]
    public float fatigueRatePer45s = 0.0098f;
    public float paranoiaRatePer45s = 0.0120f;

    [Header("Heat System")]
    public bool isBeingHeated = false;
    public float heatGainPerSecond = 0.05f;
    public float coldDrainPerSecond = 0.01f;

    private float timer = 0f;
    private const float interval = 45f;

    private Vector3 bodyTempBaseScale;
    private Vector3 fatigueBaseScale;
    private Vector3 paranoiaBaseScale;

    void Start()
    {
        // Normalize all bars so FULL = scale.x = 1
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
        // Continuous body temperature change
        if (isBeingHeated)
        {
            bodyTempPercent += heatGainPerSecond * Time.deltaTime;
        }
        else
        {
            bodyTempPercent -= coldDrainPerSecond * Time.deltaTime;
        }

        bodyTempPercent = Mathf.Clamp01(bodyTempPercent);

        // Passive fatigue/paranoia increase every 45s
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            fatiguePercent += fatigueRatePer45s;
            paranoiaPercent += paranoiaRatePer45s;

            fatiguePercent = Mathf.Clamp01(fatiguePercent);
            paranoiaPercent = Mathf.Clamp01(paranoiaPercent);

            timer = 0f;
        }

        // Update all bars visually
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
}