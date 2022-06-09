using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 隠れるアクションの種類
/// </summary>
public enum HideActionType {
    Left,
    Right
}

public class HideAction : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private Transform cameraTran;

    private Vector3 leftHide = new Vector3(-3, -3, 0);
    private Vector3 rightHide = new Vector3(3, -3, 0);
    private float actionTime = 1.0f;

    private Vector3 originCameraPos;
    private Tween tween;

    private bool isHide;  // true で隠れている状態
    public bool IsHide { get => isHide; }


    /// <summary>
    /// デバッグ用
    /// </summary>
    private void Start() {
        SetOriginCameraTran(cameraTran.position);
    }

    void Update()
    {
        // ミッション以外は操作できない
        if (gameManager.currentGameState != GameState.Play_Mission) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {

            Hide(HideActionType.Left);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) {

            Hide(HideActionType.Right);
        }
    }

    /// <summary>
    /// 左右に隠れるアクション
    /// </summary>
    /// <param name="chooseHideType"></param>
    private void Hide(HideActionType chooseHideType) {

        if (!isHide) {
            tween = chooseHideType == HideActionType.Left ? cameraTran.DOMove(leftHide, actionTime) : cameraTran.DOMove(rightHide, actionTime);
        } else {
            tween = cameraTran.DOMove(originCameraPos, actionTime);
        }     
        isHide = !isHide;
    }

    /// <summary>
    /// ミッション開始時のカメラ位置情報を設定
    /// </summary>
    private void SetOriginCameraTran(Vector3 originPos) {
        originCameraPos = originPos;
    }

    /// <summary>
    /// ミッション開始時のカメラ位置情報をリセット
    /// </summary>
    private void ResetOriginCameraTran() {
        originCameraPos = Vector3.zero;
    }
}
