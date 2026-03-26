using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Paranoid : MonoBehaviour
{
    [Header("Paranoia Settings")]
    public bool causeParanoia = true;
    public float triggerRadius = 5f;
    public float paranoiaIncreasePerSecond = 0.02f;

    [Header("Flash Effect")]
    public Image blackFlashImage;
    public float flashDuration = 15f;
    public float flashSpeed = 6f;

    private SphereCollider triggerCollider;
    private Needs playerNeeds;
    private bool flashingStarted = false;

    void Awake()
    {
        triggerCollider = GetComponent<SphereCollider>();

        if (triggerCollider == null)
        {
            triggerCollider = gameObject.AddComponent<SphereCollider>();
        }

        triggerCollider.isTrigger = true;
        triggerCollider.radius = triggerRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!causeParanoia) return;

        if (other.CompareTag("Player"))
        {
            playerNeeds = other.GetComponent<Needs>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNeeds = null;
        }
    }

    private void Update()
    {
        if (!causeParanoia || playerNeeds == null) return;

        playerNeeds.paranoiaPercent += paranoiaIncreasePerSecond * Time.deltaTime;
        playerNeeds.paranoiaPercent = Mathf.Clamp01(playerNeeds.paranoiaPercent);

        if (playerNeeds.paranoiaPercent >= 1f && !flashingStarted)
        {
            flashingStarted = true;
            StartCoroutine(FlashBlack());
        }
    }

    IEnumerator FlashBlack()
    {
        float timer = 0f;
        Color color = blackFlashImage.color;

        while (timer < flashDuration)
        {
            timer += Time.deltaTime;

            float alpha = Mathf.PingPong(Time.time * flashSpeed, 1f);
            color.a = alpha;
            blackFlashImage.color = color;

            yield return null;
        }

        color.a = 0f;
        blackFlashImage.color = color;

        // reset paranoia
        if (playerNeeds != null)
            playerNeeds.paranoiaPercent = 0f;

        flashingStarted = false; // allow it to happen again
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = causeParanoia ? Color.magenta : Color.gray;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}