using UnityEngine;

public class F1Manager : MonoBehaviour
{
    public GameObject menuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            menuUI.SetActive(!menuUI.activeSelf);
        }
    }
}
