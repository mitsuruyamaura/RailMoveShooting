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

    [SerializeField, Header("�J�����̈ړ��^�C�v(Linear �� Catmull Rom ��ݒ�)")]
    private PathType pathType;

    [SerializeField]
    private DollyCamera dollyCamera;

    [SerializeField]
    private CameraSwitcher cameraSwitcher;

    private Tween tween;

    private GameManager gameManager;

    private int moveCount;

    //void Start() {
    //    // Debug �p  ���[���ړ��̊J�n
    //    StartCoroutine(StartRailMove());
    //}

    /// <summary>
    /// RailMoveController �̏����ݒ�
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpRailMoveController(GameManager gameManager) {
        this.gameManager = gameManager;

        // TODO ���ɂ�����ꍇ�ɂ͒ǋL�B�K�v�ɉ����Ĉ�����ʂ��ĊO������������炤�悤�ɂ���

    }

    /// <summary>
    /// ���ɍĐ����郌�[���ړ��̖ړI�n�ƌo�H�̃p�X���擾���Đݒ�
    /// </summary>
    /// <param name="nextPathDataList"></param>
    public void SetNextRailPathData(RailPathData nextRailPathData) {
        // �ړI�n�擾
        currentRailPathData = nextRailPathData;

        // Cinemachine �� TrackedDolly ���g�p����ꍇ
        if (GameData.instance.useCinemachine && dollyCamera != null) {

            // Path ���Z�b�g
            dollyCamera.SetPath(currentRailPathData.smoothPath);

        } else {

            // �ړ��J�n
            StartCoroutine(StartRailMove());
        }
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
        tween = railMoveTarget.transform.DOPath(paths, totalTime, pathType).SetEase(Ease.Linear).OnWaypointChange((waypointIndex) => CheckArrivalDestination(waypointIndex));
        Debug.Log("�ړ��J�n");

        // TODO ���ɕK�v�ȏ�����ǋL

        // �ړ����ꎞ��~
        PauseMove();

        // �Q�[���̐i�s��Ԃ��ړ����ɂȂ�܂őҋ@
        yield return new WaitUntil(() => gameManager.currentGameState == GameState.Play_Move);

        // �ړ��J�n
        ResumeMove();
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

    /// <summary>
    /// �p�X�̖ڕW�n�_�ɓ������邽�тɎ��s�����
    /// </summary>
    /// <param name="waypointIndex"></param>
    private void CheckArrivalDestination(int waypointIndex) {

        Debug.Log("�ڕW�n�_ ���� : " + waypointIndex + " �Ԗ�");

        // �J�����̉�]
        railMoveTarget.transform.DORotate(currentRailPathData.pathDataDetails[waypointIndex].pathTran.eulerAngles, currentRailPathData.pathDataDetails[waypointIndex].railMoveDuration).SetEase(Ease.Linear);
        Debug.Log(currentRailPathData.pathDataDetails[waypointIndex].pathTran.eulerAngles);

        // �ړ��̈ꎞ��~
        PauseMove();

        // �ړ���̃p�X���܂��c���Ă��邩�m�F
        if (waypointIndex < currentRailPathData.GetPathTrans().Length) {
            // �~�b�V�������������邩�Q�[���}�l�[�W���[���Ŋm�F
            gameManager.CheckMissionTrigger(waypointIndex++);

            // Debug�p  ���̃p�X�ւ̈ړ��J�n
            //ResumeMove();

            // VirtualCamera �؂�ւ�
            cameraSwitcher.SwitchCamera(waypointIndex);

        } else {
            // DOTween ���~
            tween.Kill();

            tween = null;

            // �ړ��悪�c���Ă��Ȃ��ꍇ�ɂ́A�Q�[���}�l�[�W���[���ŕ���̊m�F(���̃��[�g�I��A�ړ���̕���A�{�X�A�N���A�̂����ꂩ)
            moveCount++;

            gameManager.PreparateCheckNextBranch(moveCount);

            Debug.Log("����m�F");
        }
    }

    /// <summary>
    /// �ړ��p�̏������o�^���ꂽ���m�F
    /// </summary>
    /// <returns></returns>
    public bool GetMoveSetting() {
        return tween != null ? true : false;
    }
}
