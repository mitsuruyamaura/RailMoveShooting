using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDetail : MonoBehaviour
{
    public int timeValue;   // �Q�[�����Ԃ̑����l


    private void OnDestroy() {
        GameTimeManager.instance.CalcGameTime(timeValue);
    }
}