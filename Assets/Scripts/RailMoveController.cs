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

    [SerializeField, Header("カメラの移動タイプ(Linear か Catmull Rom を設定)")]
    private PathType pathType;

    [SerializeField]
    private DollyCamera dollyCamera;

    [SerializeField]
    private CameraSwitcher cameraSwitcher;

    private Tween tweenMove;
    private Tween tweenRotation;

    private GameManager gameManager;

    private int moveCount;

    // 以下の３つはパスごとの移動時に利用する
    private Vector3[] paths;
    private float[] moveDurations;
    private int pathCount;


    //void Start() {
    //    // Debug 用  レール移動の開始
    //    StartCoroutine(StartRailMove());
    //}

    /// <summary>
    /// RailMoveController の初期設定
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpRailMoveController(GameManager gameManager) {
        this.gameManager = gameManager;

        // TODO 他にもある場合には追記。必要に応じて引数を通じて外部から情報をもらうようにする

    }

    /// <summary>
    /// 次に再生するレール移動の目的地と経路のパスを取得して設定
    /// </summary>
    /// <param name="nextPathDataList"></param>
    public void SetNextRailPathData(RailPathData nextRailPathData) {
        // 目的地取得
        currentRailPathData = nextRailPathData;

        // Cinemachine の TrackedDolly を使用する場合
        if (GameData.instance.useCinemachine && dollyCamera != null) {

            // Path をセット
            dollyCamera.SetPath(currentRailPathData.smoothPath);

        } else {

            // Virtual Camera をオフ(MainCamera を有効にする)
            cameraSwitcher.gameObject.SetActive(false);

            // 移動開始
            StartCoroutine(StartRailMove());
        }
    }

    /// <summary>
    /// レール移動の開始
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartRailMove() {

        yield return null;

        // パスのカウントを初期化
        pathCount = 0;

        // 移動先のパスの情報から Position の情報だけを抽出して配列を作成
        //Vector3[] paths = currentRailPathData.GetPathTrans().Select(x => x.position).ToArray();
        paths = currentRailPathData.GetPathTrans().Select(x => x.position).ToArray();

        // 移動先のパスの移動時間を合計
        float totalTime = currentRailPathData.GetRailMoveDurations().Sum();
        moveDurations = currentRailPathData.GetRailMoveDurations();

        //Debug.Log(totalTime);

        // TODO パスによる移動開始(すべてのパスを指定して、まとめて動かす場合)
        //tweenMove = railMoveTarget.transform.DOPath(paths, totalTime, pathType).SetEase(Ease.Linear).OnWaypointChange((waypointIndex) => CheckArrivalDestination(waypointIndex));

        RailMove();

        // TODO 他に必要な処理を追記

        // 移動を一時停止
        PauseMove();

        // ゲームの進行状態が移動中になるまで待機
        yield return new WaitUntil(() => gameManager.currentGameState == GameState.Play_Move);

        // 移動開始
        ResumeMove();

        Debug.Log("移動開始");
    }

    /// <summary>
    /// パスのカウントアップ(パスごとに動かす場合)
    /// </summary>
    public void CountUp() {
        pathCount++;
        Debug.Log(pathCount);

        RailMove();
    }

    /// <summary>
    /// 2点間のパスの目標地点を設定して移動
    /// </summary>
    public void RailMove() {

        // 残っているパスがない場合
        if (pathCount >= currentRailPathData.GetPathTrans().Length) {
            // DOTween を停止
            //tweenMove.Kill();

            //tweenMove = null;
            //tweenRotation = null;

            // 移動先が残っていない場合には、ゲームマネージャー側で分岐の確認(次のルート選定、移動先の分岐、ボス、クリアのいずれか)
            moveCount++;

            gameManager.PreparateCheckNextBranch(moveCount);

            Debug.Log("分岐確認");

            return;
        }

        Vector3[] targetPaths;

        if (pathCount == 0) {
            targetPaths = new Vector3[2] { railMoveTarget.position, paths[pathCount] };
        } else {
            targetPaths = new Vector3[2] { paths[pathCount -1], paths[pathCount] };            
        }
        float duration = moveDurations[pathCount];

        //Debug.Log("スタート地点 :" + targetPaths[0]);
        //Debug.Log("目標地点 :" + targetPaths[1]);
        //Debug.Log("移動にかかる時間 :" + duration);

        tweenMove = railMoveTarget.transform.DOPath(targetPaths, duration, pathType).SetEase(Ease.Linear).OnWaypointChange((waypointIndex) => CheckArrivalDestination(waypointIndex));

        tweenRotation = railMoveTarget.transform.DORotate(currentRailPathData.pathDataDetails[pathCount].pathTran.eulerAngles, duration).SetEase(Ease.Linear);
        //Debug.Log($" 回転角度 :  { currentRailPathData.pathDataDetails[pathCount].pathTran.eulerAngles } ");
    }

    /// <summary>
    /// レール移動の一時停止
    /// </summary>
    public void PauseMove() {
        // 一時停止
        //transform.DOPause();
        tweenMove.Pause();
        tweenRotation.Pause();
    }

    /// <summary>
    /// レール移動の再開
    /// </summary>
    public void ResumeMove() {
        // 移動再開
        //transform.DOPlay();
        tweenMove.Play();
        tweenRotation.Play();
    }

    /// <summary>
    /// パスの目標地点に到着するたびに実行される
    /// </summary>
    /// <param name="waypointIndex"></param>
    private void CheckArrivalDestination(int waypointIndex) {

        if (waypointIndex == 0) {
            return;
        }

        //Debug.Log("目標地点 到着 : " + waypointIndex + " 番目");
        Debug.Log("目標地点 到着 : " + pathCount + " 番目");

        // TODO カメラの回転(まとめて動かす場合)
        //railMoveTarget.transform.DORotate(currentRailPathData.pathDataDetails[waypointIndex].pathTran.eulerAngles, currentRailPathData.pathDataDetails[waypointIndex].railMoveDuration).SetEase(Ease.Linear);
        //Debug.Log(currentRailPathData.pathDataDetails[waypointIndex].pathTran.eulerAngles);

        // 移動の一時停止
        //PauseMove();

        // パスごとの移動のデバッグ用
        //CountUp();

        // DOTween を停止
        tweenMove.Kill();
        tweenRotation.Kill();

        tweenMove = null;
        tweenRotation = null;


        // TOOD ムービーの確認
        gameManager.CheckMoviePlay(pathCount);

        // パスごとに動かす場合
        //gameManager.CheckMissionTrigger(pathCount);


        // TODO まとめて動かす場合には、下記をすべて使う

        //// 移動先のパスがまだ残っているか確認
        //if (waypointIndex < currentRailPathData.GetPathTrans().Length) {(まとめて動かす場合の条件式)

        //    // ミッションが発生するかゲームマネージャー側で確認(まとめて動かす場合の条件式)
        //    //gameManager.CheckMissionTrigger(waypointIndex++);

        //    // Debug用  次のパスへの移動開始
        //    //ResumeMove();

        //    // VirtualCamera 切り替え
        //    //cameraSwitcher.SwitchCamera(waypointIndex);

        //    // パスごとに動かす場合
        //    gameManager.CheckMissionTrigger(pathCount);

        //} else {
        //    // DOTween を停止
        //    tweenMove.Kill();

        //    tweenMove = null;

        //    // 移動先が残っていない場合には、ゲームマネージャー側で分岐の確認(次のルート選定、移動先の分岐、ボス、クリアのいずれか)
        //    moveCount++;

        //    gameManager.PreparateCheckNextBranch(moveCount);

        //    Debug.Log("分岐確認");
        //}
    }

    /// <summary>
    /// 移動用の処理が登録されたか確認
    /// </summary>
    /// <returns></returns>
    public bool GetMoveSetting() {
        return tweenMove != null ? true : false;
    }
}
