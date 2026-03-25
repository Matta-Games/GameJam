using UnityEngine;
using System.Collections;

public class NPCD : MonoBehaviour
{
    [Header("Dialogue")]
    [TextArea(2, 5)]
    public string[] dialogueLines;

    [Header("Voice Lines (same order as dialogue)")]
    public AudioClip[] voiceLines;

    public AudioSource voiceSource;

    private bool playerInRange = false;
    private bool isTalking = false;
    private int currentLine = 0;

    [Header("Shop / Camera")]
    public Camera playerCamera;
    public Camera shopCamera;

    [Header("Player Control")]
    public PlayerController playerController;

    private bool inShop = false;
    private bool shopTransitioning = false;

    void Start()
    {
        if (shopCamera != null)
            shopCamera.enabled = false;

        if (playerController == null)
            playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (playerController == null) return;

        if (playerInRange && playerController.interactPressed)
        {
            if (!isTalking && !inShop)
            {
                StartDialogue();
            }
            else if (isTalking)
            {
                NextLine();
            }
            else if (inShop && !shopTransitioning)
            {
                StartCoroutine(ExitShopCoroutine());
            }
        }
    }

    #region Dialogue
    void StartDialogue()
    {
        if (dialogueLines == null || dialogueLines.Length == 0)
            return;

        isTalking = true;
        currentLine = 0;
        ShowCurrentLine();
    }

    void NextLine()
    {
        currentLine++;

        if (dialogueLines != null && currentLine < dialogueLines.Length)
            ShowCurrentLine();
        else
            EndDialogue();
    }

    void ShowCurrentLine()
    {
        if (NPCmanager.Instance != null &&
            dialogueLines != null &&
            currentLine < dialogueLines.Length)
        {
            NPCmanager.Instance.ShowDialogue(dialogueLines[currentLine]);
        }

        if (voiceSource != null)
        {
            voiceSource.Stop();

            if (voiceLines != null &&
                currentLine < voiceLines.Length &&
                voiceLines[currentLine] != null)
            {
                voiceSource.clip = voiceLines[currentLine];
                voiceSource.Play();
            }
        }
    }

    void EndDialogue()
    {
        isTalking = false;

        if (voiceSource != null)
            voiceSource.Stop();

        if (NPCmanager.Instance != null)
            NPCmanager.Instance.HideDialogue();

        if (!shopTransitioning)
        {
            shopTransitioning = true;
            StartCoroutine(EnterShopCoroutine());
        }
    }
    #endregion

    #region Shop Transition
    IEnumerator EnterShopCoroutine()
    {
        if (playerController != null)
            playerController.inShopMode = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        inShop = true;

        if (shopCamera != null)
            shopCamera.enabled = true;

        if (playerCamera != null)
            playerCamera.enabled = false;

        yield return null;

        shopTransitioning = false;
    }

    IEnumerator ExitShopCoroutine()
    {
        shopTransitioning = true;

        if (playerController != null)
            playerController.inShopMode = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inShop = false;

        if (shopCamera != null)
            shopCamera.enabled = false;

        if (playerCamera != null)
            playerCamera.enabled = true;

        yield return null;

        shopTransitioning = false;
    }
    #endregion

    #region Trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (isTalking)
                EndDialogue();
        }
    }
    #endregion
}