using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class FieldAutoScroller : MonoBehaviour
{
    [SerializeField]
    private List<PathData> pathDatasList = new List<PathData>();

    Tween tween;

    bool isPause;

    [SerializeField, Tooltip("�ꎞ��~�\��")]
    private int stopMotionCount;

    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private GameManager gameManager;

    public Vector3 targetPos;
    public int currentTargetPathCount;


    public IEnumerator StartFieldScroll() {
        yield return null;

        Vector3[] paths = pathDatasList.Select(x => x.pathTran.position).ToArray();
        float totalTime = pathDatasList.Select(x => x.scrollDuration).Sum();

        //paths[0].y = paths[0].y - 5;
        //paths[1].y = paths[1].y - 5;

        Debug.Log(totalTime);
        tween = transform.DOPath(paths, totalTime).SetEase(Ease.Linear).OnWaypointChange((int waypointIndex) => StartCoroutine(gameManager.CheckNextRootBranch()));

        //uiManager.DisplayDebug("�ړ��J�n");

        //uiManager.UpdateDisplayStopMotionCount(stopMotionCount);

        //
        //targetPos = pathDatasList[pathDatasList.Count - 1].pathTran.position;

        //currentTargetPathCount = 1;

        //transform.LookAt(pathDatasList[0].pathTran);

        //StartCoroutine(CheckPath());
    }

    //private IEnumerator CheckPath() {
    //    for (int i = 0; i < pathDatasList.Count; i++) {
    //        targetPos = pathDatasList[i].pathTran.position;
    //        yield return new WaitUntil(() => transform.position == targetPos);            
    //    }
    //}

    //private void Update() {
    //    //if (targetPos.z <= transform.position.z && currentTargetPathCount < pathDatasList.Count - 1) {
    //    //    currentTargetPathCount++;
    //    //    targetPos = pathDatasList[currentTargetPathCount].pathTran.position;
    //    //}

    //    //Vector3 direction = transform.position - pathDatasList[0].pathTran.position;
    //    //direction.y = 0;

    //    //Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
    //    //transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f);

    //    // �ꎞ��~�ƍĊJ����
    //    if (Input.GetKeyDown(KeyCode.Space)) {
    //        StopAndPlayMotion();
    //    }

    //    // �ړ������̊m�F
    //    if (transform.position == targetPos && currentTargetPathCount == 1) {   //  
    //        currentTargetPathCount = 0;

    //        StartCoroutine(gameManager.CheckNextRootBranch());

    //        Debug.Log("����m�F");
    //    }
    //}

    /// <summary>
    /// �t�B�[���h�̈ړ��ƈꎞ��~
    /// </summary>
    public void StopAndPlayMotion() {
        if (isPause) {
            transform.DOPlay();
            gameManager.ResumeMoveAllEnemies();

        } else if (!isPause && stopMotionCount > 0) {
            transform.DOPause();
            gameManager.StopMoveAllEnemies();

            stopMotionCount--;

            // �\���X�V
            uiManager.UpdateDisplayStopMotionCount(stopMotionCount);
        }
        isPause = !isPause;
    }

    /// <summary>
    /// ���ɍĐ�����t�B�[���h��ݒ�
    /// </summary>
    public void SetNextField(List<PathData> nextPathDataList) {
        pathDatasList = new List<PathData>(nextPathDataList);

        StartCoroutine(StartFieldScroll());
    }
}
