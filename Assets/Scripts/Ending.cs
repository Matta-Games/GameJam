using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Ending : MonoBehaviour
{
    [Header("UI Settings")]
    public Image endImage; // Assign your UI Image in inspector
    public float fadeDuration = 5f;

    [Header("Player Settings")]
    public MonoBehaviour playerController; // Assign the player controller script here

    [Header("Main Menu")]
    public string mainMenuSceneName = "MainMenu"; // Name of your main menu scene

    private bool triggered = false;
    private bool fadeComplete = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return; // prevent multiple triggers
        if (other.CompareTag("Player"))
        {
            triggered = true;

            // Disable player controller
            if (playerController != null)
                playerController.enabled = false;

            // Start the fade-in
            StartCoroutine(FadeInEndImage());
        }
    }

    private IEnumerator FadeInEndImage()
    {
        // Make sure the image is visible but transparent
        endImage.gameObject.SetActive(true);
        Color color = endImage.color;
        color.a = 0f;
        endImage.color = color;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime; // unscaled because we will stop time
            color.a = Mathf.Clamp01(timer / fadeDuration);
            endImage.color = color;
            yield return null;
        }

        fadeComplete = true;
        Time.timeScale = 0f; // freeze the game after fade
    }

    private void Update()
    {
        if (fadeComplete && Input.GetKeyDown(KeyCode.F))
        {
            // Reset timescale before loading the main menu
            Time.timeScale = 1f;
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}