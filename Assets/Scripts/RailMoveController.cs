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

    private GameManager gameManager;


    /// <summary>
    /// 次に再生するレール移動の目的地と経路のパスを取得して設定
    /// </summary>
    /// <param name="nextPathDataList"></param>
    public void SetNextRailPathData(RailPathData nextRailPathData) {
        // 目的地取得
        currentRailPathData = nextRailPathData; ;

        // 移動開始
        StartCoroutine(StartRailMove());
    }


    //void Start() {
    //    // Debug 用  レール移動の開始
    //    StartCoroutine(StartRailMove());
    //}

    /// <summary>
    /// レール移動の開始
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartRailMove() {

        yield return null;

        // 移動先のパスの情報から Position の情報だけを抽出して配列を作成
        Vector3[] paths = currentRailPathData.GetPathTrans().Select(x => x.position).ToArray();

        // 移動先のパスの移動時間を合計
        float totalTime = currentRailPathData.GetRailMoveDurations().Sum();
        Debug.Log(totalTime);

        // パスによる移動開始
        tween = railMoveTarget.transform.DOPath(paths, totalTime).SetEase(Ease.Linear).OnWaypointChange((waypointIndex) => CheckArrivalDestination(waypointIndex));
        Debug.Log("移動開始");

        // TODO 他に必要な処理を追記

    }

    /// <summary>
    /// レール移動の一時停止
    /// </summary>
    public void PauseMove() {
        // 一時停止
        transform.DOPlay();
        tween.Pause();
    }

    /// <summary>
    /// レール移動の再開
    /// </summary>
    public void ResumeMove() {
        // 移動再開
        transform.DOPause();
        tween.Play();
    }

    /// <summary>
    /// パスの目標地点に到着するたびに実行される
    /// </summary>
    /// <param name="waypointIndex"></param>
    private void CheckArrivalDestination(int waypointIndex) {

        Debug.Log("目標地点 到着 : " + waypointIndex + " 番目");

        // 移動の一時停止
        PauseMove();

        // 移動先のパスがまだ残っているか確認
        if (waypointIndex < currentRailPathData.GetPathTrans().Length) {
            // ミッションが発生するかゲームマネージャー側で確認
            gameManager.CheckMissionTrigger(waypointIndex++);

            // Debug用  次のパスへの移動開始
            //ResumeMove();

        } else {
            // DOTween を停止
            tween.Kill();

            // 移動先が残っていない場合には、ゲームマネージャー側で分岐の確認(次のルート選定、移動先の分岐、ボス、クリアのいずれか)

            Debug.Log("分岐確認");
        }
    }

    /// <summary>
    /// RailMoveController の初期設定
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpRailMoveController(GameManager gameManager) {
        this.gameManager = gameManager;

        // TODO 他にもある場合には追記。必要に応じて引数を通じて外部から情報をもらうようにする

    }
}
