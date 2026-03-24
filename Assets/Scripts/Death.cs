using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Death : MonoBehaviour
{
    [Header("References")]
    public Needs needs;
    public Image deathScreen;

    [Header("Settings")]
    public string mainMenuSceneName = "MainMenu";
    public float fadeSpeed = 1.5f;

    private bool isDead = false;

    void Start()
    {
        if (deathScreen != null)
        {
            Color c = deathScreen.color;
            c.a = 0f;
            deathScreen.color = c;
        }

        Time.timeScale = 1f;
    }

    void Update()
    {
        if (!isDead)
        {
            if (needs != null && needs.bodyTempPercent <= 0f)
            {
                Die();
            }
        }
        else
        {
            FadeIn();

            // Check input even when time is frozen
            if (Input.GetKeyDown(KeyCode.F))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(mainMenuSceneName);
            }
        }
    }

    void Die()
    {
        isDead = true;
        Time.timeScale = 0f;
    }

    void FadeIn()
    {
        if (deathScreen == null) return;

        Color c = deathScreen.color;
        c.a += fadeSpeed * Time.unscaledDeltaTime;
        c.a = Mathf.Clamp01(c.a);
        deathScreen.color = c;
    }
}