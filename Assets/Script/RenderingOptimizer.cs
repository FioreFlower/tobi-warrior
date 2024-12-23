using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderingOptimizer : MonoBehaviour
{
    [Header("Optimization Settings")]
    public int targetFrameRate = 60;
    public bool useVSync = true;

    void Start()
    {
        Application.targetFrameRate = targetFrameRate;
        QualitySettings.vSyncCount = useVSync ? 1 : 0;

        // 배치 최적화
        StaticBatchingUtility.Combine(gameObject);
    }
}
