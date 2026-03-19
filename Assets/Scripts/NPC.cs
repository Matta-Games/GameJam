using UnityEngine;

public class NPCD : MonoBehaviour
{
    [TextArea(2, 5)]
    public string[] dialogueLines;

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
                NextLine();
            }
        }
    }

    void StartDialogue()
    {
        isTalking = true;
        currentLine = 0;

        NPCmanager.Instance.ShowDialogue(dialogueLines[currentLine]);
    }

    void NextLine()
    {
        currentLine++;

        if (currentLine < dialogueLines.Length)
        {
            NPCmanager.Instance.ShowDialogue(dialogueLines[currentLine]);
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        isTalking = false;
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