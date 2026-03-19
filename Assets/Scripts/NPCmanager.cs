using UnityEngine;
using TMPro;

public class NPCmanager : MonoBehaviour
{
    public static NPCmanager Instance;

    public GameObject dialogueUI;
    public TMP_Text dialogueText;

    private void Awake()
    {
        Instance = this;
        dialogueUI.SetActive(false);
    }

    public void ShowDialogue(string line)
    {
        dialogueUI.SetActive(true);
        dialogueText.text = line;
    }

    public void HideDialogue()
    {
        dialogueUI.SetActive(false);
    }
}