using UnityEngine;
using UnityEngine.UI;

public class MessageTrigger : MonoBehaviour
{
    [Header("UI Settings")]
    public Image messageImage; // Assign your UI Image in inspector

    private bool playerInRange = false;

    private void Start()
    {
        if (messageImage != null)
            messageImage.gameObject.SetActive(false); // make sure image starts hidden
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (messageImage != null)
                messageImage.gameObject.SetActive(true); // show the image
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (messageImage != null)
                messageImage.gameObject.SetActive(false); // hide the image when F pressed
        }
    }
}