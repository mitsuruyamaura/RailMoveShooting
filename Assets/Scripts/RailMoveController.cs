using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class RailMoveController : MonoBehaviour
{
    [SerializeField]
    private Transform railMoveTarget;

    [SerializeField]
    private RailPathData currentRailPathData;

    private Tween tween;


    ///// <summary>
    ///// ���ɍĐ����郌�[���ړ��̖ړI�n���擾���Đݒ�
    ///// </summary>
    ///// <param name="nextPathDataList"></param>
    //public void SetNextPathData(List<PathData> nextPathDataList) {
    //    // �ړI�n�擾
    //    pathDatasList = new List<PathData>(nextPathDataList);

    //    // �ړ��J�n
    //    StartCoroutine(StartRailMove());
    //}


    void Start() {
        // Debug �p  ���[���ړ��̊J�n
        StartCoroutine(StartRailMove());
    }

    /// <summary>
    /// ���[���ړ��̊J�n
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartRailMove() {

        yield return null;

        // �ړ���̃p�X�̏�񂩂� Position �̏�񂾂��𒊏o���Ĕz����쐬
        Vector3[] paths = currentRailPathData.GetPathTrans().Select(x => x.position).ToArray();

        // �ړ���̃p�X�̈ړ����Ԃ����v
        float totalTime = currentRailPathData.GetRailMoveDurations().Sum();
        Debug.Log(totalTime);

        // �p�X�ɂ��ړ��J�n
        tween = railMoveTarget.transform.DOPath(paths, totalTime).SetEase(Ease.Linear).OnWaypointChange((waypointIndex) => CheckArrivalDestination(waypointIndex));
        Debug.Log("�ړ��J�n");

        // TODO ���ɕK�v�ȏ�����ǋL

    }

    /// <summary>
    /// ���[���ړ��̈ꎞ��~
    /// </summary>
    public void PauseMove() {
        // �ꎞ��~
        transform.DOPlay();
        tween.Pause();
    }

    /// <summary>
    /// ���[���ړ��̍ĊJ
    /// </summary>
    public void ResumeMove() {
        // �ړ��ĊJ
        transform.DOPause();
        tween.Play();
    }

    private void CheckArrivalDestination(int waypointIndex) {

        Debug.Log("�ڕW�n�_ ���� : " + waypointIndex + " �Ԗ�");

        // �ړ��̈ꎞ��~
        PauseMove();

        // �ړ���̃p�X���܂��c���Ă��邩�m�F
        if (waypointIndex < currentRailPathData.GetPathTrans().Length) {
            // �~�b�V�������������邩�Q�[���}�l�[�W���[���Ŋm�F

            // Debug�p  ���̃p�X�ւ̈ړ��J�n
            ResumeMove();

        } else {
            // DOTween ���~
            tween.Kill();

            // �ړ��悪�c���Ă��Ȃ��ꍇ�ɂ́A�Q�[���}�l�[�W���[���ŕ���̊m�F(���̃��[�g�I��A�ړ���̕���A�{�X�A�N���A�̂����ꂩ)

            Debug.Log("����m�F");
        }
    }
}
