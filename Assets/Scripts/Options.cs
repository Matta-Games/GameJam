using UnityEngine;

public class Options : MonoBehaviour
{
    [Header("UI Canvas")]
    public GameObject optionsCanvas;  // assign your options panel here

    [Header("Key to open/close")]
    public KeyCode toggleKey = KeyCode.Escape;

    private bool isOpen = false;

    void Start()
    {
        if (optionsCanvas != null)
            optionsCanvas.SetActive(false); // start hidden
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey) && optionsCanvas != null)
        {
            isOpen = !isOpen;
            optionsCanvas.SetActive(isOpen);

            // Optional: unlock cursor when menu is open
            Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isOpen;
        }
    }
}