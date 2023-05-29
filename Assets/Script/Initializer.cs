using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 初期化する
/// </summary>
public class Initializer : MonoBehaviour
{
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
}
