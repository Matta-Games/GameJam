using UnityEngine;
using System.Collections;

public class ShopController : MonoBehaviour
{
    [Header("Cameras")]
    public Camera playerCamera;
    public Camera shopCamera;

    [Header("Player")]
    public PlayerController playerController;

    [Header("Transition")]
    public float transitionSpeed = 2f;

    private bool inShop = false;

    void Start()
    {
        shopCamera.gameObject.SetActive(false);
    }

    void Update()
    {
        if (inShop && Input.GetKeyDown(KeyCode.F))
        {
            ExitShop();
        }
    }

    public void EnterShop()
    {
        if (playerController != null)
            playerController.inShopMode = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine(TransitionToShop());
    }

    void ExitShop()
    {
        if (playerController != null)
            playerController.inShopMode = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(TransitionBack());
    }

    IEnumerator TransitionToShop()
    {
        inShop = true;

        shopCamera.gameObject.SetActive(true);

        float t = 0f;

        Transform start = playerCamera.transform;
        Transform end = shopCamera.transform;

        Vector3 startPos = start.position;
        Quaternion startRot = start.rotation;

        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;

            playerCamera.transform.position =
                Vector3.Lerp(startPos, end.position, t);

            playerCamera.transform.rotation =
                Quaternion.Slerp(startRot, end.rotation, t);

            yield return null;
        }
    }


    IEnumerator TransitionBack()
    {
        float t = 0f;

        Transform start = playerCamera.transform;
        Transform end = shopCamera.transform;

        Vector3 startPos = start.position;
        Quaternion startRot = start.rotation;

        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;

            playerCamera.transform.position =
                Vector3.Lerp(startPos, end.position, t);

            playerCamera.transform.rotation =
                Quaternion.Slerp(startRot, end.rotation, t);

            yield return null;
        }

        inShop = false;
        shopCamera.gameObject.SetActive(false);
    }
}