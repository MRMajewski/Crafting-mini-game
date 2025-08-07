using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

public class QualityManager : MonoBehaviour
{
    [SerializeField] private Volume postProcessingVolume;
    [SerializeField] private Material lowQualityWaterMaterial;
    [SerializeField] private Material highQualityWaterMaterial;
    [SerializeField] private Renderer waterRenderer;

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern int IsMobileDevice();
#endif

    public static bool IsRunningOnMobile()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        try
        {
            return IsMobileDevice() == 1;
        }
        catch
        {
            Debug.LogWarning("IsMobileDevice() call failed.");
            return false;
        }
#else
        return false; 
#endif
    }

    public void SetQuality()
    {
        bool isMobile;

#if UNITY_WEBGL
        isMobile = IsRunningOnMobile();
#else
        isMobile = Application.isMobilePlatform;
#endif

        if (isMobile)
        {
            QualitySettings.SetQualityLevel(2, true);
            postProcessingVolume.enabled = false;
            if (waterRenderer != null)
                waterRenderer.material = lowQualityWaterMaterial;

            Debug.Log("Mobilne urz¹dzenie – ustawiono nisk¹ jakoœæ");
        }
        else
        {
            QualitySettings.SetQualityLevel(5, true);
            postProcessingVolume.enabled = true;
            if (waterRenderer != null)
                waterRenderer.material = highQualityWaterMaterial;

            Debug.Log("Desktop lub WebGL na PC – ustawiono wysok¹ jakoœæ");
        }
    }
}
