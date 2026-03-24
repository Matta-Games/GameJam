using UnityEngine;
using TMPro;
using System.Collections;

public class BlizzardManager : MonoBehaviour
{
    [Header("Blizzard Settings")]
    public float blizzardCheckInterval = 10f;
    public float blizzardChance = 0.25f;
    public float blizzardDuration = 15f;
    public float fogDensityDelay = 2f;
    public float blizzardFogDensity = 0.5f;
    public float blizzardCooldown = 20f;

    [Header("Shake Settings")]
    public float shakeIntensity = 10f;
    public float shakeSpeed = 25f;

    private Vector3 warningOriginalPos;

    [Header("UI")]
    public TextMeshProUGUI warningText;
    public float warningDuration = 2f;

    [Header("Fog Settings")]
    public float normalFogDensity = 0.1f;

    private float blizzardTimer = 0f;
    private bool isBlizzardActive = false;
    private float cooldownTimer = 0f;

    void Start()
    {
        blizzardTimer = blizzardCheckInterval;

        if (RenderSettings.fog)
        {
            RenderSettings.fogDensity = normalFogDensity;
        }

        if (warningText != null)
        {
            warningText.text = "";
            warningOriginalPos = warningText.rectTransform.localPosition;
        }
    }

    void ShakeWarning()
    {
        if (warningText == null) return;

        Vector2 randomOffset = Random.insideUnitCircle * shakeIntensity;
        warningText.rectTransform.localPosition =
            warningOriginalPos + new Vector3(randomOffset.x, randomOffset.y, 0f);
    }

    void Update()
    {
        blizzardTimer -= Time.deltaTime;

        // Decrease cooldown timer
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (isBlizzardActive)
        {
            ShakeWarning();
        }
        else if (warningText != null)
        {
            warningText.rectTransform.localPosition = warningOriginalPos;
        }

        if (blizzardTimer <= 0f && !isBlizzardActive && cooldownTimer <= 0f)
        {
            // Check if blizzard should happen
            if (Random.value < blizzardChance)
            {
                StartCoroutine(TriggerBlizzard());
            }

            blizzardTimer = blizzardCheckInterval;
        }
    }

    IEnumerator TriggerBlizzard()
    {
        isBlizzardActive = true;

        // Show warning text
        if (warningText != null)
        {
            warningText.text = "Blizzard approaching\r\nVisibility will be reduced!";
            warningText.alpha = 1f;
        }

        // Wait 2 seconds
        yield return new WaitForSeconds(fogDensityDelay);

        // Increase fog density
        RenderSettings.fogDensity = blizzardFogDensity;

        // Wait for blizzard duration
        yield return new WaitForSeconds(blizzardDuration);

        // Return to normal fog density
        RenderSettings.fogDensity = normalFogDensity;

        // Hide warning text
        if (warningText != null)
        {
            warningText.text = "";
        }

        isBlizzardActive = false;

        // Start cooldown
        cooldownTimer = blizzardCooldown;
    }
}