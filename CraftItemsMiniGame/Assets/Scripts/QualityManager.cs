using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class QualityManager : MonoBehaviour
{
    [SerializeField] private Volume postProcessingVolume;
    [SerializeField] private Material lowQualityWaterMaterial;
    [SerializeField] private Material highQualityWaterMaterial;
    [SerializeField] private Renderer waterRenderer;

    public void SetQuality()
    {
#if UNITY_WEBGL
        if (IsPCBuild())
        {
            QualitySettings.SetQualityLevel(3, true);
            Debug.Log("WebGL na komputerze – ustawiono œredni¹ jakoœæ");

            if (waterRenderer != null)
            {
                waterRenderer.material = lowQualityWaterMaterial; 
            }
        }
        else
        {
            QualitySettings.SetQualityLevel(0, true);
            Debug.Log("WebGL na telefonie – ustawiono nisk¹ jakoœæ");
        }
#else
        if (Application.isMobilePlatform)
        {
            // Mobilny build (np. Android/iOS natywnie)
            QualitySettings.SetQualityLevel(5, true);
            Debug.Log("Natywny mobilny build – ustawiono nisk¹ jakoœæ");

            if (waterRenderer != null)
            {
                waterRenderer.material = lowQualityWaterMaterial; 
            }
        }
        else
        {
            // Natywny build PC
            QualitySettings.SetQualityLevel(5, true);
            Debug.Log("Natywny build PC – ustawiono wysok¹ jakoœæ");

            if (waterRenderer != null)
            {
                waterRenderer.material = highQualityWaterMaterial; 
            }
        }
#endif
    }
}
   
