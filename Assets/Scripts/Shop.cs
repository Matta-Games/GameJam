using UnityEngine;
using System.Collections;

public class ShopController : MonoBehaviour
{
    [Header("Cameras")]
    public Camera playerCamera;
    public Camera shopCamera;

    [Header("Transition")]
    public float transitionSpeed = 2f;

    public bool inShop = false;

    void Start()
    {
        if (shopCamera != null)
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
        StartCoroutine(TransitionToShop());
    }

    void ExitShop()
    {
        StartCoroutine(TransitionBack());
    }

    IEnumerator TransitionToShop()
    {
        inShop = true;

        if (shopCamera != null)
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

        if (shopCamera != null)
            shopCamera.gameObject.SetActive(false);
    }
}