using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidInit : MonoBehaviour
{
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
    }
}
