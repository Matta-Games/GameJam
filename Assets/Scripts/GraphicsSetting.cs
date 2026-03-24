using UnityEngine;

public class GraphicsSettings : MonoBehaviour
{
    public void SetQualityLow() => QualitySettings.SetQualityLevel(0, true);
    public void SetQualityMedium() => QualitySettings.SetQualityLevel(2, true);
    public void SetQualityHigh() => QualitySettings.SetQualityLevel(4, true);

    public void ToggleVSync(bool enabled)
    {
        QualitySettings.vSyncCount = enabled ? 1 : 0;
    }

    public void SetTargetFPS(int fps)
    {
        Application.targetFrameRate = fps;
    }
}