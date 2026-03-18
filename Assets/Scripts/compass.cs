using UnityEngine;

public class compass : MonoBehaviour
{
    public RectTransform compassBarTransform;
    public RectTransform objectiveMarkerTransform;
    public RectTransform northMarkerTransform;
    public RectTransform southMarkerTransform;
    public RectTransform eastMarkerTransform;
    public RectTransform westMarkerTransform;

    public Transform cameraObjectTransform;
    public Transform objectiveObjectTransform;

    void Update()
    {
        if (!cameraObjectTransform) return;
        if (!objectiveObjectTransform) return;

        SetMarkerPosition(objectiveMarkerTransform, objectiveObjectTransform.position);

        SetMarkerPosition(northMarkerTransform, cameraObjectTransform.position + Vector3.forward * 1000);
        SetMarkerPosition(southMarkerTransform, cameraObjectTransform.position + Vector3.back * 1000);
        SetMarkerPosition(eastMarkerTransform, cameraObjectTransform.position + Vector3.right * 1000);
        SetMarkerPosition(westMarkerTransform, cameraObjectTransform.position + Vector3.left * 1000);
    }

    private void SetMarkerPosition(RectTransform markerTransform, Vector3 worldPosition)
    {
        if (!markerTransform) return;

        Vector3 directionToTarget = worldPosition - cameraObjectTransform.position;

        Vector3 flatForward = new Vector3(cameraObjectTransform.forward.x, 0, cameraObjectTransform.forward.z);
        Vector3 flatDirection = new Vector3(directionToTarget.x, 0, directionToTarget.z);

        float signedAngle = Vector3.SignedAngle(flatForward, flatDirection, Vector3.up);

        // Normalize angle (-180 to 180) into -1 to 1
        float normalized = signedAngle / 180f;

        // 🔥 Hide markers when they go too far (prevents overlap)
        if (Mathf.Abs(normalized) > 0.5f)
        {
            markerTransform.gameObject.SetActive(false);
            return;
        }

        markerTransform.gameObject.SetActive(true);

        markerTransform.anchoredPosition = new Vector2(
            compassBarTransform.rect.width * normalized,
            0
        );
    }
}
