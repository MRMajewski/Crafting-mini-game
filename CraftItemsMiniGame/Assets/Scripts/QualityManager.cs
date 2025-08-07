using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class QualityManager : MonoBehaviour
{
    [SerializeField] private Volume postProcessingVolume;
    public void SetQuality()
    {
#if UNITY_WEBGL
        if (IsMobile())
        {
            QualitySettings.SetQualityLevel(0, true);
            Debug.Log("WebGL na telefonie – ustawiono nisk¹ jakoœæ");
        }
        else
        {
            QualitySettings.SetQualityLevel(3, true);

        }
#else
        if (Application.isMobilePlatform)
        {
            // Mobilny build (np. Android/iOS natywnie)
            QualitySettings.SetQualityLevel(5, true);
            Debug.Log("Natywny mobilny build – ustawiono nisk¹ jakoœæ");
        }
        else
        {
            // Natywny build PC
            QualitySettings.SetQualityLevel(5, true);
            Debug.Log("Natywny build PC – ustawiono wysok¹ jakoœæ");
        }
#endif

        if (QualitySettings.GetQualityLevel() <= 1)
        {
            postProcessingVolume.enabled = false;
        }

        bool IsMobile()
        {
            string ua = Application.absoluteURL;
            return Application.isMobilePlatform ||
                   (ua.Contains("Android") || ua.Contains("iPhone") || ua.Contains("iPad"));
        }
    }
}
