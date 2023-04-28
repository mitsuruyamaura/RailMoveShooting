using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDetail : MonoBehaviour
{
    public int timeValue;   // ƒQ[ƒ€ŠÔ‚Ì‘Œ¸’l


    private void OnDestroy() {
        GameTimeManager.instance.CalcGameTime(timeValue);
    }
}