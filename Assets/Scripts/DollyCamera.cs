using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// �Q�l�T�C�g
// https://light11.hatenadiary.com/entry/2019/07/16/210124

/// <summary>
/// VirtualCamera �̏��
/// </summary>
public enum CameraStatus {
    Play,
    Stop
}

/// <summary>
/// Dolly �ړ��p�̃J�����̐���
/// </summary>
public class DollyCamera : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField, Header("�J�����̈ړ����x")]
    private float movePosSpeed;

    private CinemachineTrackedDolly dolly;

    public CameraStatus currentCameraStatus;


    void Start()
    {
        currentCameraStatus = CameraStatus.Stop;

        // Virtual Camera �ɑ΂��� GetCinemachineComponent �� CinemachineTrackedDolly ���擾
        // GetComponent �ł͂Ȃ��āAGetCinemachineComponent ���g���̂Œ���
        dolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    void Update()
    {
        if (currentCameraStatus == CameraStatus.Play) {

            // �p�X�̈ʒu���X�V����
            dolly.m_PathPosition += movePosSpeed;

            if (dolly.m_Path.PathLength <= dolly.m_PathPosition) {
                currentCameraStatus = CameraStatus.Stop;
            }
        }
    }

    /// <summary>
    /// �p�X���Z�b�g
    /// </summary>
    /// <param name="nextPath"></param>
    public void SetPath(CinemachinePathBase nextPath) {
        dolly.m_Path = nextPath;

        //Debug.Log(dolly.m_Path.PathLength);   // �C���X�y�N�^�[�Ō����BWaypoints �̐��ƈʒu�ɂ���Ď����I�ɐݒ�

        currentCameraStatus = CameraStatus.Play;
    }
}
