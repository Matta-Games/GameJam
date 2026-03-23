using UnityEngine;

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

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (!isTalking)
            {
                StartDialogue();
            }
            else
            {
                NextLine(); // skip current and go next
            }
        }
    }

    void StartDialogue()
    {
        isTalking = true;
        currentLine = 0;
        ShowCurrentLine();
    }

    void NextLine()
    {
        currentLine++;

        if (currentLine < dialogueLines.Length)
        {
            ShowCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    void ShowCurrentLine()
    {
        NPCmanager.Instance.ShowDialogue(dialogueLines[currentLine]);

        // stop previous audio if playing
        if (voiceSource != null)
            voiceSource.Stop();

        // play matching voice line
        if (voiceSource != null &&
            voiceLines != null &&
            currentLine < voiceLines.Length &&
            voiceLines[currentLine] != null)
        {
            voiceSource.clip = voiceLines[currentLine];
            voiceSource.Play();
        }
    }

    void EndDialogue()
    {
        isTalking = false;

        if (voiceSource != null)
            voiceSource.Stop();

        NPCmanager.Instance.HideDialogue();
    }

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
            EndDialogue();
        }
    }
}