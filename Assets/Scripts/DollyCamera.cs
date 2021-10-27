using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// 参考サイト
// https://light11.hatenadiary.com/entry/2019/07/16/210124

/// <summary>
/// VirtualCamera の状態
/// </summary>
public enum CameraStatus {
    Play,
    Stop
}

/// <summary>
/// Dolly 移動用のカメラの制御
/// </summary>
public class DollyCamera : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField, Header("カメラの移動速度")]
    private float movePosSpeed;

    private CinemachineTrackedDolly dolly;

    public CameraStatus currentCameraStatus;


    void Start()
    {
        currentCameraStatus = CameraStatus.Stop;

        // Virtual Camera に対して GetCinemachineComponent で CinemachineTrackedDolly を取得
        // GetComponent ではなくて、GetCinemachineComponent を使うので注意
        dolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    void Update()
    {
        if (currentCameraStatus == CameraStatus.Play) {

            // パスの位置を更新する
            dolly.m_PathPosition += movePosSpeed;

            if (dolly.m_Path.PathLength <= dolly.m_PathPosition) {
                currentCameraStatus = CameraStatus.Stop;
            }
        }
    }

    /// <summary>
    /// パスをセット
    /// </summary>
    /// <param name="nextPath"></param>
    public void SetPath(CinemachinePathBase nextPath) {
        dolly.m_Path = nextPath;

        //Debug.Log(dolly.m_Path.PathLength);   // インスペクターで見れる。Waypoints の数と位置によって自動的に設定

        currentCameraStatus = CameraStatus.Play;
    }
}
