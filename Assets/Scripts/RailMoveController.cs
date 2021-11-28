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

    private Tween tweenMove;
    private Tween tweenRotation;

    private GameManager gameManager;

    private int moveCount;

    // �ȉ��̂R�̓p�X���Ƃ̈ړ����ɗ��p����
    private Vector3[] paths;
    private float[] moveDurations;
    private int pathCount;


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

            // Virtual Camera ���I�t(MainCamera ��L���ɂ���)
            cameraSwitcher.gameObject.SetActive(false);

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

        // �p�X�̃J�E���g��������
        pathCount = 0;

        // �ړ���̃p�X�̏�񂩂� Position �̏�񂾂��𒊏o���Ĕz����쐬
        //Vector3[] paths = currentRailPathData.GetPathTrans().Select(x => x.position).ToArray();
        paths = currentRailPathData.GetPathTrans().Select(x => x.position).ToArray();

        // �ړ���̃p�X�̈ړ����Ԃ����v
        float totalTime = currentRailPathData.GetRailMoveDurations().Sum();
        moveDurations = currentRailPathData.GetRailMoveDurations();

        //Debug.Log(totalTime);

        // TODO �p�X�ɂ��ړ��J�n(���ׂẴp�X���w�肵�āA�܂Ƃ߂ē������ꍇ)
        //tweenMove = railMoveTarget.transform.DOPath(paths, totalTime, pathType).SetEase(Ease.Linear).OnWaypointChange((waypointIndex) => CheckArrivalDestination(waypointIndex));

        RailMove();

        // TODO ���ɕK�v�ȏ�����ǋL

        // �ړ����ꎞ��~
        PauseMove();

        // �Q�[���̐i�s��Ԃ��ړ����ɂȂ�܂őҋ@
        yield return new WaitUntil(() => gameManager.currentGameState == GameState.Play_Move);

        // �ړ��J�n
        ResumeMove();

        Debug.Log("�ړ��J�n");
    }

    /// <summary>
    /// �p�X�̃J�E���g�A�b�v(�p�X���Ƃɓ������ꍇ)
    /// </summary>
    public void CountUp() {
        pathCount++;
        Debug.Log(pathCount);

        RailMove();
    }

    /// <summary>
    /// 2�_�Ԃ̃p�X�̖ڕW�n�_��ݒ肵�Ĉړ�
    /// </summary>
    public void RailMove() {

        // �c���Ă���p�X���Ȃ��ꍇ
        if (pathCount >= currentRailPathData.GetPathTrans().Length) {
            // DOTween ���~
            //tweenMove.Kill();

            //tweenMove = null;
            //tweenRotation = null;

            // �ړ��悪�c���Ă��Ȃ��ꍇ�ɂ́A�Q�[���}�l�[�W���[���ŕ���̊m�F(���̃��[�g�I��A�ړ���̕���A�{�X�A�N���A�̂����ꂩ)
            moveCount++;

            gameManager.PreparateCheckNextBranch(moveCount);

            Debug.Log("����m�F");

            return;
        }

        Vector3[] targetPaths;

        if (pathCount == 0) {
            targetPaths = new Vector3[2] { railMoveTarget.position, paths[pathCount] };
        } else {
            targetPaths = new Vector3[2] { paths[pathCount -1], paths[pathCount] };            
        }
        float duration = moveDurations[pathCount];

        //Debug.Log("�X�^�[�g�n�_ :" + targetPaths[0]);
        //Debug.Log("�ڕW�n�_ :" + targetPaths[1]);
        //Debug.Log("�ړ��ɂ����鎞�� :" + duration);

        tweenMove = railMoveTarget.transform.DOPath(targetPaths, duration, pathType).SetEase(Ease.Linear).OnWaypointChange((waypointIndex) => CheckArrivalDestination(waypointIndex));

        tweenRotation = railMoveTarget.transform.DORotate(currentRailPathData.pathDataDetails[pathCount].pathTran.eulerAngles, duration).SetEase(Ease.Linear);
        //Debug.Log($" ��]�p�x :  { currentRailPathData.pathDataDetails[pathCount].pathTran.eulerAngles } ");
    }

    /// <summary>
    /// ���[���ړ��̈ꎞ��~
    /// </summary>
    public void PauseMove() {
        // �ꎞ��~
        //transform.DOPause();
        tweenMove.Pause();
        tweenRotation.Pause();
    }

    /// <summary>
    /// ���[���ړ��̍ĊJ
    /// </summary>
    public void ResumeMove() {
        // �ړ��ĊJ
        //transform.DOPlay();
        tweenMove.Play();
        tweenRotation.Play();
    }

    /// <summary>
    /// �p�X�̖ڕW�n�_�ɓ������邽�тɎ��s�����
    /// </summary>
    /// <param name="waypointIndex"></param>
    private void CheckArrivalDestination(int waypointIndex) {

        if (waypointIndex == 0) {
            return;
        }

        //Debug.Log("�ڕW�n�_ ���� : " + waypointIndex + " �Ԗ�");
        Debug.Log("�ڕW�n�_ ���� : " + pathCount + " �Ԗ�");

        // TODO �J�����̉�](�܂Ƃ߂ē������ꍇ)
        //railMoveTarget.transform.DORotate(currentRailPathData.pathDataDetails[waypointIndex].pathTran.eulerAngles, currentRailPathData.pathDataDetails[waypointIndex].railMoveDuration).SetEase(Ease.Linear);
        //Debug.Log(currentRailPathData.pathDataDetails[waypointIndex].pathTran.eulerAngles);

        // �ړ��̈ꎞ��~
        //PauseMove();

        // �p�X���Ƃ̈ړ��̃f�o�b�O�p
        //CountUp();

        // DOTween ���~
        tweenMove.Kill();
        tweenRotation.Kill();

        tweenMove = null;
        tweenRotation = null;


        // TOOD ���[�r�[�̊m�F
        gameManager.CheckMoviePlay(pathCount);

        // �p�X���Ƃɓ������ꍇ
        //gameManager.CheckMissionTrigger(pathCount);


        // TODO �܂Ƃ߂ē������ꍇ�ɂ́A���L�����ׂĎg��

        //// �ړ���̃p�X���܂��c���Ă��邩�m�F
        //if (waypointIndex < currentRailPathData.GetPathTrans().Length) {(�܂Ƃ߂ē������ꍇ�̏�����)

        //    // �~�b�V�������������邩�Q�[���}�l�[�W���[���Ŋm�F(�܂Ƃ߂ē������ꍇ�̏�����)
        //    //gameManager.CheckMissionTrigger(waypointIndex++);

        //    // Debug�p  ���̃p�X�ւ̈ړ��J�n
        //    //ResumeMove();

        //    // VirtualCamera �؂�ւ�
        //    //cameraSwitcher.SwitchCamera(waypointIndex);

        //    // �p�X���Ƃɓ������ꍇ
        //    gameManager.CheckMissionTrigger(pathCount);

        //} else {
        //    // DOTween ���~
        //    tweenMove.Kill();

        //    tweenMove = null;

        //    // �ړ��悪�c���Ă��Ȃ��ꍇ�ɂ́A�Q�[���}�l�[�W���[���ŕ���̊m�F(���̃��[�g�I��A�ړ���̕���A�{�X�A�N���A�̂����ꂩ)
        //    moveCount++;

        //    gameManager.PreparateCheckNextBranch(moveCount);

        //    Debug.Log("����m�F");
        //}
    }

    /// <summary>
    /// �ړ��p�̏������o�^���ꂽ���m�F
    /// </summary>
    /// <returns></returns>
    public bool GetMoveSetting() {
        return tweenMove != null ? true : false;
    }
}
