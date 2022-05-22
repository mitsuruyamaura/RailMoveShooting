using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Transform lifeTran;

    [SerializeField]
    private GameObject lifePrefab;

    [SerializeField]
    private List<GameObject> lifesList = new List<GameObject>();

    [SerializeField]
    private GameObject playerInfoSet;

    //private int maxLifeIcon;

    [SerializeField]
    private SubmitBranchButton submitBranchButtonPrefab;

    [SerializeField]
    private List<SubmitBranchButton> submitBranchButtonsList = new List<SubmitBranchButton>();

    [SerializeField]
    private Transform rightBranchTran;

    [SerializeField]
    private Transform leftBranchTran;

    [SerializeField]
    private Transform centerBranchTran;

    [SerializeField]
    private bool isSubmitBranch;

    private int submitBranchNo;　　　//　RootData 用の古い変数

    private BranchDirectionType chooseBranchDirectionType;

    [SerializeField]
    private GameObject canvasObj;

    [SerializeField]
    private Button btnWeaponChange;

    [SerializeField]
    private GameObject targetIcon;


    //mi

    [SerializeField]
    private Text txtDebugMessage;

    [SerializeField]
    private Button btnStopMotion;

    [SerializeField]
    private FieldAutoScroller autoScroller;

    [SerializeField]
    private Text txtStopMotionCount;

    [SerializeField]
    private Text txtARIntroduction;

    [SerializeField]
    private Text txtBulletCount;

    private int maxBulletCount;

    [SerializeField]
    private Text txtScore;

    [SerializeField]
    private ResultCanvas resultCanvas;


    /// <summary>
    /// ライフ用アイコンの最大値と弾数の最大値を設定
    /// </summary>
    /// <param name="maxHp"></param>
    public void SetPlayerInfo(int maxHp, int maxBulletCount) {  // TODO 第2引数はまだ

        StartCoroutine(GenerateLife(maxHp));

        //this.maxBulletCount = maxBulletCount;
        //UpdateDisplayBulletCount(this.maxBulletCount);

        SwitchActivateTargetIcon(GameData.instance.isTargetMarker);
    }

    /// <summary>
    /// ライフ用のアイコン(パーティクル)を生成
    /// </summary>
    /// <param name="lifeCount"></param>
    /// <returns></returns>
    public IEnumerator GenerateLife(int lifeCount) {

        for (int i = 0; i < lifeCount; i++) {
            yield return new WaitForSeconds(0.25f);

            lifesList.Add(Instantiate(lifePrefab, lifeTran, false));

        }
    }

    /// <summary>
    /// ライフの再表示
    /// </summary>
    /// <param name="amout"></param>
    public void UpdateDisplayLife(int amout) {

        for (int i = 0; i < lifesList.Count; i++) {

            if (i < amout) {
                lifesList[i].SetActive(true);
            } else {
                lifesList[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// 分岐ボタンの生成
    /// </summary>
    /// <param name="branchDirectionTypes"></param>
    public void GenerateBranchButtons(BranchDirectionType[] branchDirectionTypes) {
        isSubmitBranch = false;
        Debug.Log("分岐のボタン作成");

        // 分岐の数だけボタンを生成
        for (int i = 0; i < branchDirectionTypes.Length; i++) {

            // ボタンの生成位置を設定
            Transform branchTran = BranchDirectionType.Right == branchDirectionTypes[i] ? rightBranchTran : BranchDirectionType.Left == branchDirectionTypes[i] ? leftBranchTran : centerBranchTran;

            // ボタン生成
            SubmitBranchButton submitBranchButton = Instantiate(submitBranchButtonPrefab, branchTran, false);

            // ボタン設定
            submitBranchButton.SetUpSubmitBranchButton(branchDirectionTypes[i], this);

            // List に追加
            submitBranchButtonsList.Add(submitBranchButton);
        }
    }

    /// <summary>
    /// 分岐先の決定
    /// </summary>
    /// <param name="branchDirectionType"></param>
    public void SubmitBranch(BranchDirectionType branchDirectionType) {
        for (int i = 0; i < submitBranchButtonsList.Count; i++) {
            // 分岐のボタンを非活性化して重複タップを防止
            submitBranchButtonsList[i].InactivateSubmitButton();
            Destroy(submitBranchButtonsList[i].gameObject);
        }
        submitBranchButtonsList.Clear();

        chooseBranchDirectionType = branchDirectionType;
        isSubmitBranch = true;
    }

    /// <summary>
    /// 分岐情報の取得
    /// </summary>
    /// <returns></returns>
    public (bool, BranchDirectionType) GetSubmitBranch() {
        return (isSubmitBranch, chooseBranchDirectionType);
    }

    /// <summary>
    /// キャンバスの表示オンオフ切り替え
    /// </summary>
    public void SwitchActivateCanvas(bool isSwitch) {
        canvasObj.SetActive(isSwitch);
    }

    /// <summary>
    /// 武器交換ボタンの取得
    /// </summary>
    /// <returns></returns>
    public Button GetWeaponChangeButton() {
        return btnWeaponChange;
    }


    // mi

    /// <summary>
    /// デバッグ内容を画面表示
    /// </summary>
    /// <param name="message"></param>
    public void DisplayDebug(string message) {
        txtDebugMessage.text = message;
    }

    /// <summary>
    /// ターゲットマーカーのオンオフ切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchActivateTargetIcon(bool isSwitch) {
        targetIcon.SetActive(isSwitch);
    }

    void Update() {

        if (targetIcon != null && targetIcon.activeSelf) {
            // マウスの位置にターゲットマーカーを移動
            targetIcon.transform.position = Input.mousePosition;
        }
    }


    public void SwitchActivatePlayerInfoSet(bool isSwitch) {
        playerInfoSet.SetActive(isSwitch);
    }


    // 本来は使う。いまはデバッグのため未使用

    //void Start() {
    //    btnStopMotion.onClick.AddListener(OnClickStopMotion);

    //    GameData.instance.scoreReactiveProperty.Subscribe(x => UpdateDisplayScore(x));
    //}

    private void UpdateDisplayScore(int scoreValue) {
        txtScore.text = scoreValue.ToString();
    }

    /// <summary>
    /// 一時停止処理の実行
    /// </summary>
    private void OnClickStopMotion() {
        autoScroller.StopAndPlayMotion();
    }

    /// <summary>
    /// 一時停止できる残り回数の表示更新
    /// </summary>
    /// <param name="stopMotionCount"></param>
    public void UpdateDisplayStopMotionCount(int stopMotionCount) {
        txtStopMotionCount.text = stopMotionCount.ToString();
    }

    /// <summary>
    /// AR 導入部分のメッセージ表示
    /// </summary>
    public void DisplayARIntroduction(string message) {
        txtARIntroduction.text = message;
    }

    /// <summary>
    /// AR 導入用のメッセージ表示のオン/オフ切り替え
    /// </summary>
    /// <param name="isSwicth"></param>
    public void InactiveARIntroductionText(bool isSwicth) {
        txtARIntroduction.transform.parent.parent.gameObject.SetActive(isSwicth);
    }

    /// <summary>
    /// 弾数表示の更新
    /// </summary>
    /// <param name="currentBulletCount"></param>
    public void UpdateDisplayBulletCount(int currentBulletCount) {
        txtBulletCount.text = currentBulletCount.ToString() + " / " + maxBulletCount.ToString();
    }


    // 未

    /// <summary>
    /// 分岐のボタン作成(RootData を使う、古い方式)
    /// </summary>
    public IEnumerator GenerateBranchButtons(int[] branchNums, BranchDirectionType[] branchDirectionTypes) {

        isSubmitBranch = false;
        Debug.Log("分岐のボタン作成");

        // 分岐の数だけボタンを生成
        for (int i = 0; i < branchNums.Length; i++) {

            // ボタンの生成位置を設定
            Transform branchTran = BranchDirectionType.Right == branchDirectionTypes[i] ? rightBranchTran : BranchDirectionType.Left == branchDirectionTypes[i] ? leftBranchTran : centerBranchTran;

            // ボタン生成
            SubmitBranchButton submitBranchButton = Instantiate(submitBranchButtonPrefab, branchTran, false);

            // ボタン設定
            submitBranchButton.SetUpSubmitBranchButton(branchNums[i], this);

            // List に追加
            submitBranchButtonsList.Add(submitBranchButton);
        }
        yield return null;
    }

    /// <summary>
    /// 分岐先の決定
    /// </summary>
    /// <param name="rootNo"></param>
    public void SubmitBranch(int rootNo) {
        for (int i = 0; i < submitBranchButtonsList.Count; i++) {
            // 分岐のボタンを非活性化して重複タップを防止
            submitBranchButtonsList[i].InactivateSubmitButton();
            Destroy(submitBranchButtonsList[i].gameObject);
        }
        submitBranchButtonsList.Clear();

        submitBranchNo = rootNo;
        isSubmitBranch = true;
    }

    /// <summary>
    /// 分岐情報の取得
    /// </summary>
    /// <returns></returns>
    public (bool, int) GetSubmitBranchNo() {
        return (isSubmitBranch, submitBranchNo);
    }


    public void GenerateResultCanvas(int score, int secretPoint) {
        Instantiate(resultCanvas).SetUpResultCanvas(score, secretPoint);
    }
}