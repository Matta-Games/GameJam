using UnityEngine;

public class Options : MonoBehaviour
{
    [Header("UI Canvas")]
    public GameObject optionsCanvas;  // assign your options panel here

    void Start()
    {
        if (optionsCanvas != null)
            optionsCanvas.SetActive(false); // start hidden
    }

    // Call this from your button's OnClick()
    public void OpenOptions()
    {
        if (optionsCanvas != null)
            optionsCanvas.SetActive(true);
    }

    // Optional: call this from a Close button in your options panel
    public void CloseOptions()
    {
        if (optionsCanvas != null)
            optionsCanvas.SetActive(false);
    }
}