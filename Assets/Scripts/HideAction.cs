using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// �B���A�N�V�����̎��
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

    private bool isHide;  // true �ŉB��Ă�����
    public bool IsHide { get => isHide; }


    /// <summary>
    /// �f�o�b�O�p
    /// </summary>
    private void Start() {
        SetOriginCameraTran(cameraTran.position);
    }

    void Update()
    {
        // �~�b�V�����ȊO�͑���ł��Ȃ�
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
    /// ���E�ɉB���A�N�V����
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
    /// �~�b�V�����J�n���̃J�����ʒu����ݒ�
    /// </summary>
    private void SetOriginCameraTran(Vector3 originPos) {
        originCameraPos = originPos;
    }

    /// <summary>
    /// �~�b�V�����J�n���̃J�����ʒu�������Z�b�g
    /// </summary>
    private void ResetOriginCameraTran() {
        originCameraPos = Vector3.zero;
    }
}
