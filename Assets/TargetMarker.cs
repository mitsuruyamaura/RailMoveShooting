using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMarker : MonoBehaviour
{
    private Transform targetTran;
    private Vector3 offsetTargetMarker = new Vector3(0, 1.5f, 0.2f);
    private float markerScale = 0.05f;

    /// <summary>
    /// �}�[�J�[�ݒ�
    /// </summary>
    /// <param name="target"></param>
    public void SetUpTargetMarker(Transform target) {
        targetTran = target;

        transform.localPosition = offsetTargetMarker;
        transform.localScale = Vector3.one * markerScale;
    }

    void Update()
    {
        if (!targetTran) {
            return;
        }

        // �}�[�J�[����Ƀv���C���[(�J����)�̕����Ɍ�����
        transform.LookAt(targetTran);
    }
}